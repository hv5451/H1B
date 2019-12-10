using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCleaningLib.Transformers
{
    public class Naics_observer : TextObserver, ITransformer
    {
        public Naics_observer(string path)
            : base(path + $"{nameof(Naics_observer)}.json")
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
            this.UpdateMap(source.NAICS_CODE.ToString(), source.CASE_STATUS == CaseStatus.CERTIFIED);
        }
    }
}
