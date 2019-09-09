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
        public double BladeKerf { get; set; }

        /// <summary>
        /// Padding to be added to the parts when planning to allow for planing and sanding
        /// </summary>
        public double PartPaddingLength { get; set; }

        /// <summary>
        /// Padding to be added to the parts when planning to allow for planing and sanding
        /// </summary>
        public double PartPaddingWidth { get; set; }

        /// <summary>
        /// Setting to control if parts are repacked automatically on each data change
        /// </summary>
        public bool AutoRepack { get; set; }

        /// <summary>
        /// not used - If the boards are drawn vertically or horizontally
        /// </summary>
        public string ResultOrientation { get; set; }

        /// <summary>
        /// Draw the stock not containing any parts
        /// </summary>
        public bool DrawUnusedStock { get; set; }

        /// <summary>
        /// Name for the project to be printed on reports
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Job ID for the project to be printed on reports
        /// </summary>
        public string JobID { get; set; }

        /// <summary>
        /// The client's name - printed on the reports
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// The client's tellephone number
        /// </summary>
        public string ClientTelNr { get; set; }

        /// <summary>
        /// The client's address
        /// </summary>
        public string ClientAddr { get; set; }

        /// <summary>
        /// The date on which the job should be done
        /// </summary>
        public string TargetDate { get; set; }

        /// <summary>
        /// Indicator to add the padding to the dimensions on the reports
        /// </summary>
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
