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

        // datetime
        public string CASE_SUBMITTED { get; set; }

        // datetime
        public string DECISION_DATE { get; set; }

        // datetime
        public string ORIGINAL_CERT_DATE { get; set; }

        public string VISA_CLASS { get; set; }

        // datetime
        public string EMPLOYMENT_START_DATE { get; set; }

        // datetime
        public string EMPLOYMENT_END_DATE { get; set; }

        public string EMPLOYER_NAME { get; set; }

        // Decision enum
        public string SECONDARY_ENTITY { get; set; }

        public string SECONDARY_ENTITY_BUSINESS_NAME { get; set; }

        // Decision enum
        public string AGENT_REPRESENTING_EMPLOYER { get; set; }

        public string AGENT_ATTORNEY_NAME { get; set; }

        public string JOB_TITLE { get; set; }

        public string SOC_CODE { get; set; }

        public string SOC_NAME { get; set; }

        // int enum
        public string NAICS_CODE { get; set; }

        // int enum
        public string TOTAL_WORKERS { get; set; }

        // int enum
        public string NEW_EMPLOYMENT { get; set; }

        // int enum
        public string CONTINUED_EMPLOYMENT { get; set; }

        // int enum
        public string CHANGE_PREVIOUS_EMPLOYMENT { get; set; }

        // int enum
        public string NEW_CONCURRENT_EMPLOYMENT { get; set; }

        // int enum
        public string CHANGE_EMPLOYER { get; set; }

        // int enum
        public string AMENDED_PETITION { get; set; }

        // Decision enum
        public string FULL_TIME_POSITION { get; set; }

        public string PREVAILING_WAGE { get; set; }

        // PayDuration enum
        public string PW_UNIT_OF_PAY { get; set; }

        // WageLevel enum
        public string PW_WAGE_LEVEL { get; set; }
        
        public string PW_SOURCE { get; set; }

        public string PW_SOURCE_OTHER { get; set; }

        public string WAGE_RATE_OF_PAY_FROM { get; set; }

        public string WAGE_RATE_OF_PAY_TO { get; set; }

        // PayDuration enum
        public string WAGE_UNIT_OF_PAY { get; set; }

        // Decision enum
        public string H1B_DEPENDENT { get; set; }

        public string WORKSITE_STATE { get; set; }

        // Decision enum
        public string WILLFUL_VIOLATOR { get; set; }
    }
}
