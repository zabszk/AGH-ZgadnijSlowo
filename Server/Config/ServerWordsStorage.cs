using System.Collections.Generic;
using Core;
using Server.ServerConsole;

namespace Server.Config
{
    public class ServerWordsStorage : WordsStorage
    {
        public readonly List<string> Words = new();

        protected override void Add(string text) => Words.Add(text);

        protected override void Error(string error)
        {
            Words.Clear();
            Logger.Log(error, Logger.LogEntryPriority.Critical);
        }
    }
}