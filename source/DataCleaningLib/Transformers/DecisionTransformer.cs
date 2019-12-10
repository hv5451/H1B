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
            target.SECONDARY_ENTITY = this.GetDecision(source.SECONDARY_ENTITY);
            target.AGENT_REPRESENTING_EMPLOYER = this.GetDecision(source.AGENT_REPRESENTING_EMPLOYER);
            target.TOTAL_WORKERS = source.TOTAL_WORKERS;
            target.NEW_EMPLOYMENT = source.NEW_EMPLOYMENT;
            target.CONTINUED_EMPLOYMENT = source.CONTINUED_EMPLOYMENT;
            target.CHANGE_PREVIOUS_EMPLOYMENT = source.CHANGE_PREVIOUS_EMPLOYMENT;
            target.NEW_CONCURRENT_EMPLOYMENT = source.NEW_CONCURRENT_EMPLOYMENT;
            target.CHANGE_EMPLOYER = source.CHANGE_EMPLOYER;
            target.AMENDED_PETITION = source.AMENDED_PETITION;
            target.FULL_TIME_POSITION = this.GetDecision(source.FULL_TIME_POSITION);
            target.H1B_DEPENDENT = this.GetDecision(source.H1B_DEPENDENT);
            target.WILLFUL_VIOLATOR = this.GetDecision(source.WILLFUL_VIOLATOR);
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
