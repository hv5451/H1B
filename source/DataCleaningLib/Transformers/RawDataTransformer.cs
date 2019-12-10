using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataCleaningLib
{
    public class RawDataTransformer
    {
        private List<InvalidData> invalids = new List<InvalidData>();
        private string path;

        public RawDataTransformer(string errorPath)
        {
            this.path = errorPath;
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(this.invalids);
            File.WriteAllText(this.path + "errors.json", json);
        }

        public bool IsValidData(ImmiData data)
        {
            VisaSource source = new VisaSource() ;
            return this.VISA_CLASS(source,data) && this.CaseStatus(source, data);
        }

        public VisaSource GetData(ImmiData data, string row)
        {
            VisaSource source = new VisaSource();

            if (
                !this.FillData(this.VISA_CLASS, $"{nameof(this.VISA_CLASS)}", row, source, data)
                || !this.FillData(this.CaseStatus, $"{nameof(this.CaseStatus)}", row, source, data)
                || !this.FillData(this.EMPLOYMENT_START_DATE, $"{nameof(this.EMPLOYMENT_START_DATE)}", row, source, data)
                || !this.FillData(this.EMPLOYMENT_END_DATE, $"{nameof(this.EMPLOYMENT_END_DATE)}", row, source, data)
                || !this.FillData(this.SECONDARY_ENTITY, $"{nameof(this.SECONDARY_ENTITY)}", row, source, data)
                || !this.FillData(this.AGENT_REPRESENTING_EMPLOYER, $"{nameof(this.AGENT_REPRESENTING_EMPLOYER)}", row, source, data)
                || !this.FillData(this.NAICS_CODE, $"{nameof(this.NAICS_CODE)}", row, source, data)
                || !this.FillData(this.TOTAL_WORKERS, $"{nameof(this.TOTAL_WORKERS)}", row, source, data)
                || !this.FillData(this.NEW_EMPLOYMENT, $"{nameof(this.NEW_EMPLOYMENT)}", row, source, data)
                || !this.FillData(this.CONTINUED_EMPLOYMENT, $"{nameof(this.CONTINUED_EMPLOYMENT)}", row, source, data)
                || !this.FillData(this.CHANGE_PREVIOUS_EMPLOYMENT, $"{nameof(this.CHANGE_PREVIOUS_EMPLOYMENT)}", row, source, data)
                || !this.FillData(this.NEW_CONCURRENT_EMPLOYMENT, $"{nameof(this.NEW_CONCURRENT_EMPLOYMENT)}", row, source, data)
                || !this.FillData(this.CHANGE_EMPLOYER, $"{nameof(this.CHANGE_EMPLOYER)}", row, source, data)
                || !this.FillData(this.AMENDED_PETITION, $"{nameof(this.AMENDED_PETITION)}", row, source, data)
                || !this.FillData(this.FULL_TIME_POSITION, $"{nameof(this.FULL_TIME_POSITION)}", row, source, data)
                || !this.FillData(this.PW_UNIT_OF_PAY, $"{nameof(this.PW_UNIT_OF_PAY)}", row, source, data)
                || !this.FillData(this.PW_WAGE_LEVEL, $"{nameof(this.PW_WAGE_LEVEL)}", row, source, data)
                || !this.FillData(this.WAGE_UNIT_OF_PAY, $"{nameof(this.WAGE_UNIT_OF_PAY)}", row, source, data)
                || !this.FillData(this.H1B_DEPENDENT, $"{nameof(this.H1B_DEPENDENT)}", row, source, data)
                || !this.FillData(this.WILLFUL_VIOLATOR, $"{nameof(this.WILLFUL_VIOLATOR)}", row, source, data)
                || !this.FillData(this.WAGE_SOURCE, $"{nameof(this.WAGE_SOURCE)}", row, source, data)
                )
            {
                return null;
            }

            source.CASE_NUMBER = data.CASE_NUMBER;
            source.EMPLOYER_NAME = data.EMPLOYER_NAME;
            source.SECONDARY_ENTITY_BUSINESS_NAME = data.SECONDARY_ENTITY_BUSINESS_NAME;
            source.AGENT_ATTORNEY_NAME = data.AGENT_ATTORNEY_NAME;
            source.JOB_TITLE = data.JOB_TITLE;
            source.SOC_CODE = data.SOC_CODE;
            source.SOC_NAME = data.SOC_NAME;
            source.PREVAILING_WAGE = data.PREVAILING_WAGE;
            source.WAGE_RATE_OF_PAY_FROM = data.WAGE_RATE_OF_PAY_FROM;
            source.WAGE_RATE_OF_PAY_TO = data.WAGE_RATE_OF_PAY_TO;
            source.WORKSITE_STATE = data.WORKSITE_STATE;
            return source;
        }

        private bool FillData(Func<VisaSource, ImmiData, bool> trans, string name, string row, VisaSource source, ImmiData data)
        {
            if (!trans(source, data))
            {
                this.invalids.Add(new InvalidData()
                {
                    RawData = data,
                    Reason = name,
                    Transformed = source,
                    Row = row
                });

                return false;
            }
            else
            {
                return true;
            }
        }

        private bool VISA_CLASS(VisaSource source, ImmiData data)
        {
            return data.VISA_CLASS == "H-1B";
        }

        private bool WAGE_SOURCE(VisaSource source, ImmiData data)
        {
            source.PW_SOURCE_OTHER = data.PW_SOURCE_OTHER;
            source.PW_SOURCE = data.PW_SOURCE;
            return true;
        }

        private bool EMPLOYMENT_START_DATE(VisaSource source, ImmiData data)
        {
            DateTime status;
            if (!DateTime.TryParse(data.EMPLOYMENT_START_DATE, out status))
            {
                source.EMPLOYMENT_START_DATE = DateTime.MinValue;
                return true;
            }

            source.EMPLOYMENT_START_DATE = status;
            return true;
        }

        private bool EMPLOYMENT_END_DATE(VisaSource source, ImmiData data)
        {
            DateTime status;
            if (!DateTime.TryParse(data.EMPLOYMENT_END_DATE, out status))
            {
                source.EMPLOYMENT_END_DATE = DateTime.MinValue;
                return true;
            }

            source.EMPLOYMENT_END_DATE = status;
            return true;
        }

        private bool WILLFUL_VIOLATOR(VisaSource source, ImmiData data)
        {
            Decision status;
            if (!Enum.TryParse<Decision>(data.WILLFUL_VIOLATOR, true, out status))
            {
                source.WILLFUL_VIOLATOR = Decision.N;
                return true;
            }

            source.WILLFUL_VIOLATOR = status;
            return true;
        }

        private bool H1B_DEPENDENT(VisaSource source, ImmiData data)
        {
            Decision status;
            if (!Enum.TryParse<Decision>(data.H1B_DEPENDENT, true, out status))
            {
                source.H1B_DEPENDENT = Decision.N;
                return false;
            }

            source.H1B_DEPENDENT = status;
            return true;
        }

        private bool FULL_TIME_POSITION(VisaSource source, ImmiData data)
        {
            Decision status;
            if (!Enum.TryParse<Decision>(data.FULL_TIME_POSITION, true, out status))
            {
                return false;
            }

            source.FULL_TIME_POSITION = status;
            return true;
        }

        private bool PW_UNIT_OF_PAY(VisaSource source, ImmiData data)
        {
            PayDuration status;
            if (!Enum.TryParse<PayDuration>(data.PW_UNIT_OF_PAY, true, out status))
            {
            }

            source.PW_UNIT_OF_PAY = status;
            return true;
        }

        private bool WAGE_UNIT_OF_PAY(VisaSource source, ImmiData data)
        {
            PayDuration status;
            if (!Enum.TryParse<PayDuration>(data.WAGE_UNIT_OF_PAY, true, out status))
            {
            }

            source.WAGE_UNIT_OF_PAY = status;
            return true;
        }

        private bool PW_WAGE_LEVEL(VisaSource source, ImmiData data)
        {
            WageLevel status;
            if (!Enum.TryParse<WageLevel>(data.PW_WAGE_LEVEL, true, out status))
            {
            }

            source.PW_WAGE_LEVEL = status;
            return true;
        }

        private bool NAICS_CODE(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.NAICS_CODE, out status))
            {
                return false;
            }

            source.NAICS_CODE = status;
            return true;
        }

        private bool TOTAL_WORKERS(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.TOTAL_WORKERS, out status))
            {
                source.TOTAL_WORKERS = 0;
                return true;
            }

            source.TOTAL_WORKERS = status;
            return true;
        }

        private bool NEW_EMPLOYMENT(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.NEW_EMPLOYMENT, out status))
            {
                source.NEW_EMPLOYMENT = 0;
                return true;
            }

            source.NEW_EMPLOYMENT = status;
            return true;
        }

        private bool CONTINUED_EMPLOYMENT(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.CONTINUED_EMPLOYMENT, out status))
            {
                source.CONTINUED_EMPLOYMENT = 0;
                return true;
            }

            source.CONTINUED_EMPLOYMENT = status;
            return true;
        }

        private bool AMENDED_PETITION(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.AMENDED_PETITION, out status))
            {
                source.AMENDED_PETITION = 0;
                return true;
            }

            source.AMENDED_PETITION = status;
            return true;
        }

        private bool CHANGE_PREVIOUS_EMPLOYMENT(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.CHANGE_PREVIOUS_EMPLOYMENT, out status))
            {
                source.CHANGE_PREVIOUS_EMPLOYMENT = 0;
                return true;
            }

            source.CHANGE_PREVIOUS_EMPLOYMENT = status;
            return true;
        }

        private bool NEW_CONCURRENT_EMPLOYMENT(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.NEW_CONCURRENT_EMPLOYMENT, out status))
            {
                source.NEW_CONCURRENT_EMPLOYMENT = 0;
                return true;
            }

            source.NEW_CONCURRENT_EMPLOYMENT = status;
            return true;
        }

        private bool CHANGE_EMPLOYER(VisaSource source, ImmiData data)
        {
            int status;
            if (!int.TryParse(data.CHANGE_EMPLOYER, out status))
            {
                source.CHANGE_EMPLOYER = 0;
                return true;
            }

            source.CHANGE_EMPLOYER = status;
            return true;
        }

        private bool AGENT_REPRESENTING_EMPLOYER(VisaSource source, ImmiData data)
        {
            Decision status;
            if (!Enum.TryParse<Decision>(data.AGENT_REPRESENTING_EMPLOYER, true, out status))
            {
                source.AGENT_REPRESENTING_EMPLOYER = Decision.N;
                return true;
            }

            source.AGENT_REPRESENTING_EMPLOYER = status;
            return true;
        }

        private bool SECONDARY_ENTITY(VisaSource source, ImmiData data)
        {
            Decision status;
            if (!Enum.TryParse<Decision>(data.SECONDARY_ENTITY, true, out status))
            {
                source.SECONDARY_ENTITY = Decision.N;
                return true;
            }

            source.SECONDARY_ENTITY = status;
            return true;
        }

        private bool CaseStatus(VisaSource source, ImmiData data)
        {
            CaseStatus status;
            if (!Enum.TryParse<CaseStatus>(data.CASE_STATUS, true, out status))
            {
                return false;
            }

            source.CASE_STATUS = status;
            return true;
        }
    }

    public class InvalidData
    {
        public string Reason { get; set; }

        public ImmiData RawData { get; set; }

        public VisaSource Transformed { get; set; }

        public string Row { get; set; }
    }
}
