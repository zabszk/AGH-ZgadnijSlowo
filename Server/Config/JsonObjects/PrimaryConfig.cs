using System;
using System.Collections.Generic;
using Utf8Json;

namespace Server.Config.JsonObjects
{
    public struct PrimaryConfig : IEquatable<PrimaryConfig>
    {
        public string ListeningIp;
        public ushort ListeningPort;
        public ushort PlayersLimit;
        public ushort GameDelay;
        public List<Round> Rounds;
        public string CurrentRound;
        public uint NextGameId;
        public string WebRootPath;

        [SerializationConstructor]
        public PrimaryConfig(string listeningIp, ushort listeningPort, ushort playersLimit, ushort gameDelay, List<Round> rounds, string currentRound, uint nextGameId, string webRootPath)
        {
            ListeningIp = listeningIp;
            ListeningPort = listeningPort;
            PlayersLimit = playersLimit;
            GameDelay = gameDelay;
            Rounds = rounds;
            CurrentRound = currentRound;
            NextGameId = nextGameId;
            WebRootPath = webRootPath;
        }

        public bool Equals(PrimaryConfig other)
        {
            return ListeningIp == other.ListeningIp && ListeningPort == other.ListeningPort &&
                   PlayersLimit == other.PlayersLimit && GameDelay == other.GameDelay && Rounds == other.Rounds &&
                   CurrentRound == other.CurrentRound && WebRootPath == other.WebRootPath;
        }

        public override bool Equals(object obj)
        {
            return obj is PrimaryConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ListeningIp, ListeningPort, PlayersLimit, GameDelay, Rounds, CurrentRound, WebRootPath);
        }

        public static bool operator ==(PrimaryConfig left, PrimaryConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PrimaryConfig left, PrimaryConfig right)
        {
            return !left.Equals(right);
        }
    }

    public struct Round : IEquatable<Round>
    {
        public string ShortName;
        public string Name;
        public int DisplayOrder;

        [SerializationConstructor]
        public Round(string shortName, string name, int displayOrder)
        {
            ShortName = shortName;
            Name = name;
            DisplayOrder = displayOrder;
        }

        public void SetName(string name) => Name = name;
        
        public void SetPriority(int displayOrder) => DisplayOrder = displayOrder;

        public bool Equals(Round other)
        {
            return ShortName == other.ShortName && Name == other.Name && DisplayOrder == other.DisplayOrder;
        }

        public override bool Equals(object obj)
        {
            return obj is Round other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ShortName, Name, DisplayOrder);
        }

        public static bool operator ==(Round left, Round right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Round left, Round right)
        {
            return !left.Equals(right);
        }
    }
}