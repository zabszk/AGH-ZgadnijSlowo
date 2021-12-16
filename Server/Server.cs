using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Server.Config;
using Server.ServerConsole;

namespace Server
{
    internal class Server : IDisposable
    {
        private readonly TcpListener _listener;
        internal readonly List<ServerClient> Clients = new();
        public readonly List<Game> GameQueue = new();
        private CancellationToken _token;
        public static volatile uint InternalRoundId = 0;
        private volatile bool _disposed;

        public readonly object GameQueueLock = new(), ClientsListLock = new();

        internal Server(IPEndPoint ep) => _listener = new TcpListener(ep);

        internal async Task Start(CancellationToken token)
        {
            try
            {
                _token = token;
                _listener.Start();
                Logger.Log($"Server started at endpoint {_listener.LocalEndpoint}");
                while (!token.IsCancellationRequested)
                {
                    await Task.Run(async () =>
                    {
                        try
                        {
                            var connection = await _listener.AcceptTcpClientAsync();
                            if (_disposed)
                                return;
                            
                            lock (Program.Server.ClientsListLock)
                            {
                                Clients.Add(new ServerClient(this, connection, token));
                            }
                        }
                        catch (Exception e)
                        {
                            if (!_disposed)
                            {
                                Logger.Log($"Server listening exception: {e.Message}",
                                    Logger.LogEntryPriority.Critical);
                            }
                        }

                    }, token);

                }
            }
            catch (TaskCanceledException)
            {
                //Ignore
            }
            finally
            {
                Dispose();
            }
        }

        internal void RemoveClient(ServerClient c)
        {
            lock (Program.Server.ClientsListLock)
            {
                Clients.Remove(c);
            }
        }

        internal void EnqueueClient(ServerClient c)
        {
            lock (GameQueueLock)
            {
                foreach (var game in GameQueue)
                {
                    if (game.InProgress || game == c.Game || game.Players.Count >= ConfigManager.PrimaryConfig.PlayersLimit)
                        continue;

                    foreach (var players in game.Players)
                    {
                        if (players.Username.Equals(c.Username, StringComparison.OrdinalIgnoreCase))
                        {
                            c.Dispose();
                            return;
                        }
                    }

                    AssignPlayer(game, c);
                    return;
                }

                Game g = new(this, _token);

                GameQueue.Add(g);

                AssignPlayer(g, c);
            }
        }

        private void AssignPlayer(Game g, ServerClient c)
        {
            lock (g.PlayersListLock)
            {
                g.Players.Add(c);
            }

            c.Game = g;
            g.PlayersChanged();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            
            for (int i = Clients.Count - 1; i >= 0; i--)
                Clients[i].Dispose();

            lock (GameQueueLock)
            {
                for (int i = GameQueue.Count - 1; i >= 0; i--)
                    GameQueue[i].Terminate();
            }

            _listener.Stop();
        }

        internal async Task TimeoutClients(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    lock (ClientsListLock)
                    {
                        for (int i = Clients.Count - 1; i >= 0; i--)
                        {
                            try
                            {
                                if (Clients[i].TimeoutStopwatch.Elapsed.TotalSeconds > 3)
                                {
                                    Clients[i].WriteText("Authentication timeout");
                                    Clients[i].Dispose("?", "authentication timeout");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Log($"Inner timeouting exception: {e.Message}", Logger.LogEntryPriority.Error);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log($"Timeouting exception: {e.Message}", Logger.LogEntryPriority.Critical);
                }

                try
                {
                    await Task.Delay(750, token);
                }
                catch (TaskCanceledException)
                {
                    //Ignore
                }
            }
        }
    }
}