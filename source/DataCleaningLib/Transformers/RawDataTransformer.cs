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
                || !this.FillData(this.NAICS_CODE, $"{nameof(this.NAICS_CODE)}", row, source, data)
                || !this.FillData(this.TOTAL_WORKERS, $"{nameof(this.TOTAL_WORKERS)}", row, source, data)
                || !this.FillData(this.PW_UNIT_OF_PAY, $"{nameof(this.PW_UNIT_OF_PAY)}", row, source, data)
                || !this.FillData(this.WAGE_UNIT_OF_PAY, $"{nameof(this.WAGE_UNIT_OF_PAY)}", row, source, data)
                || !this.FillData(this.WAGE_SOURCE, $"{nameof(this.WAGE_SOURCE)}", row, source, data)
                )
            {
                return null;
            }

            source.CASE_NUMBER = data.CASE_NUMBER;
            source.EMPLOYER_NAME = data.EMPLOYER_NAME;
            source.JOB_TITLE = data.JOB_TITLE;
            source.SOC_CODE = data.SOC_CODE;
            source.SOC_NAME = data.SOC_NAME;
            source.PREVAILING_WAGE = data.PREVAILING_WAGE;
            source.WAGE_RATE_OF_PAY_FROM = data.WAGE_RATE_OF_PAY_FROM;
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
