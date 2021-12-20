using System;
using System.IO;

namespace Core
{
    public abstract class WordsStorage
    {
        protected abstract void Add(string text);
        protected WordsStorage() => Load();

        private void Load()
        {
            if (!File.Exists("slowa.txt"))
            {
                Error("Words file (\"slowa.txt\") doesn't exist!");
                return;
            }

            try
            {
                string line;
                var fs = new FileStream("slowa.txt", FileMode.Open, FileAccess.Read,
                    FileShare.ReadWrite);

                var reader = new StreamReader(fs);

                while ((line = reader.ReadLine()) != null)
                    if (line.Length >= 5)
                        Add(line);
            }
            catch (Exception e)
            {
                Error($"Loading words list failed: {e.Message}");
            }
        }
        
        protected virtual void Error(string error)
        {
            Console.WriteLine(error);
            Environment.Exit(0);
        }
    }
}