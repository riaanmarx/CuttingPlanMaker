using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// A data model class for the stock items or boards 
    /// </summary>
    public class StockItem
    {
        /// <summary>
        /// A name for the board
        /// </summary>
        [CsvColumn(Name = "Name", FieldIndex = 1)]
        public string Name { get; set; }

        /// <summary>
        /// The material this board is made of
        /// </summary>
        [CsvColumn(Name = "Material", FieldIndex = 2)]
        public string Material { get; set; }

        /// <summary>
        /// The length (grain-direction) dimension of the board
        /// </summary>
        [CsvColumn(Name = "Length", FieldIndex = 3)]
        public double Length { get; set; }

        /// <summary>
        /// The width (cross-grain) dimension of the board
        /// </summary>
        [CsvColumn(Name = "Width", FieldIndex = 4)]
        public double Width { get; set; }

        /// <summary>
        /// Calculated area of the board
        /// </summary>
        public double Area => Length * Width;

        /// <summary>
        /// Array of part placements (part and offset)
        /// </summary>
        public Placement[] PackedParts;

        /// <summary>
        /// Total area of the parts packed on this board
        /// </summary>
        public double PackedPartsTotalArea => PackedParts == null ? 0 : PackedParts.Sum(s => s?.Part?.Area ?? 0);// { get; set; }

        /// <summary>
        /// Count of parts packed on this board
        /// </summary>
        public int PackedPartsCount
        //{ get; set; }
        {
            get
            {
                //return cnt;
                if (PackedParts == null) return 0;
                for (int i = 0; i < PackedParts.Length; i++)
                    if (PackedParts[i] == null) return i;
                return PackedParts.Length;
            }
            set {
                cnt = value;
            }
        }
        private int cnt = 0;

        public double PackingCoverage => PackedPartsTotalArea / Area;

        /// <summary>
        /// Flag indicating if this board has been packed already
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Indication of weather this board shoulf be repacked by the packer algorithm
        /// </summary>
        public bool IsFrozen { get; set; }


        /// <summary>
        /// String repressentation of the board
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} [{Length,7:0.0} x {Width,5:0.0}]";
        }

    }
}
