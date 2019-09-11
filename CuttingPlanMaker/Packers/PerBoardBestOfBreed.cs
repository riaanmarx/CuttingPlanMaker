using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    class PerBoardBestOfBreed : PerBoardPackerBase
    {
        //An algorithm that uses all the per-board algorithms, and 
        //  for each board, calculate the best packing, using all algorithms

        public new static string AlgorithmName => "Per Board Best-Of-Breed";

        internal override void PackBoard(Part[] parts, Board board, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // given a board and collection of parts,
            // pack the parts into the board using all known algorithms
            // pick the best of the packings and return it

            var type = typeof(PerBoardPackerBase);
            var packerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.FullName != "CuttingPlanMaker.Packers.PerBoardPackerBase" && p.FullName != "CuttingPlanMaker.Packers.PerBoardBestOfBreed");
            Part[] bestPacking = null;
            double bestArea = 0;

            foreach (var iPacker in packerTypes)
            {
                // make a 1-level deep copy of the parts array
                Part[] iParts_alg = parts.Select(p => p.Clone()).ToArray();

                // use existing algorthm to pack the board
                PerBoardPackerBase iPackerInstance = (PerBoardPackerBase)Activator.CreateInstance(iPacker);
                iPackerInstance.PackBoard(iParts_alg, board, sawkerf, partLengthPadding, partWidthPadding);

                // if better than prev, remember it...
                double newArea = iParts_alg.Where(f=>f.Source != null).Sum(p => p.Area);
                if(newArea > bestArea)
                {
                    bestArea = newArea;
                    bestPacking = iParts_alg;
                }
            }

            // return best result to caller
            bestPacking.ToList().ForEach(p =>
            {
                var t = parts.First<Part>(o => o.Name == p.Name);
                t.Source = p.Source;
                t.OffsetLength = p.OffsetLength;
                t.OffsetWidth = p.OffsetWidth;
            });

        }
    }
}
