using DataCleaningLib.Transformers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataCleaningLib
{
    public class Cleanup
    {
        public static void ProcessCsv(string sourceFile, string destinationPath, int maxRecords = -1)
        {
            int processed = 0;
            ITransformer transformer = new CompositeTransformer(destinationPath);
            RawDataTransformer raw = new RawDataTransformer(destinationPath);

            using (Reader<ImmiData> reader = new Reader<ImmiData>(sourceFile))
            using (Writer<Transformed> writer = new Writer<Transformed>(destinationPath + "transformed.csv"))
            {
                transformer.Start();
                ImmiData read;
                while (processed != maxRecords && reader.TryReadNextRecord(out read))
                {
                    if (raw.IsValidData(read))
                    {
                        processed++;
                        VisaSource source = raw.GetData(read, read.CASE_NUMBER);

                        if (source != null)
                        {
                            Transformed trans = new Transformed();

                            transformer.TransformRow(source, trans);
                            writer.WriteTransformed(trans);
                        }
                    }
                }

                writer.Flush();
                raw.Save();
                transformer.End();
            }
        }

        public static void SeperateData<T>(double percent, string sourceFile, string destinationPath)
        {
            Random r = new Random();
            using (Reader<T> reader = new Reader<T>(sourceFile))
            using (Writer<T> validation = new Writer<T>(destinationPath + "validation.csv"))
            using (Writer<T> training = new Writer<T>(destinationPath + "training.csv"))
            {
                T read;
                while (reader.TryReadNextRecord(out read))
                {
                    if (r.Next(0, 100) < percent)
                    {
                        training.WriteTransformed(read);
                    }
                    else
                    {
                        validation.WriteTransformed(read);
                    }
                }

                validation.Flush();
                training.Flush();
            }
        }


        public static void MergeData(Dictionary<string,string> yearToFileMapping, string destinationFile)
        {
            using (Writer<NewTransformed> validation = new Writer<NewTransformed>(destinationFile))
            {
                foreach (string year in yearToFileMapping.Keys)
                {
                    using (Reader<Transformed> reader = new Reader<Transformed>(yearToFileMapping[year]))
                    {
                        Transformed read;
                        while (reader.TryReadNextRecord(out read))
                        {
                            NewTransformed n = new NewTransformed()
                            {
                                Year = year,
                                Calculated_WageLevel = read.Calculated_WageLevel,
                                Calculated_Yearly_Wage = read.Calculated_Yearly_Wage,
                                CASE_NUMBER = read.CASE_NUMBER,
                                Decision = read.Decision
                            };
                            validation.WriteTransformed(n);
                        }
                    }
                }
                validation.Flush();
            }
        }


        public static void MergeDataText(Dictionary<string, string> yearToFileMapping, string destinationFile)
        {
            using (Writer<NewCombined> validation = new Writer<NewCombined>(destinationFile))
            {
                foreach (string year in yearToFileMapping.Keys)
                {
                    using (Reader<Combined> reader = new Reader<Combined>(yearToFileMapping[year]))
                    {
                        Combined read;
                        while (reader.TryReadNextRecord(out read))
                        {
                            NewCombined n = new NewCombined()
                            {
                                Year = year,
                                Decision = read.Decision,
                                CaseNumber = read.CaseNumber,
                                Together = read.Together
                            };
                            validation.WriteTransformed(n);
                        }
                    }
                }
                validation.Flush();
            }
        }
    }

    public class NewTransformed : Transformed
    {
        public string Year { get; set; }
    }

    public class NewCombined : Combined
    {
        public string Year { get; set; }
    }
}
