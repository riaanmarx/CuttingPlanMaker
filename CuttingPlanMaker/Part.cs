using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    public class Part
    {
        [CsvColumn(Name = "Name", FieldIndex = 1)]
        public string Name { get; set; }

        [CsvColumn(Name = "Material", FieldIndex = 2)]
        public string Material { get; set; }

        [CsvColumn(Name = "Length", FieldIndex = 3)]
        public double Length { get; set; }

        [CsvColumn(Name = "Width", FieldIndex = 4)]
        public double Width { get; set; }

        public double Area => Length * Width;

        public bool IsPacked { get; set; }

        public override string ToString()
        {
            return $"{Name} [{Length,7:0.0} x {Width,5:0.0}]";
        }

        public void Inflate(double deltaWidth, double deltaLength)
        {
            Width += 2 * deltaWidth;
            Length += 2 * deltaLength;
        }
    }
}
