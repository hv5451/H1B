using System;
using System.ComponentModel;

namespace DataCleaningLib
{

    // https://onedrive.live.com/?cid=D301CFC61890BBFF&id=D301CFC61890BBFF%2124461&parId=D301CFC61890BBFF%2124448&o=OneUp
    public class ImmiData
    {
        public string CASE_NUMBER { get; set; }

        // Case Status enum
        public string CASE_STATUS { get; set; }

        public string VISA_CLASS { get; set; }

        // datetime
        public string EMPLOYMENT_START_DATE { get; set; }

        // datetime
        public string EMPLOYMENT_END_DATE { get; set; }

        public string EMPLOYER_NAME { get; set; }

        public string JOB_TITLE { get; set; }

        public string SOC_CODE { get; set; }

        public string SOC_NAME { get; set; }

        // int enum
        public string NAICS_CODE { get; set; }

        // int enum
        public string TOTAL_WORKERS { get; set; }

        public string PREVAILING_WAGE { get; set; }

        // PayDuration enum
        public string PW_UNIT_OF_PAY { get; set; }
        
        public string PW_SOURCE { get; set; }

        public string PW_SOURCE_OTHER { get; set; }

        public string WAGE_RATE_OF_PAY_FROM { get; set; }

        // PayDuration enum
        public string WAGE_UNIT_OF_PAY { get; set; }
    }
}
