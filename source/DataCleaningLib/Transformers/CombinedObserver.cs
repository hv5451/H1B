using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataCleaningLib.Transformers
{
    class CombinedObserver : ITransformer
    {
        private Regex splitter = new Regex(@"(\w+)");
        private const string WageLevelId = "CalculatedWageLevel_";
        private Writer<Combined> csvWriter;
        private const string separator = " ";

        public CombinedObserver(string path)
        {
            csvWriter = new Writer<Combined>(path + $"{nameof(CombinedObserver)}.csv");
        }

        public void End()
        {
            this.csvWriter?.Flush();
            this.csvWriter?.Dispose();
        }

        public void Start()
        {
        }

        public void TransformRow(VisaSource source, Transformed target)
        {
            HashSet<string> together = new HashSet<string>();
            ISet<string> jobs = this.FeatureExtractor(source.JOB_TITLE.ToLower());
            together.UnionWith(jobs);
            ISet<string> company = this.FeatureExtractor(source.EMPLOYER_NAME.ToLower());
            together.UnionWith(company);
            ISet<string> soc_code = this.FeatureExtractor(source.SOC_CODE.ToLower());
            together.UnionWith(soc_code);
            ISet<string> soc_name = this.FeatureExtractor(source.SOC_NAME.ToLower());
            together.UnionWith(soc_name);
            ISet<string> Naics = this.FeatureExtractor(source.NAICS_CODE.ToString().ToLower());
            together.UnionWith(Naics);

            string paySource = source.PW_SOURCE_OTHER;
            paySource = paySource + "," + source.PW_SOURCE.ToString();

            if (paySource == ",")
            {
                paySource = "paySource_Unknown";
            }

            ISet<string> WageSource = this.FeatureExtractor(paySource.ToLower());
            together.UnionWith(WageSource);

            together.Add(this.GetWageLevel(source));

            if (source.SECONDARY_ENTITY == Decision.Y)
            {
                together.Add($"{nameof(source.SECONDARY_ENTITY)}");
            }

            var d = new Combined()
            {
                CaseNumber = source.CASE_NUMBER,
                Decision = source.CASE_STATUS == CaseStatus.CERTIFIED ? 1 : 0,
                Together = string.Join(separator, together)
            };

            this.csvWriter.WriteTransformed(d);
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

        private string GetWageLevel(VisaSource source)
        {
            long wage = WageTransformer.CalculateWage(source, out string reason);
            return $"{WageLevelId}{WageTransformer.GetWageLevel(wage)}";
        }

        private int GetWorkerLevel(int workers)
        {
            return workers/1000;
        }
    }

    public class Combined
    {
        public string CaseNumber { get; set; }

        public int Decision { get; set; }

        public string Together { get; set; }
    }
}
