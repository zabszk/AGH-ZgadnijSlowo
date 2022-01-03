using System;
using System.Collections.Generic;
using Utf8Json;

namespace Server.Config.JsonObjects
{
    public struct PrimaryConfig : IEquatable<PrimaryConfig>
    {
        public string ListeningIp;
        public ushort ListeningPort;
        public ushort MinimumPlayersAmount;
        public ushort PlayersLimit;
        public ushort GameDelay;
        public List<Round> Rounds;
        public string CurrentRound;
        public uint NextGameId;
        public string WebRootPath;
        public bool LiveView;
        public bool VerboseView;

        [SerializationConstructor]
        public PrimaryConfig(string listeningIp, ushort listeningPort, ushort minimumPlayersAmount, ushort playersLimit, ushort gameDelay, List<Round> rounds, string currentRound, uint nextGameId, string webRootPath, bool liveView, bool verboseView)
        {
            ListeningIp = listeningIp;
            ListeningPort = listeningPort;
            MinimumPlayersAmount = minimumPlayersAmount;
            PlayersLimit = playersLimit;
            GameDelay = gameDelay;
            Rounds = rounds;
            CurrentRound = currentRound;
            NextGameId = nextGameId;
            WebRootPath = webRootPath;
            LiveView = liveView;
            VerboseView = verboseView;
        }

        public bool Equals(PrimaryConfig other)
        {
            return ListeningIp == other.ListeningIp && ListeningPort == other.ListeningPort &&
                   PlayersLimit == other.PlayersLimit && MinimumPlayersAmount == other.MinimumPlayersAmount &&
                   GameDelay == other.GameDelay && Rounds == other.Rounds && CurrentRound == other.CurrentRound &&
                   WebRootPath == other.WebRootPath && LiveView == other.LiveView;
        }

        public override bool Equals(object obj)
        {
            return obj is PrimaryConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ListeningIp, ListeningPort, PlayersLimit, GameDelay, Rounds, CurrentRound, WebRootPath, LiveView);
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

    public readonly struct Round : IEquatable<Round>
    {
        public readonly string ShortName;
        public readonly string Name;
        public readonly int DisplayOrder;

        [SerializationConstructor]
        public Round(string shortName, string name, int displayOrder)
        {
            ShortName = shortName;
            Name = name;
            DisplayOrder = displayOrder;
        }

        public Round SetName(string name) => new(ShortName, name, DisplayOrder);

        public Round SetPriority(int displayOrder) => new(ShortName, Name, displayOrder);

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