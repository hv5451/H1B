using DataCleaningLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DataCleaningConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // GenerateDataSet()
            //MergeDataSet();
            SeparateMergedData();
        }

        public static void SeparateMergedData()
        {
            string folder = @"C:\Users\hitvya\Downloads\final\";
            Cleanup.SeperateData<NewCombined>(30, @"C:\Users\hitvya\Downloads\mergedtxt4.csv", folder);
            Cleanup.SeperateData<NewTransformed>(30, @"C:\Users\hitvya\Downloads\merged4.csv", $"{folder}wage\\");
        }

        public static void MergeDataSet()
        {
            int max = 9;
            int min = 4;
            Dictionary<string, string> csvs = new Dictionary<string, string>();
            Dictionary<string, string> txts = new Dictionary<string, string>();

            string source = @"C:\Users\hitvya\Downloads\";
            string csvDest = $"{source}merged{min}.csv";
            string txtDest = $"{source}mergedtxt{min}.csv";

            for (int i = min; i <= max; i++)
            {
                string dataSource = $"{source}transformed_{min}_201{i}\\";
                string csvfile = $"{dataSource}merged.csv";
                string csvSource = $"{dataSource}transformed.csv";
                string textfile = $"{dataSource}CombinedObserver.csv";

                csvs.Add($"201{i}", csvSource);
                txts.Add($"201{i}", textfile);
            }

            Stopwatch w = new Stopwatch();
            w.Start();
            Cleanup.MergeData(csvs, csvDest);
            Console.WriteLine($"Merges CSV in {w.ElapsedMilliseconds} ms");
            w.Restart();
            Cleanup.MergeDataText(txts, txtDest);
            Console.WriteLine($"Merges txt in {w.ElapsedMilliseconds} ms");
        }

        public static void RunOnce(string source, string target, int trainingPercent = 30, int amount = -1)
        {
            Console.WriteLine($"Starting processing for {source} data path: {target}");
            Stopwatch w = new Stopwatch();
            w.Start();
            Cleanup.ProcessCsv(source, target, amount);
            Console.WriteLine($"Completed transformation in {w.ElapsedMilliseconds} ms");
            Cleanup.SeperateData<Transformed>(trainingPercent, $"{target}transformed.csv", target);
            Console.WriteLine($"Completed training data sepratation in {w.ElapsedMilliseconds} ms");
        }

        public static void GenerateDataSet()
        {
            int max = 9;
            int min = 4;
            for (int i = min; i <= max; i++)
            {
                string source = @"C:\Users\hitvya\Downloads\visa data\";
                string dataSource = $"{source}201{i}.csv";

                string destination = @"C:\Users\hitvya\Downloads\";
                string destinationFolder = $"{destination}transformed_{min}_201{i}\\";

                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }

                RunOnce(dataSource, destinationFolder, 30);
            }
        }
    }
}
