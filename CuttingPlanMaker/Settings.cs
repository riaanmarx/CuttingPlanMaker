using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// Class to host configuration settings for the project
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The kerf of the blade used to saw the parts
        /// </summary>
        [CsvColumn(Name = "BladeKerf", FieldIndex = 1)]
        public double BladeKerf { get; set; }

        /// <summary>
        /// Padding to be added to the parts when planning to allow for planing and sanding
        /// </summary>
        [CsvColumn(Name = "PartPaddingLength", FieldIndex = 2)]
        public double PartPaddingLength { get; set; }

        /// <summary>
        /// Padding to be added to the parts when planning to allow for planing and sanding
        /// </summary>
        [CsvColumn(Name = "PartPaddingWidth", FieldIndex = 3)]
        public double PartPaddingWidth { get; set; }

        /// <summary>
        /// Setting to control if parts are repacked automatically on each data change
        /// </summary>
        [CsvColumn(Name = "AutoRecalc", FieldIndex = 4)]
        public bool AutoRepack { get; set; }

        /// <summary>
        /// not used - If the boards are drawn vertically or horizontally
        /// </summary>
        [CsvColumn(Name = "ResultOrientation", FieldIndex = 5)]
        public string ResultOrientation { get; set; }

        /// <summary>
        /// Draw the stock not containing any parts
        /// </summary>
        [CsvColumn(Name = "DrawUnusedStock", FieldIndex = 6)]
        public bool DrawUnusedStock { get; set; }

        /// <summary>
        /// Name for the project to be printed on reports
        /// </summary>
        [CsvColumn(Name = "ProjectName", FieldIndex = 7)]
        public string ProjectName { get; set; }

        /// <summary>
        /// Job ID for the project to be printed on reports
        /// </summary>
        [CsvColumn(Name = "JobID", FieldIndex = 8)]
        public string JobID { get; set; }

        /// <summary>
        /// The client's name - printed on the reports
        /// </summary>
        [CsvColumn(Name = "ClientName", FieldIndex = 9)]
        public string ClientName { get; set; }

        /// <summary>
        /// The client's tellephone number
        /// </summary>
        [CsvColumn(Name = "ClientTel", FieldIndex =10)]
        public string ClientTelNr { get; set; }

        /// <summary>
        /// The client's address
        /// </summary>
        [CsvColumn(Name = "ClientAddr", FieldIndex = 11)]
        public string ClientAddr { get; set; }

        /// <summary>
        /// The date on which the job should be done
        /// </summary>
        [CsvColumn(Name = "TargetDate", FieldIndex = 12)]
        public string TargetDate { get; set; }

        /// <summary>
        /// Indicator to add the padding to the dimensions on the reports
        /// </summary>
        [CsvColumn(Name = "IncludePaddingOnReports", FieldIndex = 13)]
        public bool IncludePaddingInReports { get; set; }

        /// <summary>
        /// Indicator to add the padding to the displayed layout
        /// </summary>
        [CsvColumn(Name = "IncludePaddingInDisplay", FieldIndex = 14)]
        public bool IncludePaddingInDisplay { get; set; }

        /// <summary>
        /// Selection of the algorithm to use when packing the parts
        /// </summary>
        [CsvColumn(Name = "Algorithm", FieldIndex = 15)]
        public string Algorithm { get; set; }
    }
}
