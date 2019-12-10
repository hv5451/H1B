using System;
using System.Collections.Generic;
using System.Text;

namespace DataCleaningLib.Transformers
{
    class CompositeTransformer : ITransformer
    {
        private ITransformer[] transformers;
        private ITransformer[] oberserver;

        public CompositeTransformer(string path)
        {
             this.oberserver = new ITransformer[] { new JobObserver(path), new Naics_observer(path), new SocCodeObserver(path), new SocNameObserver(path), new CompanyObserver(path), new WageSourceObserver(path), new CombinedObserver(path) };
             this.transformers = new ITransformer[] { new DecisionTransformer(), new WageTransformer(path) };
        }

        public void End()
        {
            foreach(ITransformer trans in transformers)
            {
                trans.End();
            }

            foreach (ITransformer trans in oberserver)
            {
                trans.End();
            }
        }

        public void Start()
        {
            foreach (ITransformer trans in transformers)
            {
                trans.Start();
            }

            foreach (ITransformer trans in oberserver)
            {
                trans.Start();
            }
        }

        public void TransformRow(VisaSource source, Transformed target)
        {
            foreach (ITransformer trans in transformers)
            {
                trans.TransformRow(source, target);
            }

            foreach (ITransformer trans in oberserver)
            {
                trans.TransformRow(source, target);
            }
        }
    }
}
