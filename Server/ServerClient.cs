using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Server.Config;
using Server.Config.JsonObjects;
using Server.Misc;
using Server.ServerConsole;

namespace Server
{
    public class ServerClient : IDisposable
    {
        private readonly Server _server;
        private readonly TcpClient _client;
        private readonly NetworkStream _s;
        private readonly StreamReader _reader;
        private readonly CancellationToken _token;
        private bool _disposed;
        internal Game Game;
        internal Queue<string> Received;
        internal List<char> GuessedLetters;
        internal ushort ScoredDuringThisGame;
        public User User;
        public readonly Stopwatch TimeoutStopwatch = new();
        private string _username;
        private int _err;

        public string Username => User == null ? "(null)" : _username ?? "(null)";

        internal ServerClient(Server s, TcpClient c, CancellationToken token)
        {
            _server = s;
            _token = token;
            _client = c;
            _s = c.GetStream();
            _reader = new StreamReader(_s);
            
            Logger.Log($"New connection from endpoint {RemoteEndPoint}.", Logger.LogEntryPriority.LiveView, uint.MaxValue);
            
            TimeoutStopwatch.Start();
            Task.Run(Receive, token);
        }

        private async void Receive()
        {
            try
            {
                while (!_token.IsCancellationRequested)
                {
                    await Task.Delay(15, _token);

                    if (!_client.Connected || !_s.CanRead)
                    {
                        Dispose(null, "client disconnection");
                        return;
                    }
                    
                    if (_err > 3)
                    {
                        Dispose(null, "timeout");
                        return;
                    }
                    
                    if (_reader.EndOfStream)
                    {
                        _err++;
                        continue;
                    }

                    string read = await _reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(read))
                    {
                        _err++;
                        continue;
                    }

                    _err = 0;

                    if (_username == null)
                    {
                        if (read.Length > 32 || !ConfigManager.Users.ContainsKey(read))
                        {
                            Dispose("-", "auth failed - username");
                            return;
                        }

                        _username = read;
                        continue;
                    }
                    
                    if (User == null)
                    {
                        if (read.Length > 64)
                        {
                            Dispose("-", "auth failed - too long password");
                            return;
                        }
                        
                        var u = ConfigManager.Users[_username];
                        if (u.Suspended || !u.Password.Equals(Sha.HashToString(Sha.Sha512(read)), StringComparison.Ordinal))
                        {
                            _username = null;
                            Dispose("-", "auth failed - password");
                            return;
                        }

                        TimeoutStopwatch.Reset();
                        Received = new Queue<string>();
                        GuessedLetters = new List<char>(10);
                        User = u;
                        User.LastLogin = DateTime.UtcNow;
                        
                        Logger.Log($"Player {Username} authenticated from endpoint {RemoteEndPoint}.", Logger.LogEntryPriority.LiveView, uint.MaxValue);
                        WriteText("+");
                        
                        for (int i = 0; i < _server.Clients.Count; i++)
                            if (_server.Clients[i] != this && _server.Clients[i].Username.Equals(Username, StringComparison.OrdinalIgnoreCase))
                            {
                                _server.Clients[i].Dispose();
                                break;
                            }
                        
                        _server.EnqueueClient(this);
                        continue;
                    }
                    
                    Received.Enqueue(read);
                }
            }
            catch
            {
                Dispose(null, "read exception");
            }
            finally
            {
                Dispose(null);
            }
        }

        internal void WriteText(string text)
        {
            try
            {
                if (!_s.CanWrite) return;
                var encoded = Program.Encoder.GetBytes(text + "\r\n");
                _s.Write(encoded, 0, encoded.Length);
            }
            catch
            {
                //Ignore
            }
        }

        public void Dispose() => Dispose("?");

        public void Dispose(string terminationText, string terminationReason = null)
        {
            if (_disposed)
                return;

            _disposed = true;

            GC.SuppressFinalize(this);
            if (ScoredDuringThisGame > 0)
            {
                if (User.Score.ContainsKey(ConfigManager.PrimaryConfig.CurrentRound))
                    User.Score[ConfigManager.PrimaryConfig.CurrentRound] += ScoredDuringThisGame;
                else User.Score.Add(ConfigManager.PrimaryConfig.CurrentRound, ScoredDuringThisGame);
            }

            if (Game != null && Game.Players.Contains(this))
            {
                lock (Game.PlayersListLock)
                {
                    Game.Players.Remove(this);
                }

                Game.PlayersChanged();
            }
            
            Game = null;
            
            try
            {
                if (terminationText != null)
                    WriteText(terminationText);
                
                _s.Flush();
            }
            catch
            {
                //Ignore
            }

            try
            {
                Logger.Log($"Connection from endpoint {RemoteEndPoint} has been terminated{(terminationReason == null ? "" : $" ({terminationReason})")}. Authenticated user: {Username}.", Logger.LogEntryPriority.LiveView, uint.MaxValue);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to print connection termination message: {e.Message}", Logger.LogEntryPriority.Error);
            }
            
            TimeoutStopwatch.Reset();
            _reader.Dispose();
            _s.Dispose();
            _client.Dispose();

            _server.RemoveClient(this);
        }

        public string RemoteEndPoint => _client.Client.RemoteEndPoint == null ? "(null)" : _client.Client.RemoteEndPoint.ToString();
        
        public override string ToString() => $"{(User == null ? "(unauthenticated)" : Username)} [{RemoteEndPoint}]: {(Game == null ? "---" : Game.ToString())}";
    }
}