using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Config;
using Server.Misc;
using Server.ServerConsole;
using Server.ServerConsole.Commands;
using static System.FormattableString;

namespace Server
{
    internal class Game
    {
        private readonly Server _s;

        internal readonly uint InternalId;
        private uint _gameId = uint.MaxValue;
        public bool InProgress;
        public readonly Stopwatch ToStart = new();
        public readonly List<ServerClient> Players = new();
        private readonly Queue<string> _log = new();
        private readonly CancellationToken _token, _localToken;
        private readonly CancellationTokenSource _localTokenSource = new ();
        private string _word;
        
        public readonly object PlayersListLock = new ();
        
        private static readonly Dictionary<char, int> Lines = new()
        {
            ['a'] = 1,
            ['ą'] = 1,
            ['b'] = 3,
            ['c'] = 1,
            ['ć'] = 1,
            ['d'] = 3,
            ['e'] = 1,
            ['ę'] = 1,
            ['f'] = 4,
            ['g'] = 2,
            ['h'] = 3,
            ['i'] = 1,
            ['j'] = 2,
            ['k'] = 3,
            ['l'] = 3,
            ['ł'] = 3,
            ['m'] = 1,
            ['n'] = 1,
            ['ń'] = 1,
            ['o'] = 1,
            ['ó'] = 1,
            ['p'] = 2,
            ['q'] = 2,
            ['r'] = 1,
            ['s'] = 1,
            ['ś'] = 1,
            ['t'] = 3,
            ['u'] = 1,
            ['v'] = 1,
            ['w'] = 1,
            ['x'] = 1,
            ['y'] = 2,
            ['z'] = 1,
            ['ź'] = 1,
            ['ż'] = 1
        };

        internal Game(Server server, CancellationToken token)
        {
            _s = server;
            _token = token;
            InternalId = Interlocked.Increment(ref Server.InternalRoundId);

            _localToken = _localTokenSource.Token;
            Task.Run(GameTask, _localToken);
        }

        internal void PlayersChanged(bool maintenance = false)
        {
            if (Players.Count == 0 && !maintenance)
            {
                lock (_s.GameQueueLock)
                {
                    if (_s.GameQueue.Any(g => !g.InProgress && g != this))
                    {
                        _localTokenSource.Cancel();
                        return;
                    }
                }
            }

            if (MaintenanceCommand.NoAutoStart)
            {
                ToStart.Reset();
                return;
            }

            if (Players.Count < 2)
                ToStart.Reset();
            else if (!ToStart.IsRunning)
                ToStart.Restart();
        }

