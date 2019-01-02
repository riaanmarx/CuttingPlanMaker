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
        public string Length { get; set; }

        [CsvColumn(Name = "Width", FieldIndex = 3)]
        public string Width { get; set; }

        [CsvColumn(Name = "Thickness", FieldIndex = 4)]
        public string Thickness { get; set; }
        [CsvColumn(Name = "Cost", FieldIndex = 5)]
        public string Cost { get; set; }
    }
}
