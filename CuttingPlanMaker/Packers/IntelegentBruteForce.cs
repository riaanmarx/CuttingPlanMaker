using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    /// <summary>
    /// I want to revisit the Brute force method of trying the parts in every order, placing one at a time until none can be placed further.
    /// Then looking at how much space is unused.
    /// 
    /// We cannot calculate the area for each combination and keep that.there will be 2^n 
    /// 
    /// We are looking at a binary tree recursion function which 
    ///     0) excludes the current item and packs rest
    ///     1) includes the current item and packs rest
    /// </summary>
    class IntelegentBruteForce : PerBoardPackerBase
    {
        new public static string AlgorithmName => "Inteligent Brute Force";

        private List<byte[]> Combos;

        private void GetCombos(Part[] parts, double maxArea, int pos, double currentArea, byte[] currentMask)
        {
            // if we reach the end of the list, return the current area and an empty list of indexes which will be populated as we unstack the recurrances
            if (pos >= parts.Length) return;

            // calculate new area if this part would have been added
            double newArea = currentArea + parts[pos].Area;

            // if the new area is still smaller than the max area
            if (newArea < maxArea)
            {
                // add this item to the mask
                var newMask = (byte[])currentMask.Clone();
                newMask[pos] = 1;

                // add this new combo to the list
                Combos.Add(newMask);

                // recurs with the new area on the next part...
                GetCombos(parts, maxArea, pos + 1, newArea, newMask);
            }

            // also attempt to pack not using this part, and just return the packing result for the next part
            GetCombos(parts, maxArea, pos + 1, currentArea, currentMask);
        }


        internal override void PackBoard(Part[] parts, Board board, double sawkerf = 3.2)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // first order the parts by area
            var sortedParts = parts.OrderBy(o => o.Area).ToArray();

            // calculate list of combinations of the parts that has area less than board
            Combos = new List<byte[]>();
            GetCombos(sortedParts, board.Area, 0, 0, new byte[parts.Length]);
            
            //var sortedcombinations = combinations.OrderByDescending(t => t.Sum(s => s.Area)).ToArray();

            sw.Stop();

            // for each combination of parts
            //      brute force pack the board with full recursion
        }

    }
}
