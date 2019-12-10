using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataCleaningLib
{
    public enum CaseStatus
    {
        DENIED = 0,
        CERTIFIED = 1
    }

    public enum Decision
    {
        Unknown = 0,
        Y = 1,
        N = 2
    }

    public enum PayDuration
    {
        Unknown = 0,
        Daily = 1,
        DAI = 2,
        Hourly = 3,
        HR = 4,
        [Description("Bi-weekly")]
        Biweekly = 5,
        BI = 6,
        Weekly = 7,
        WK = 8,
        Monthly = 9,
        MTH = 10,
        Yearly = 11,
        YR = 12,
        Hour = 13,
        Year = 14,
        Month = 15,
        Week = 16,
        [Description("Bi-week")]
        BiWeek = 17
    }

    public enum WageLevel
    {
        Unkown = 0,
        I = 1,
        II = 2,
        III = 3,
        IV = 4
    }

    public enum Basis
    {
        Unkown = 0,
        Wage = 1,
        Degree = 2,
        Both = 3
    }

    public enum PaySource
    {
        Unkwnon = 0,
        Other = 1,
        OES = 2,
        CBA = 3,
        DBA = 4,
        SCA = 5,
        SURVEY = 6
    }

    public class VisaSource
    {
        public string CASE_NUMBER { get; set; }

        public CaseStatus CASE_STATUS { get; set; }

        public string VISA_CLASS { get; set; }

        public DateTime EMPLOYMENT_START_DATE { get; set; }

        public DateTime EMPLOYMENT_END_DATE { get; set; }

        public string EMPLOYER_NAME { get; set; }

        public Decision SECONDARY_ENTITY { get; set; }

        public string SECONDARY_ENTITY_BUSINESS_NAME { get; set; }

        public string JOB_TITLE { get; set; }

        public string SOC_CODE { get; set; }

        public string SOC_NAME { get; set; }

        public int NAICS_CODE { get; set; }

        public int TOTAL_WORKERS { get; set; }

        public string PREVAILING_WAGE { get; set; }

        public PayDuration PW_UNIT_OF_PAY { get; set; }

        public string WAGE_RATE_OF_PAY_FROM { get; set; }

        public PayDuration WAGE_UNIT_OF_PAY { get; set; }

        public string PW_SOURCE { get; set; }

        public string PW_SOURCE_OTHER { get; set; }

        public string Error_Info { get; set; }
    }
}
