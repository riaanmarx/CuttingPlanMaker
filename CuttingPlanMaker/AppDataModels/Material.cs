using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// Class to keep track of the different types of material we are using in the project
    /// </summary>
    class Material
    {
        /// <summary>
        /// Name for the material
        /// </summary>
        [CsvColumn(Name = "Name", FieldIndex = 1)]
        public string Name { get; set; }

        /// <summary>
        /// Length of standard boards of this material - not used yet
        /// </summary>
        [CsvColumn(Name = "Length", FieldIndex = 2)]
        public double Length { get; set; }

        /// <summary>
        /// Width of standard boards of this material - not used yet
        /// </summary>
        [CsvColumn(Name = "Width", FieldIndex = 3)]
        public double Width { get; set; }

        /// <summary>
        /// Thickness of boards of this material
        /// </summary>
        [CsvColumn(Name = "Thickness", FieldIndex = 4)]
        public double Thickness { get; set; }

        /// <summary>
        /// Volume cost of the material
        /// </summary>
        [CsvColumn(Name = "Cost", FieldIndex = 5)]
        public double Cost { get; set; }
    }
}
