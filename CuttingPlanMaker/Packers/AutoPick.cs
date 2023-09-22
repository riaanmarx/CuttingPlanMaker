using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    class AutoPick : PackerBase
    {
        /// <summary>
        /// Use all the packers to pack all boards.
        /// Keep the one that wasted the least
        /// </summary>


        new public static string AlgorithmName => "Auto pick algoritm";
        public override void Pack(Part[] parts, Board[] boards, double sawkerf = 3.2)
        {

            //in seperate threads per algorithm detected, pack the parts on the boards
            //for each thread/algorithm
            //    for each board
            //        check if board has least waste(mm2)
            //solidify this board's packing
            //remove board and parts used from the lists
            //if boards==0 and parts not, not possible
            //if parts==0 and boards not, done


            //get a list of the packers, excluding the bases and the per-board BoB
            var type = typeof(PackerBase);
            var packerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)
                    && p.FullName != "CuttingPlanMaker.Packers.PackerBase"
                    && p.FullName != "CuttingPlanMaker.Packers.PerBoardPackerBase"
                    && p.FullName != "CuttingPlanMaker.Packers.PerBoardBestOfBreed"
                    && p.FullName != "CuttingPlanMaker.Packers.BestOfBreed"
                    && p.FullName != "CuttingPlanMaker.Packers.AutoPick");

            // pack the parts onto the boards using each of the algorithms
            List<Part[]> partsperpacker = new List<Part[]>();
            double leasttotalwaste = double.MaxValue;
            Part[] bestParts = null;
            Board[] bestBoards = null;
            foreach (var iPacker in packerTypes)
            {
                // make a 1-level deep copy of the parts and board arrays of all unpacked
                Part[] iParts_alg = parts.Select(p => p.Clone()).ToArray();
                Board[] iBoards_alg = boards.Select(s => s.Clone()).ToArray();
                
                // use existing algorthm to pack the boards
                PackerBase iPackerInstance = (PackerBase)Activator.CreateInstance(iPacker);
                iPackerInstance.Pack(iParts_alg, iBoards_alg, sawkerf);

                double totalstockarea = iBoards_alg.Where(f => f.AreaUsed > 0).Sum(s => s.Area);
                double totalusedarea = iBoards_alg.Where(f => f.AreaUsed > 0).Sum(s => s.AreaUsed);
                double totalwaste = (totalstockarea - totalusedarea) / totalstockarea;

                if (totalwaste < leasttotalwaste)
                {
                    leasttotalwaste = totalwaste;
                    bestParts = iParts_alg;
                    bestBoards = iBoards_alg;
                }
            }
            //transfer usage
            if (bestBoards != null)
                foreach (var iBoard in boards)
                {
                    var b = bestBoards.First(f => f.Name == iBoard.Name);
                    iBoard.AreaUsed = b.AreaUsed;
                    iBoard.IsComplete = b.IsComplete;
                }
            //transfer packings
            if (bestParts != null)
                foreach (var iPart in parts)
                {
                    var t = bestParts.First<Part>(f => f.Name == iPart.Name);
                        iPart.Source = boards.First(f => f.Name == t.Source.Name);
                        iPart.OffsetLength = t.OffsetLength;
                        iPart.OffsetWidth = t.OffsetWidth;
                }
        }
    }
}
