using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    /// <summary>
    /// In this packer, a space is replaced with 1 or 2 horiz spaces when a part is placed. 
    ///     if the part spans the width of the space, the existing space is shortened and offset to right of the part
    ///     if the part partially spans the width of a space, the space is shortened and offset, and width decreased to overlapping width and
    ///          second space is introduced spanning the original space horizontally, below the part placement.
    ///     Spaces are orderred ascending according to origin distance from board origin.
    ///     Spaces have links to next space below them
    ///     max width at space would be:
    ///         
    ///     Avail parts are orderred desc on width, then length
    ///     loop through spaces
    ///         loop through avail parts
    ///         find first part that fits combined space width for spaces below and to left
    ///     
    ///     
    /// </summary>
    class HorRectSpannable : PerBoardPackerBase
    {
        /// <summary>
        /// Name
        /// </summary>
        new public static string AlgorithmName => "Combined Hor. Rects";

        internal override void PackBoard(Part[] parts, Board board, double sawkerf = 3.2)
        {
            return;
        }
    }
}
