using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DataCleaningLib.Transformers
{
    public class WageTransformer : ITransformer
    {
        private static Regex GetWage = new Regex(@"(\$|^)(\d+)(\.|$)\d*");
        private Writer<VisaSource> csvWriter;

        public WageTransformer(string path)
        {
            path = path + $"{nameof(WageTransformer)}_errors.csv";
            csvWriter = new Writer<VisaSource>(path);
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
            string reason = string.Empty;
            long wage = CalculateWage(source, out reason);
            target.Calculated_Yearly_Wage = wage;
            target.Calculated_WageLevel = GetWageLevel(wage);
            if (!string.IsNullOrEmpty(reason))
            {
                source.Error_Info = reason;
                this.csvWriter.WriteTransformed(source);
            }
        }

        public static long CalculateWage(VisaSource source, out string reason)
        {
            reason = string.Empty;
            long wage = 0;
            PayDuration duration;

            long from = WageExtractor(source.WAGE_RATE_OF_PAY_FROM, out reason);
            long to = WageExtractor(source.WAGE_RATE_OF_PAY_TO, out reason);
            duration = source.WAGE_UNIT_OF_PAY;

            if (from == 0)
            {
                wage = to;
            }
            else if (to == 0)
            {
                wage = from;
            }
            else
            {
                wage = (from + to) / 2;
            }

            wage = CalculateYearlyWage(duration, wage);
            return wage;
        }

        private static long CalculateYearlyWage(PayDuration duration, long wage)
        {
            if (wage == 0)
            {
                return wage;
            }

            switch(duration)
            {
                case PayDuration.BI:
                case PayDuration.BiWeek:
                case PayDuration.Biweekly:
                    return (wage/(2))*52;

                case PayDuration.DAI:
                case PayDuration.Daily:
                    return (wage * 5) * 52;

                case PayDuration.Hour:
                case PayDuration.Hourly:
                case PayDuration.HR:
                    return (wage * 8 * 5) * 52;

                case PayDuration.Month:
                case PayDuration.Monthly:
                case PayDuration.MTH:
                    return (wage/4) * 52;

                case PayDuration.Week:
                case PayDuration.Weekly:
                case PayDuration.WK:
                    return (wage) * 52;

                default:
                    return wage;
            }
        }

        public static long GetWageLevel(long wage)
        {
            if (wage == 0)
            {
                return 0;
            }
            else if (wage < 50000)
            {
                return 1 + wage/10000 + 1;
            }
            else if (wage < 500000)
            {
                return 1 + 50000 / 10000 + wage / 50000 + 1;
            }
            else
            {
                return 1 + 50000 / 10000 + 500000 / 50000 + wage / 500000 + 1;
            }
        }

        private static long WageExtractor(string value, out string reason)
        {
            reason = string.Empty;
            MatchCollection collections = GetWage.Matches(value.Replace(",",""));
            if (collections.Count != 1)
            {
                reason = $"Collection Count {collections.Count} value:{value}";
                return 0;
            }

            Match m = collections[0];
            if (m.Groups.Count < 3)
            {
                reason = $"Match Count {m.Groups.Count} value:{value}";
                return 0;
            }

            string wage = m.Groups[2].Value;
            long actual;

            if (!long.TryParse(wage, out actual))
            {
                reason = $"wage {wage} value:{value}";
                return 0;
            }

            return actual;
        }
    }
}
