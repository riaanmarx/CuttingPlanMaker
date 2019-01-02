using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    class StockItem
    {
        [CsvColumn(Name = "Name", FieldIndex = 1)]
        public string Name { get; set; }

        [CsvColumn(Name = "Material", FieldIndex = 2)]
        public string Material { get; set; }

        [CsvColumn(Name = "Length", FieldIndex = 3)]
        public string Length { get; set; }

        [CsvColumn(Name = "Width", FieldIndex = 4)]
        public string Width { get; set; }
    }
}
