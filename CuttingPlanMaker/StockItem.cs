using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    public class StockItem
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

        public double dWidth { get; set; }

        public double dLength { get; set; }

        public StockItem AssociatedBoard { get; set; }

        public Part[] PackedParts;
        public double[] PackedPartdLengths { get; set; }
        public double[] PackedPartdWidths { get; set; }
        public double PackedPartsTotalArea { get; set; }
        public int PackedPartsCount { get; set; }
        public bool isComplete { get; set; }
        public bool isInUse { get; set; }

        public double oldLength;// { get; set; }
        public double oldWidth;// { get; set; }

        public StockItem()
        { }

        public StockItem(string name, double length, double width, double dlength = 0, double dwidth = 0)
        {
            Name = name;
            dWidth = dwidth;
            dLength = dlength;
            Length = length;
            Width = width;
        }

        public override string ToString()
        {
            if (dLength != 0 || dWidth != 0)
                return $"{Name} [{Length,7:0.0} x {Width,5:0.0}] @ ({dLength,7:0.0}, {dWidth,5:0.0})";

            return $"{Name} [{Length,7:0.0} x {Width,5:0.0}]";
        }

    }
}
