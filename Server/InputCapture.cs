using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal static class InputCapture
    {
        public delegate void InputCaptured(string message);

        public static event InputCaptured InputCapturedEvent;

        internal static void Start(CancellationToken token)
        {
            new Thread(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var line = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        InputCapturedEvent?.Invoke(line);
                }
            })
            {
                Name = "Input capture",
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            }.Start();
        }
    }
}