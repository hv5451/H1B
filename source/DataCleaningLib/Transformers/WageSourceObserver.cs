using System;
using System.Collections.Generic;
using System.Text;

namespace DataCleaningLib.Transformers
{
    class WageSourceObserver : TextObserver,ITransformer
    {
        public WageSourceObserver(string path)
            : base(path + $"{nameof(WageSourceObserver)}.json")
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
            this.UpdateMap(source.PW_SOURCE_OTHER, source.CASE_STATUS == CaseStatus.CERTIFIED);
            this.UpdateMap(source.PW_SOURCE, source.CASE_STATUS == CaseStatus.CERTIFIED);
        }
    }
}
