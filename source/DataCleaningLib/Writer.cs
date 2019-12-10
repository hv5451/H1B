using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCleaningLib
{
    public class Writer<T> : IDisposable
    {
        private const int saveSnapshotAfter = 10000;
        
        private StreamWriter disposableWriter;
        private CsvWriter disposableCsv;

        public Writer(string path)
        {
            disposableWriter = new StreamWriter(path);
            disposableCsv = new CsvWriter(disposableWriter);
            disposableCsv.WriteHeader<T>();
        }

        public void WriteTransformed(T row)
        {
            // write to file
            disposableCsv.NextRecord();
            disposableCsv.WriteRecord<T>(row);

            // save snapshot
        }

        public void Flush()
        {
            disposableCsv.Flush();
        }

        public void Dispose()
        {
            this.disposableCsv?.Dispose();
            this.disposableWriter?.Dispose();
        }
    }
}
