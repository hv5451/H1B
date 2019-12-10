using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DataCleaningLib.Transformers
{
    public abstract class TextObserver
    {
        private string path;
        private Dictionary<string, ValueStats> stats = new Dictionary<string, ValueStats>();
        private Regex splitter = new Regex(@"(\w+)");
        private Writer<Stats> csvWriter;


        public TextObserver(string path)
        {
            this.path = path;
            csvWriter = new Writer<Stats>(path.Replace("json", "csv"));
        }

        public void UpdateMap(string value, bool isCertified)
        {
            value = value.ToLower();
            ISet<string> keys = this.FeatureExtractor(value);

            foreach(string key in keys)
            {
                this.csvWriter.WriteTransformed(new Stats() {
                    Decision = isCertified ? 1 : 0,
                    Key = key
                });

                if (this.stats.ContainsKey(key))
                {
                    if (isCertified)
                    {
                        this.stats[key].CertifiedCount++;
                    }
                    else
                    {
                        this.stats[key].DeniedCount++;
                    }
                }
                else
                {
                    this.stats[key] = new ValueStats()
                    {
                        CertifiedCount = isCertified ? 1 : 0,
                        DeniedCount = isCertified ? 0 : 1,
                        Key = key
                    };
                }

                if (isCertified)
                {
                    this.stats[key].AllCertifiedValues.Add(value);
                }
                else
                {
                    this.stats[key].AllDeniedValues.Add(value);
                }

                this.stats[key].DenielPercentage = (this.stats[key].DeniedCount*(double)100.00) / (this.stats[key].DeniedCount + this.stats[key].CertifiedCount);
            }
        }

        protected virtual ISet<string> FeatureExtractor(string value)
        {
            MatchCollection collections = this.splitter.Matches(value);
            HashSet<string> maps = new HashSet<string>();
            
            foreach (Match m in collections)
            {
                maps.Add(m.Value);
            }

            return maps;
        }

        public void SaveFile()
        {
            string json = JsonConvert.SerializeObject(stats.Values);
            File.WriteAllText(this.path, json);

            this.csvWriter?.Flush();
            this.csvWriter?.Dispose();
        }
    }

    public class ValueStats
    {
        public ValueStats()
        {
            this.AllCertifiedValues = new HashSet<string>();
            this.AllDeniedValues = new HashSet<string>();
        }

        public string Key { get; set; }

        public int CertifiedCount { get; set; }

        public int DeniedCount { get; set; }

        public double DenielPercentage { get; set; }

        public ISet<string> AllCertifiedValues { get; }

        public ISet<string> AllDeniedValues { get; }
    }

    public class Stats
    {
        public string Key { get; set; }
        public int Decision { get; set; }
    }
}
