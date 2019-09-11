using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// Class to keep track of the different types of material we are using in the project
    /// </summary>
    class Material
    {
        /// <summary>
        /// Name for the material
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Thickness of boards of this material
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Volume cost of the material
        /// </summary>
        public double Cost { get; set; }
    }
}
