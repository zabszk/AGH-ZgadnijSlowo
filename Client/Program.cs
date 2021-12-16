using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Client
{
    internal static class Program
    {
        internal static readonly CancellationTokenSource Cts = new ();
        public static readonly UTF8Encoding Encoder = new ();
        public static ClientWordsStorage Words;

        private static void Main(string[] args)
        {
            Console.WriteLine($"ZgadnijSlowo Client, v. {Core.Version.VersionString}");
            Console.WriteLine("Copyright by Łukasz Jurczyk, 2021");
            Console.WriteLine("Licensed under the MIT License.");
            
            if (args.Length != 4 || !IPAddress.TryParse(args[0], out var ip) || !ushort.TryParse(args[1], out var port))
            {
                Console.WriteLine("Syntax: <ip address> <port> <username> <password>");
                return;
            }
            
            Console.CancelKeyPress += delegate {
                Console.WriteLine("Shutting down...");
                Cts.Cancel();
            };
            
            Console.WriteLine("Loading dictionary...");
            Words = new ClientWordsStorage();

            Client.Start(Cts.Token, ip, port, args[2], args[3]).Wait();
        }
    }
}