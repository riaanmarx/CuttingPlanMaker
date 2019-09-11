using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    /// <summary>
    /// Interface required by all packer implementations
    /// </summary>
    class PackerBase
    {
        /// <summary>
        /// A name for this algorithm implementation
        /// </summary>
        public static string AlgorithmName => "BASE";

        /// <summary>
        /// A public method to start the packing of parts onto boards
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="boards"></param>
        /// <param name="sawkerf"></param>
        /// <param name="partLengthPadding"></param>
        /// <param name="partWidthPadding"></param>
        public virtual void Pack(Part[] parts, Board[] boards, double sawkerf = 3.2)
        {
            throw new Exception("Please override Pack() method when inheritting from PackerBase");
        }

    }
}
