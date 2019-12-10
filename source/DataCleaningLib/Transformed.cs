using System;
using System.Collections.Generic;
using System.Text;

namespace DataCleaningLib
{
    public class Transformed
    {
        public string CASE_NUMBER { get; set; }

        public int Decision { get; set; }

        public int SECONDARY_ENTITY { get; set; }

        public int AGENT_REPRESENTING_EMPLOYER { get; set; }

        public int TOTAL_WORKERS { get; set; }

        public int NEW_EMPLOYMENT { get; set; }

        public int CONTINUED_EMPLOYMENT { get; set; }

        public int CHANGE_PREVIOUS_EMPLOYMENT { get; set; }

        public int NEW_CONCURRENT_EMPLOYMENT { get; set; }

        public int CHANGE_EMPLOYER { get; set; }

        public int AMENDED_PETITION { get; set; }

        public int FULL_TIME_POSITION { get; set; }

        public int H1B_DEPENDENT { get; set; }

        public int WILLFUL_VIOLATOR { get; set; }

        public long Calculated_Yearly_Wage { get; set; }

        public long Calculated_WageLevel { get; set; }
    }
}