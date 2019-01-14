using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    public class Settings
    {
        [CsvColumn(Name = "BladeKerf", FieldIndex = 1)]
        public double BladeKerf { get; set; }

        [CsvColumn(Name = "PartPaddingLength", FieldIndex = 2)]
        public double PartPaddingLength { get; set; }

        [CsvColumn(Name = "PartPaddingWidth", FieldIndex = 3)]
        public double PartPaddingWidth { get; set; }

        [CsvColumn(Name = "AutoRecalc", FieldIndex = 4)]
        public bool AutoRepack { get; set; }

        [CsvColumn(Name = "ResultOrientation", FieldIndex = 5)]
        public string ResultOrientation { get; set; }

        [CsvColumn(Name = "DrawUnusedStock", FieldIndex = 6)]
        public bool DrawUnusedStock { get; set; }

        [CsvColumn(Name = "ProjectName", FieldIndex = 7)]
        public string ProjectName { get; set; }

        [CsvColumn(Name = "JobID", FieldIndex = 8)]
        public string JobID { get; set; }

        [CsvColumn(Name = "ClientName", FieldIndex = 9)]
        public string ClientName { get; set; }

        [CsvColumn(Name = "ClientTel", FieldIndex =10)]
        public string ClientTelNr { get; set; }

        [CsvColumn(Name = "ClientAddr", FieldIndex = 11)]
        public string ClientAddr { get; set; }

        [CsvColumn(Name = "TargetDate", FieldIndex = 12)]
        public string TargetDate { get; set; }

        [CsvColumn(Name = "IncludePaddingOnReports", FieldIndex = 13)]
        public bool IncludePaddingInReports { get; set; }

        [CsvColumn(Name = "IncludePaddingInDisplay", FieldIndex = 14)]
        public bool IncludePaddingInDisplay { get; set; }

        [CsvColumn(Name = "Algorithm", FieldIndex = 15)]
        public string Algorithm { get; set; }
    }
}
