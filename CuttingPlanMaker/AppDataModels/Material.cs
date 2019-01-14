using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    class Material
    {
        [CsvColumn(Name = "Name", FieldIndex = 1)]
        public string Name { get; set; }

        [CsvColumn(Name = "Length", FieldIndex = 2)]
        public double Length { get; set; }

        [CsvColumn(Name = "Width", FieldIndex = 3)]
        public double Width { get; set; }

        [CsvColumn(Name = "Thickness", FieldIndex = 4)]
        public double Thickness { get; set; }

        [CsvColumn(Name = "Cost", FieldIndex = 5)]
        public double Cost { get; set; }
    }
}
