using System;
using System.Collections.Generic;
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
        public virtual void Pack(Part[] parts, StockItem[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // do...repeat until no new parts are placed in this iteration
            do
            {
                #region // filter parts and boards for items that are not already packed ...
                Part[] unpackedParts = parts.Where(p => !p.IsPacked).ToArray();
                StockItem[] incompleteBoards = boards.Where(p => !p.IsComplete).ToArray(); 
                #endregion

                #region // Fire off a thread per incomplete board to pack that board using the unplaced parts ...
                List<Task> threads = new List<Task>();
                foreach (var iIncompleteBoard in incompleteBoards)
                {
                    // clear the board's packing
                    iIncompleteBoard.PackedParts = null;
                    iIncompleteBoard.PackedPartsCount = 0;
                    iIncompleteBoard.PackedPartsTotalArea = 0;

                    threads.Add(Task.Factory.StartNew((o) =>
                    {
                        StockItem iBoard = o as StockItem;

                        // repack it
                        PackBoard(unpackedParts, iBoard, sawkerf, partLengthPadding, partWidthPadding);
                    }, iIncompleteBoard));
                }
                    
                #endregion

                // Wait for all threads to complete
                Task.WaitAll(threads.ToArray());

                #region // Find the best packed board from this iteration ...
                StockItem[] newlyPackedBoards = boards.Where(q => !q.IsComplete && q.PackedPartsCount > 0).ToArray();
                StockItem BestPackedBoard = newlyPackedBoards.OrderByDescending(t => t.PackingCoverage).FirstOrDefault();
                #endregion

                // If no board could be packed, exit
                if (BestPackedBoard == null) break;

                #region // Set the board and parts as completed for best packed board ...
                // set board complete
                BestPackedBoard.IsComplete = true;
                
                //Compact the packed parts array of the board
                Array.Resize<Placement>(ref BestPackedBoard.PackedParts, BestPackedBoard.PackedPartsCount);

                // set the packed flag for the packed parts
                BestPackedBoard.PackedParts.ToList().ForEach(t => t.Part.IsPacked = true);


                #endregion

                #region // clear the packing for the unsuccesfull board(s) ...
                newlyPackedBoards.ToList().ForEach(i =>
                {
                    if (!i.IsComplete)
                    {
                        i.PackedParts = null;
                        i.PackedPartsCount = 0;
                        i.PackedPartsTotalArea = 0;
                    }
                }); 
                #endregion
            } while (true);

        }

        protected virtual void PackBoard(Part[] parts, StockItem boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
        }
    }
}
