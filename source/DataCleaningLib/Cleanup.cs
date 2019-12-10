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

        public static void SeperateData(double percent, string sourceFile, string destinationPath)
        {
            Random r = new Random();
            using (Reader<Transformed> reader = new Reader<Transformed>(sourceFile))
            using (Writer<Transformed> validation = new Writer<Transformed>(destinationPath + "validation.csv"))
            using (Writer<Transformed> training = new Writer<Transformed>(destinationPath + "training.csv"))
            {
                Transformed read;
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

    }
}
