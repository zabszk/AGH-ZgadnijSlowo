using System;
using System.Collections.Generic;
using Utf8Json;

namespace Server.Config.JsonObjects
{
    public readonly struct Scoreboard : IEquatable<Scoreboard>
    {
        public readonly long Timestamp;
        public readonly string ServerVersion;
        public readonly List<Round> Rounds;
        public readonly CurrentConfig ServerConfig;
        public readonly List<ScoreboardUser> Scores;
        
        [SerializationConstructor]
        public Scoreboard(long timestamp, string serverVersion, List<Round> rounds, CurrentConfig serverConfig, List<ScoreboardUser> scores)
        {
            Timestamp = timestamp;
            ServerVersion = serverVersion;
            Rounds = rounds;
            ServerConfig = serverConfig;
            Scores = scores;
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
        public readonly ushort GameDelay;

        [SerializationConstructor]
        public CurrentConfig(string activeRound, ushort playersLimit, ushort gameDelay)
        {
            ActiveRound = activeRound;
            PlayersLimit = playersLimit;
            GameDelay = gameDelay;
        }

        public bool Equals(CurrentConfig other)
        {
            return ActiveRound == other.ActiveRound && PlayersLimit == other.PlayersLimit && GameDelay == other.GameDelay;
        }

        public override bool Equals(object obj)
        {
            return obj is CurrentConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ActiveRound, PlayersLimit, GameDelay);
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

    public readonly struct ScoreboardUser : IEquatable<ScoreboardUser>
    {
        public readonly string Username;
        public readonly Dictionary<string, int> Score;
        public readonly long LastLogin;
        
        [SerializationConstructor]
        public ScoreboardUser(string username, Dictionary<string, int> score, long lastLogin)
        {
            Username = username;
            Score = score;
            LastLogin = lastLogin;
        }

        public bool Equals(ScoreboardUser other)
        {
            return Username == other.Username && Equals(Score, other.Score) && LastLogin == other.LastLogin;
        }

        public override bool Equals(object obj)
        {
            return obj is ScoreboardUser other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Score, LastLogin);
        }

        public static bool operator ==(ScoreboardUser left, ScoreboardUser right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ScoreboardUser left, ScoreboardUser right)
        {
            return !left.Equals(right);
        }
    }
}