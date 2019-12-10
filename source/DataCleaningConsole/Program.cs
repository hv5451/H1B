using DataCleaningLib;
using System;
using System.Diagnostics;

namespace DataCleaningConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RunOnce(@"C:\Users\hitvya\Downloads\visa data\2019.csv", @"C:\Users\hitvya\Downloads\transformed2019\", 30);
        }

        public static void RunOnce(string source, string target, int trainingPercent = 30, int amount = -1)
        {
            Console.WriteLine($"Starting processing for {source} data path: {target}");
            Stopwatch w = new Stopwatch();
            w.Start();
            Cleanup.ProcessCsv(source, target, amount);
            Console.WriteLine($"Completed transformation in {w.ElapsedMilliseconds} ms");
            Cleanup.SeperateData(trainingPercent, $"{target}transformed.csv", target);
            Console.WriteLine($"Completed training data sepratation in {w.ElapsedMilliseconds} ms");
        }
    }
}
