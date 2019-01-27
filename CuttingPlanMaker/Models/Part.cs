using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// Model class for Part objects
    /// </summary>
    public class Part
    {
        /// <summary>
        /// A name for the part
        /// </summary>
        [CsvColumn(Name = "Name", FieldIndex = 1)]
        public string Name { get; set; }

        /// <summary>
        /// The material from which this part is to be made
        /// </summary>
        [CsvColumn(Name = "Material", FieldIndex = 2)]
        public string Material { get; set; }

        /// <summary>
        /// Length of the part (dimension parallel with grain)
        /// </summary>
        [CsvColumn(Name = "Length", FieldIndex = 3)]
        public double Length { get; set; }

        /// <summary>
        /// Width of the part (dimension accross the grain)
        /// </summary>
        [CsvColumn(Name = "Width", FieldIndex = 4)]
        public double Width { get; set; }

        /// <summary>
        /// The surface area of the part (length * width)
        /// </summary>
        public double Area => Length * Width;

        /// <summary>
        /// Flag to indicate if this part has been placed on a board already
        /// </summary>
        public bool IsPacked { get; set; }
        public bool IsFrozen { get; set; }
        /// <summary>
        /// string repressentation of the part
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} [{Length,7:0.0} x {Width,5:0.0}]";
        }

        /// <summary>
        /// inflates the part's size with the specified amounts
        /// </summary>
        /// <param name="deltaWidth"></param>
        /// <param name="deltaLength"></param>
        public void Inflate(double deltaWidth, double deltaLength)
        {
            Width += 2 * deltaWidth;
            Length += 2 * deltaLength;
        }
    }
}
