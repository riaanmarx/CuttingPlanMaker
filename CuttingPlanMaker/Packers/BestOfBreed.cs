using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    class BestOfBreed : PackerBase
    {
        /// <summary>
        /// Use all the packers to pack all boards.
        /// Find the board packed best accross all boards and all packars
        ///     remove the board and packed parts
        /// rince and repeat....
        /// </summary>


        new public static string AlgorithmName => "Best packers iterative";
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
                    && p.FullName != "CuttingPlanMaker.Packers.BestOfBreed");

            do
            {
                // pack the parts onto the boards using each of the algorithms
                List<Part[]> partsperpacker = new List<Part[]>();
                foreach (var iPacker in packerTypes)
                {
                    // make a 1-level deep copy of the parts and board arrays of all unpacked
                    Part[] iParts_alg = parts.Where(f => f.Source == null).Select(p => p.Clone()).ToArray();
                    partsperpacker.Add(iParts_alg);
                    Board[] iBoards_alg = boards.Where(b => b.IsComplete == false).Select(s => s.Clone()).ToArray();

                    // use existing algorthm to pack the boards
                    PackerBase iPackerInstance = (PackerBase)Activator.CreateInstance(iPacker);
                    iPackerInstance.Pack(iParts_alg, iBoards_alg, sawkerf);
                }

                // find best packed board through all algorithms
                Part[] bestPacking = null;
                double leastArea = double.MaxValue;
                Board bestPackedBoard = null;
                // if better than prev, remember it...
                for (int iAlg = 0; iAlg < packerTypes.Count(); iAlg++)
                {
                    //loop through each board
                    foreach (var iBoard in boards)
                    {
                        //check which board is packed best...
                        double packedArea = partsperpacker[iAlg].Where(f => f.Source?.Name == iBoard.Name).Sum(p => p.Area);
                        double availArea = iBoard.Area - packedArea;
                        if (packedArea > 0 && availArea < leastArea)
                        {
                            leastArea = availArea;
                            bestPacking = partsperpacker[iAlg];
                            bestPackedBoard = iBoard;
                        }
                    }
                }

                if (bestPackedBoard == null) return;

                // return best result to caller
                bestPackedBoard.IsComplete = true;
                
                bestPacking.Where(p => p.Source?.Name == bestPackedBoard.Name).ToList().ForEach(p =>
                  {
                      var t = parts.FirstOrDefault<Part>(o => o.Name == p.Name);
                      if (t != null)
                      {
                          t.Source = bestPackedBoard;
                          t.OffsetLength = p.OffsetLength;
                          t.OffsetWidth = p.OffsetWidth;
                      }
                  });

            } while (true);

        }
    }
}
