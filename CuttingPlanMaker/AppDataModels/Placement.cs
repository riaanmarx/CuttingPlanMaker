using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// A class used by the StockItem class to keep track of the parts placed on the board in question
    /// </summary>
    public class Placement
    {
        /// <summary>
        /// The part object instance
        /// </summary>
        public Part Part { get; set; }

        /// <summary>
        /// The length-wise (parallel with grain) offset of the part's placement
        /// </summary>
        public double dLength { get; set; }

        /// <summary>
        /// The width-wise (accross the grain) offset of the part's placement
        /// </summary>
        public double dWidth { get; set; }
    }
}
