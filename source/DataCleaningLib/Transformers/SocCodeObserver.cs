using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCleaningLib.Transformers
{
    class SocCodeObserver : TextObserver, ITransformer
    {
        public SocCodeObserver(string path)
            : base(path + $"{nameof(SocCodeObserver)}.json")
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
            this.UpdateMap(source.SOC_CODE, source.CASE_STATUS == CaseStatus.CERTIFIED);
        }
    }
}
