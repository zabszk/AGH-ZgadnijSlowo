using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server.Misc;

namespace Client
{
    internal static class Client
    {
        private static TcpClient _client;
        private static NetworkStream _s;
        private static StreamReader _reader;
        private static List<char> _availableLetters;
        private static char[] _guessedLetters;
        private static char _lastGuessed;
        private static int _i;

        internal static async Task Start(CancellationToken token, IPAddress ip, int port, string username, string password)
        {
            try
            {
                Console.WriteLine($"Connecting to {ip}:{port}...");
                _i = 0;
                _guessedLetters = null;
                _lastGuessed = default;
                _availableLetters = null;
                _client = new();
                await _client.ConnectAsync(ip, port, token);
                Console.WriteLine($"Connected to {ip}:{port}");

                _s = _client.GetStream();
                _reader = new StreamReader(_s);
                WriteText(username);
                WriteText(password);

                var line = await _reader.ReadLineAsync();
                if (line is "+1" or "+2")
                    Console.WriteLine("Authentication successful.");
                else
                {
                    Console.WriteLine("Authentication failed!");
                    return;
                }

                bool capturePositions = false;
                _availableLetters = new(26);
                StringBuilder sb = new();

                for (byte c = 97; c <= 122; c++)
                    _availableLetters.Add(Convert.ToChar(c));

                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(15, token);

                    if (!_client.Connected || !_s.CanRead || _reader.EndOfStream)
                    {
                        Program.Cts.Cancel();
                        break;
                    }

                    line = await _reader.ReadLineAsync();
                    Console.WriteLine($"[RECV] {line ?? "(null)"}");

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    if (capturePositions)
                    {
                        capturePositions = false;

                        if (_lastGuessed != '\0')
                        {
                            for (int j = 0; j < line.Length; j++)
                                if (line[j] == '1')
                                    _guessedLetters[j] = _lastGuessed;
                            Guess();
                            _i++;
                            continue;
                        }
                        
                        sb.Append("Currently guessed: ");

                        foreach (var c in _guessedLetters)
                            sb.Append(c);

                        Console.WriteLine(sb.ToString());
                        sb.Clear();
                    }

                    switch (line)
                    {
                        case "?":
                            Console.WriteLine("Server requested disconnection.");
                            return;

                        case "@":
                            WriteText(Program.Words.Words[
                                SecureRandomGenerator.RandomInt(0, Program.Words.Words.Count - 1)]);
                            break;

                        case "!":
                        case "#":
                            if (_i < 10)
                            {
                                Guess();
                                _i++;
                            }
                            break;

                        case "=":
                            if (_lastGuessed != '\0')
                                capturePositions = true;
                            break;

                        default:
                            if (_i == 0)
                            {
                                _guessedLetters = new char[line.Length];
                                for (int j = 0; j < line.Length; j++)
                                    _guessedLetters[j] = '?';

                                Guess();
                                
                                _i++;
                            }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                _client.Dispose();
                Program.Cts.Cancel();
            }
        }

        private static void Guess()
        {
            if (_i < 9)
            {
                int r = SecureRandomGenerator.RandomInt(0, _availableLetters.Count - 1);
                _lastGuessed = _availableLetters[r];
                WriteText("+");
                WriteText(_lastGuessed.ToString());
                _availableLetters.RemoveAt(r);
                return;
            }

            _lastGuessed = '\0';
            WriteText("=");
            WriteText(Program.Words.Words[SecureRandomGenerator.RandomInt(0, Program.Words.Words.Count - 1)]);
        }
        
        private static void WriteText(string text)
        {
            Console.WriteLine($"[SENT] {text ?? "(null)"}");
            
            try
            {
                if (!_s.CanWrite) return;
                var encoded = Program.Encoder.GetBytes(text + "\r\n");
                _s.Write(encoded, 0, encoded.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("[WriteText] Exception: " + e.Message);
            }
        }
    }
}