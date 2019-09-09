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
    public class Board
    {
        /// <summary>
        /// A name for the board
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The material this board is made of
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// The length (grain-direction) dimension of the board
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// The width (cross-grain) dimension of the board
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Calculated area of the board
        /// </summary>
        public double Area => Length * Width;

        /// <summary>
        /// Flag indicating if this board has been packed already
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Indication of weather this board shoulf be repacked by the packer algorithm
        /// </summary>
        public bool IsFrozen { get; set; }

        public double OffsetLength { get; set; }
        public double OffsetWidth { get; set; }


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
