using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCleaningLib
{
    public class Reader<T> : IDisposable
    {
        private StreamReader disposableReader;
        private CsvReader disposableCsv;

        public Reader(string path)
        {
            disposableReader = new StreamReader(path);
            disposableCsv = new CsvReader(disposableReader);
        }

        public bool TryReadNextRecord(out T record)
        {
            record = default(T);
            bool read = disposableCsv.Read();

            if (read)
            {
                record = disposableCsv.GetRecord<T>();
            }

            return read;
        }

        public void Dispose()
        {
            this.disposableCsv?.Dispose();
            this.disposableReader?.Dispose();
        }
    }
}
