using System;
using System.Collections.Generic;
using System.Linq;
using Server.ServerConsole.Commands;
using Utf8Json;

namespace Server.Config.JsonObjects
{
    public readonly struct Scoreboard : IEquatable<Scoreboard>
    {
        public readonly DateTime Timestamp;
        public readonly string ServerVersion;
        public readonly List<Round> Rounds;
        public readonly CurrentConfig ServerConfig;
        public readonly List<ScoreboardUser> Scores;
        public readonly List<ScoreboardGame> GamesInProgress;
        
        [SerializationConstructor]
        public Scoreboard(DateTime timestamp, string serverVersion, List<Round> rounds, CurrentConfig serverConfig, List<ScoreboardUser> scores, List<ScoreboardGame> gamesInProgress)
        {
            Timestamp = timestamp;
            ServerVersion = serverVersion;
            ServerConfig = serverConfig;
            Scores = scores;
            Scores.Sort();
            GamesInProgress = gamesInProgress;
            Rounds = new();

            foreach (var r in rounds)
                if (r.DisplayOrder >= 0)
                    Rounds.Add(r);
        }

        public bool Equals(Scoreboard other)
        {
            return Timestamp == other.Timestamp && Equals(Rounds, other.Rounds) && ServerConfig.Equals(other.ServerConfig) && Equals(Scores, other.Scores);
        }

        public override bool Equals(object obj)
        {
            return obj is Scoreboard other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Timestamp, Rounds, ServerConfig, Scores);
        }

        public static bool operator ==(Scoreboard left, Scoreboard right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Scoreboard left, Scoreboard right)
        {
            return !left.Equals(right);
        }
    }
    
    public readonly struct CurrentConfig : IEquatable<CurrentConfig>
    {
        public readonly string ActiveRound;
        public readonly ushort PlayersLimit;
        public readonly ushort MinimumPlayersAmount;
        public readonly ushort GameDelay;

        [SerializationConstructor]
        public CurrentConfig(string activeRound, ushort playersLimit, ushort minimumPlayersAmount, ushort gameDelay)
        {
            ActiveRound = activeRound;
            PlayersLimit = playersLimit;
            MinimumPlayersAmount = minimumPlayersAmount;
            GameDelay = gameDelay;
        }

        public bool Equals(CurrentConfig other)
        {
            return ActiveRound == other.ActiveRound && PlayersLimit == other.PlayersLimit &&
                   MinimumPlayersAmount == other.MinimumPlayersAmount && GameDelay == other.GameDelay;
        }

        public override bool Equals(object obj)
        {
            return obj is CurrentConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ActiveRound, PlayersLimit, MinimumPlayersAmount, GameDelay);
        }

        public static bool operator ==(CurrentConfig left, CurrentConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CurrentConfig left, CurrentConfig right)
        {
            return !left.Equals(right);
        }
    }

    public readonly struct ScoreboardUser : IEquatable<ScoreboardUser>, IComparable<ScoreboardUser>
    {
        public readonly string Username;
        public readonly Dictionary<string, UserRound> Score;

        [SerializationConstructor]
        public ScoreboardUser(string username, Dictionary<string, UserRound> score)
        {
            Username = username;
            Score = new();
            
            foreach (var s in score)
            {
                var r = ConfigManager.PrimaryConfig.Rounds.FirstOrDefault(r => r.ShortName.Equals(s.Key, StringComparison.OrdinalIgnoreCase));
                if (r != default && r.DisplayOrder >= 0)
                    Score.Add(s.Key, s.Value);
            }
        }

        private uint ScoreInCurrentRound() => Score.ContainsKey(ConfigManager.PrimaryConfig.CurrentRound)
            ? Score[ConfigManager.PrimaryConfig.CurrentRound].Score
            : 0;

        public bool Equals(ScoreboardUser other)
        {
            return Username == other.Username && Equals(Score, other.Score);
        }

        public override bool Equals(object obj)
        {
            return obj is ScoreboardUser other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Score);
        }

        public static bool operator ==(ScoreboardUser left, ScoreboardUser right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ScoreboardUser left, ScoreboardUser right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(ScoreboardUser other)
        {
            var cmp = -ScoreInCurrentRound().CompareTo(other.ScoreInCurrentRound());
            return cmp == 0 ? string.Compare(Username, other.Username, StringComparison.Ordinal) : cmp;
        }

        public static bool operator <(ScoreboardUser left, ScoreboardUser right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(ScoreboardUser left, ScoreboardUser right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(ScoreboardUser left, ScoreboardUser right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(ScoreboardUser left, ScoreboardUser right)
        {
            return left.CompareTo(right) >= 0;
        }
    }

    public readonly struct ScoreboardGame : IEquatable<ScoreboardGame>
    {
        public readonly uint InternalId;
        public readonly int TimeElapsed;
        public readonly List<string> Players;
        
        [SerializationConstructor]
        public ScoreboardGame(uint internalId, int timeElapsed, List<string> players)
        {
            InternalId = internalId;
            TimeElapsed = timeElapsed;
            Players = players;
        }

        internal ScoreboardGame(Game game)
        {
            InternalId = game.InternalId;
            TimeElapsed = game.InProgress ? -2 : MaintenanceCommand.NoAutoStart ? -3 : game.ToStart.IsRunning ? (int)game.ToStart.Elapsed.TotalSeconds : -1;
            Players = new();

            lock (game.PlayersListLock)
            {
                foreach (var pl in game.Players)
                    Players.Add(pl.Username);
            }
        }

        public bool Equals(ScoreboardGame other)
        {
            return InternalId == other.InternalId && TimeElapsed == other.TimeElapsed && Equals(Players, other.Players);
        }

        public override bool Equals(object obj)
        {
            return obj is ScoreboardGame other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InternalId, TimeElapsed, Players);
        }

        public static bool operator ==(ScoreboardGame left, ScoreboardGame right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ScoreboardGame left, ScoreboardGame right)
        {
            return !left.Equals(right);
        }
    }
}