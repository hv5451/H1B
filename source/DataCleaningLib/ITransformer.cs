using System;
using System.Collections.Generic;
using System.Text;

namespace DataCleaningLib
{
    public interface ITransformer
    {
        void Start();

        void TransformRow(VisaSource source, Transformed target);

        void End();
    }
}
