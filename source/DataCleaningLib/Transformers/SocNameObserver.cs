using System;
using System.Collections.Generic;
using System.Text;

namespace DataCleaningLib.Transformers
{
    class SocNameObserver : TextObserver, ITransformer
    {
        public SocNameObserver(string path)
            : base(path + $"{nameof(SocNameObserver)}.json")
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
            this.UpdateMap(source.SOC_NAME, source.CASE_STATUS == CaseStatus.CERTIFIED);
        }
    }
}
