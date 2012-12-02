using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Sleep
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("usage: Sleep (ms)");
                return;
            }

            int seconds = int.Parse(args[0]);

            Console.WriteLine("Sleeping: " + seconds);
            Thread.Sleep(seconds);
            return;

        }
    }
}
