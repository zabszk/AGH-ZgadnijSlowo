using System.IO;

namespace Core
{
    public abstract class WordsStorage
    {
        protected abstract void Add(string text);

        protected WordsStorage() => Load();

        private void Load()
        {
            string line;
            var fs = new FileStream("slowa.txt", FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite);

            var reader = new StreamReader(fs);

            while ((line = reader.ReadLine()) != null)
                Add(line);
        }
    }
}