        private async Task GameTask()
        {
            try
            {
                //Waiting for players
                while (!_token.IsCancellationRequested && !_localToken.IsCancellationRequested && !InProgress)
                {
                    if (Players.Count == ConfigManager.PrimaryConfig.PlayersLimit ||
                        ToStart.Elapsed.TotalSeconds >= ConfigManager.PrimaryConfig.GameDelay)
                    {
                        InProgress = true;
                        break;
                    }

                    await Task.Delay(2000, _localToken);
                }

                if (_token.IsCancellationRequested || _localToken.IsCancellationRequested)
                    return;

                _log.Enqueue(Invariant($"Game started at: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}Z"));
                _log.Enqueue($"Internal Game ID: {InternalId}");
                _log.Enqueue($"Server version: {Core.Version.VersionString}");
                _log.Enqueue(string.Empty);

                lock (PlayersListLock)
                {
                    foreach (var player in Players)
                        Log($"Player {player.Username} has been accepted into the game.");
                }

                //Selecting word
                while (_word == null && !_token.IsCancellationRequested && !_localToken.IsCancellationRequested)
                {
                    var selected = SelectRandomPlayer();
                    Log($"Player {selected.Username} has been chosen for selecting word.");
                    selected.WriteText("@");

                    for (int i = 0; i < 100; i++)
                    {
                        await Task.Delay(20, _localToken);
                        if (selected.Received.Count != 0)
                        {
                            _word = selected.Received.Dequeue().ToLowerInvariant();
                            Log($"Player {selected.Username} chose {_word}.");
                            break;
                        }
                    }

                    if (_word != null && !ConfigManager.WordsStorage.Words.Contains(_word))
                        _word = null;

                    selected.Dispose();

                    if (_word == null && Players.Count < 2)
                    {
                        lock (_s.GameQueueLock)
                        {
                            _s.GameQueue.Remove(this);
                        }
                        ReallocatePlayers();
                        return;
                    }
                }

                Log($"Word {_word} has been accepted.");
                if (_token.IsCancellationRequested || _localToken.IsCancellationRequested)
                    return;

                var sb = new StringBuilder(_word.Length);
                foreach (char c in _word)
                    sb.Append(Lines[c]);

                string s = sb.ToString();
                sb.Clear();
                Log($"Word {_word} has been converted to {s}.");

                lock (PlayersListLock)
                {
                    foreach (var c in Players)
                        c.WriteText(s);
                }

                if (_token.IsCancellationRequested)
                    return;

                List<ServerClient> cls = new(Players.Count);
                List<ServerClient> clsToRemove = new(Players.Count);
                _gameId = ConfigManager.PrimaryConfig.NextGameId++;
                bool endGuessing = false;
                Log($"GameID {_gameId} has been assigned.");

                //Playing
                for (int i = 0; i < 10 && !_token.IsCancellationRequested && !_localToken.IsCancellationRequested && Players.Count > 0 && !endGuessing; i++)
                {
                    if (Players.Count == 0)
                    {
                        lock (_s.GameQueueLock)
                        {
                            _s.GameQueue.Remove(this);
                        }
                        return;
                    }

                    cls.Clear();
                    lock (PlayersListLock)
                    {
                        foreach (var pl in Players)
                        {
                            pl.Guessing = ServerClient.GuessingMode.None;
                            cls.Add(pl);
                        }
                    }

                    for (int j = 0; j < 500 && cls.Count > 0; j++)
                    {
                        await Task.Delay(20, _localToken);

                        try
                        {
                            foreach (var c in cls)
                            {
                                try
                                {
                                    if (c.Received.Count <= 0) continue;
                                    string r = c.Received.Dequeue().ToLowerInvariant();

                                    if (j > 100)
                                    {
                                        Log($"Player {c.Username}'s input has been ignored (timeout).");
                                        c.WriteText("#");
                                    }
                                    else
                                        switch (c.Guessing)
                                        {
                                            case ServerClient.GuessingMode.GuessingWord
                                                when r.Equals(_word, StringComparison.Ordinal):
                                                c.Guessing = ServerClient.GuessingMode.None;
                                                clsToRemove.Add(c);
                                                c.WriteText("=");
                                                c.ScoredDuringThisGame += 5;
                                                c.WriteText(c.ScoredDuringThisGame.ToString());
                                                Log(
                                                    $"Player {c.Username} guessed the word {r}. Total points awarded for the entire round: {c.ScoredDuringThisGame}.");
                                                endGuessing = true;
                                                c.Dispose();
                                                break;

                                            case ServerClient.GuessingMode.GuessingWord:
                                                c.Guessing = ServerClient.GuessingMode.None;
                                                clsToRemove.Add(c);
                                                c.WriteText("!");
                                                Log(
                                                    $"Player {c.Username} failed to guess the word {(r.Length > 1 ? r : "(invalid input)")}. No points awarded.");
                                                break;

                                            case ServerClient.GuessingMode.GuessingLetter:
                                            {
                                                c.Guessing = ServerClient.GuessingMode.None;
                                                clsToRemove.Add(c);
                                                sb.Clear();
                                                ushort p = 0;
                                                for (int k = 0; k < _word.Length; k++)
                                                {
                                                    if (_word[k] == r[0])
                                                    {
                                                        p++;
                                                        sb.Append('1');
                                                    }
                                                    else sb.Append('0');
                                                }

                                                if (p == 0)
                                                {
                                                    c.WriteText("!");
                                                    Log(
                                                        $"Player {c.Username} failed to guess letter {r[0]}. No points awarded.");
                                                }
                                                else
                                                {
                                                    c.WriteText("=");
                                                    c.WriteText(sb.ToString());

                                                    if (!c.GuessedLetters.Contains(r[0]))
                                                    {
                                                        c.GuessedLetters.Add(r[0]);
                                                        c.ScoredDuringThisGame += p;
                                                    }
                                                    else p = 0;

                                                    Log(
                                                        $"Player {c.Username} guessed letter {r[0]}. Points awarded: {p}.");
                                                }
                                            }
                                                break;
                                            
                                            case ServerClient.GuessingMode.None:
                                                switch (r[0])
                                                {
                                                    case '=':
                                                        c.Guessing = ServerClient.GuessingMode.GuessingWord;
                                                        Log(
                                                            $"Player {c.Username} entered word guessing.");
                                                        break;

                                                    case '+':
                                                        c.Guessing = ServerClient.GuessingMode.GuessingLetter;
                                                        Log(
                                                            $"Player {c.Username} entered letter guessing.");
                                                        break;

                                                    default:
                                                        Log(
                                                            $"Player {c.Username} has been removed from the game - invalid input: {r}. Total awarded points: {c.ScoredDuringThisGame}.");
                                                        clsToRemove.Add(c);
                                                        c.Dispose();
                                                        continue;
                                                }
                                                break;
                                            
                                            default:
                                                c.Guessing = ServerClient.GuessingMode.None;
                                                Log(
                                                    $"Error occured while processing response from {c.Username}.");
                                                break;
                                        }
                                }
                                catch (Exception e)
                                {
                                    try
                                    {
                                        Logger.Log($"Processing reply of client {c.Username} failed: {e.Message}",
                                            Logger.LogEntryPriority.Error);
                                    }
                                    catch (Exception exception)
                                    {
                                        Logger.Log(
                                            $"Processing exception {e.Message} thrown during processing reply of client failed: {exception.Message}",
                                            Logger.LogEntryPriority.Error);
                                    }
                                }
                            }

                            cls.RemoveAll(c => clsToRemove.Contains(c));
                            clsToRemove.Clear();
                        }
                        catch (Exception e)
                        {
                            Logger.Log($"Failed to process round tick: {e.Message}", Logger.LogEntryPriority.Critical);
                            clsToRemove.Clear();
                        }
                    }

                    foreach (var player in cls)
                    {
                        Log(
                            $"Player {player.Username} has been removed from the game - timeout. Total awarded points: {player.ScoredDuringThisGame}.");
                        player.Dispose();
                    }
                }

                cls.Clear();
                lock (PlayersListLock)
                {
                    cls.AddRange(Players);
                }

                foreach (var player in cls)
                {
                    Log(
                        $"Player {player.Username} finished the game without guessing the word. Total awarded points: {player.ScoredDuringThisGame}.");
                    player.WriteText(_word);
                    player.WriteText(player.ScoredDuringThisGame.ToString());
                    player.Dispose();
                }

                cls.Clear();
            }
            catch (TaskCanceledException)
            {
                //Ignore
            }
            catch (Exception e)
            {
                Logger.Log($"Game round {this} terminated with an exception: {e.Message}", Logger.LogEntryPriority.Critical);
            }
            finally
            {
                lock (_s.GameQueueLock)
                {
                    _s.GameQueue.Remove(this);
                }

                _localTokenSource.Cancel();
                KickEveryone();

                ConfigManager.SavePrimary();
                ConfigManager.SaveUsers();
                WriteLog();
            }
        }

