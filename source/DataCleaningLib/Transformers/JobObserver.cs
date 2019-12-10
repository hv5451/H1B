using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCleaningLib.Transformers
{
    class JobObserver : TextObserver, ITransformer
    {
        public JobObserver(string path)
            : base(path + $"{nameof(JobObserver)}.json")
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
            this.UpdateMap(source.JOB_TITLE, source.CASE_STATUS == CaseStatus.CERTIFIED);
        }
    }
}
