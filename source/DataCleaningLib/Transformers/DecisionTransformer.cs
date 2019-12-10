using System;
using System.Collections.Generic;
using System.Text;

namespace DataCleaningLib.Transformers
{
    class DecisionTransformer : ITransformer
    {
        public void End()
        {
        }

        public void Start()
        {
        }

        public void TransformRow(VisaSource source, Transformed target)
        {
            target.CASE_NUMBER = source.CASE_NUMBER;
            this.GetOutcome(source, target);
        }

        private int GetDecision(Decision dec)
        {
            if (dec == Decision.Y)
            {
                return 1;
            }
            else if (dec == Decision.N)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        private void GetOutcome(VisaSource source, Transformed target)
        {
            if (source.CASE_STATUS == CaseStatus.CERTIFIED)
            {
                target.Decision = 1;
            }
            else if (source.CASE_STATUS == CaseStatus.DENIED)
            {
                target.Decision = 0;
            }
            else
            {
                target.Decision = -1;
            }
        }
    }
}
