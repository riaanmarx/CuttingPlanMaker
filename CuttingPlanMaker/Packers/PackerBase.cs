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
        public virtual void Pack(Part[] parts, StockItem[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // do...repeat until no new parts are placed in this iteration
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
                    //iIncompleteBoard.PackedPartsTotalArea = 0;

                    threads.Add(Task.Factory.StartNew((o) =>
                    {
                        try
                        {
                            StockItem iBoard = o as StockItem;

                            // repack it
                            PackBoard(unpackedParts, iBoard, sawkerf, partLengthPadding, partWidthPadding);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"Exception occured while packing:{ex}");
                        }
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
                if (BestPackedBoard == null)
                {
                    Trace.WriteLine("No further parts packed - exiting.");
                    break;
                }

                #region // Set the board and parts as completed for best packed board ...
                // set board complete
                BestPackedBoard.IsComplete = true;
                
                //Compact the packed parts array of the board
                Array.Resize<Placement>(ref BestPackedBoard.PackedParts, BestPackedBoard.PackedPartsCount);

                // set the packed flag for the packed parts
                BestPackedBoard.PackedParts.ToList().ForEach(t => t.Part.IsPacked = true);
                Trace.WriteLine($"Board {BestPackedBoard.Name} packed best to {BestPackedBoard.PackingCoverage:0.00%}");
                #endregion

                #region // clear the packing for the unsuccesfull board(s) ...
                newlyPackedBoards.ToList().ForEach(i =>
                {
                    if (!i.IsComplete)
                    {
                        i.PackedParts = null;
                        i.PackedPartsCount = 0;
                    }
                }); 
                #endregion
            } while (true);
            sw.Stop();
#if DEBUG

            if (parts.Any(t => !t.IsPacked))
                Trace.WriteLine("Processing completed with WARNING: All parts could not be placed!\r\n");
            else
                Trace.WriteLine($"Processing completed succesfully.\r\n");
            Trace.WriteLine("===========================================================");
            Trace.WriteLine("Solution detail:");
            Trace.WriteLine("----------------");
            double UsedStockArea = 0;
            int UsedStockCount = 0;
            double TotalStockArea = 0;
            int TotalPackedPartsCount = 0;
            double UsedPartsArea = 0;
            for (int i = 0; i < boards.Length; i++)
            {
                StockItem iBoard = boards[i];
                TotalStockArea += iBoard.Area;

                if (iBoard.PackedPartsCount == 0)
                    Trace.WriteLine($"   Board {iBoard.Name} [{iBoard.Length,6:0.0} x {iBoard.Width,5:0.0}] : not used.");
                else
                {
                    UsedStockArea += iBoard.Area;
                    UsedStockCount++;
                    Trace.WriteLine($"   Board {iBoard.Name} [{iBoard.Length,6:0.0} x {iBoard.Width,5:0.0}] ({(iBoard.PackedParts == null ? 0 : iBoard.PackedPartsTotalArea / iBoard.Area):00.0 %}) :");
                    UsedPartsArea += iBoard.PackedPartsTotalArea;
                    TotalPackedPartsCount += iBoard.PackedPartsCount;
                    for (int j = 0; j < iBoard.PackedPartsCount; j++)
                        Trace.WriteLine($"{iBoard.PackedParts?[j].Part.Name,10} [{iBoard.PackedParts?[j].Part.Length,7:0.0} x {iBoard.PackedParts?[j].Part.Width,5:0.0}] @ ({iBoard.PackedParts[j].dLength,7:0.0},{iBoard.PackedParts[j].dWidth,7:0.0})");
                }
            }
            double TotalPartsArea = 0;
            for (int i = 0; i < parts.Length; i++)
                TotalPartsArea += parts[i].Area;
            Trace.WriteLine("===========================================================");
            Trace.WriteLine("Solution summary:");
            Trace.WriteLine("----------------_");
            Trace.WriteLine($"   Processing time: {sw.ElapsedMilliseconds,5:0} ms");
            Trace.WriteLine($"   Boards         : {boards.Length,5:0}    ({TotalStockArea / 1000000,6:0.000} m\u00b2)");
            Trace.WriteLine($"   Used boards    : {UsedStockCount,5:0}    ({UsedStockArea / 1000000,6:0.000} m\u00b2)");
            Trace.WriteLine($"   Parts          : {parts.Length,5:0}    ({TotalPartsArea / 1000000,6:0.000} m\u00b2)");
            Trace.WriteLine($"   Placed parts   : {TotalPackedPartsCount,5:0}    ({UsedPartsArea / 1000000,6:0.000} m\u00b2)");
            Trace.WriteLine($"   Waste          : {(UsedStockArea - UsedPartsArea) / UsedStockArea,7:0.0 %}  ({(UsedStockArea - UsedPartsArea) / 1000000,6:0.000} m\u00b2)");
            Trace.WriteLine($"   Coverage       : {UsedPartsArea / UsedStockArea,7:0.0 %}  ({UsedPartsArea / 1000000,6:0.000} m\u00b2)");
            Trace.WriteLine($"Packing completed in {sw.ElapsedMilliseconds} ms"); 
#endif
            //Trace.WriteLine()
        }

        protected virtual void PackBoard(Part[] parts, StockItem boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
        }
    }
}
