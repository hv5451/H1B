using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCleaningLib.Transformers
{
    class CompanyObserver : TextObserver, ITransformer
    {
        public CompanyObserver(string path)
            : base(path + $"{nameof(CompanyObserver)}.json")
        {
        }

        public void End()
        {
            SaveFile();
        }

        public void Start()
        {
        }

        public void TransformRow(VisaSource source, Transformed target)
        {
            this.UpdateMap(source.EMPLOYER_NAME, source.CASE_STATUS == CaseStatus.CERTIFIED);
        }
    }
}
