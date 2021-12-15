using System.Collections.Generic;
using Core;

namespace Client
{
    public class ClientWordsStorage : WordsStorage
    {
        public readonly List<string> Words = new();

        protected override void Add(string text) => Words.Add(text);
    }
}