        private ServerClient SelectRandomPlayer()
        {
            lock (PlayersListLock)
            {
                return Players[SecureRandomGenerator.RandomInt(0, Players.Count - 1)];
            }
        }

        private void ReallocatePlayers()
        {
            lock (PlayersListLock)
            {
                foreach (var player in Players)
                    _s.EnqueueClient(player);

                Players.Clear();
            }
        }

        private void Log(string t)
        {
            _log.Enqueue(Invariant($"[{DateTime.UtcNow:HH:mm:ss.fff}] {t}"));
            Logger.Log(t, Logger.LogEntryPriority.LiveView, _gameId);
        }

        private void WriteLog()
        {
            if (_gameId == uint.MaxValue)
            {
                Logger.Log("Skipping writing logs - no Game ID assigned.");
                return;
            }
            
            try
            {
                Logger.Log($"Writing logs for game {_gameId}.");
                if (!Directory.Exists(PathManager.RoundLogsPath))
                    Directory.CreateDirectory(PathManager.RoundLogsPath);
            
                var fs = new FileStream(Invariant($"{PathManager.RoundLogsPath}{_gameId}-{DateTime.UtcNow:yyyy-mm-dd-HH-mm-ss}.log"), FileMode.Create, FileAccess.Write,
                    FileShare.ReadWrite);

                var wr = new StreamWriter(fs)
                {
                    AutoFlush = true
                };

                while (_log.Count > 0)
                    wr.WriteLine(_log.Dequeue());
                
                wr.Flush();
                wr.Dispose();
                fs.Close();
                Logger.Log($"Completed writing logs for game {_gameId}.");
            }
            catch (Exception e)
            {
                Logger.Log($"Writing logs for game {_gameId} failed. Exception: {e.Message}", Logger.LogEntryPriority.Critical);
            }
        }

        internal void Terminate()
        {
            Log("Game has been administratively terminated.");
            _localTokenSource.Cancel();
        }

        private void KickEveryone()
        {
            for (int i = Players.Count - 1; i >= 0; i--)
                Players[i].Dispose();
        }

        public override string ToString() =>
            $"{InternalId}{(_gameId == uint.MaxValue ? "" : $" ({_gameId})")} {(InProgress ? "In Progress" : MaintenanceCommand.NoAutoStart ? "Administrative Hold" : $"{ToStart.Elapsed.TotalSeconds:F2}/{ConfigManager.PrimaryConfig.GameDelay}")}, {Players.Count} players";
    }
}