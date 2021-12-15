using System.Collections.Generic;
using Core;

namespace Server.Config
{
    public class ServerWordsStorage : WordsStorage
    {
        public readonly HashSet<string> Words = new();

        protected override void Add(string text) => Words.Add(text);
    }
}