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
    class PerBoardPackerBase : PackerBase
    {
        public static string AlgorithmName => "BASE";

        class context
        {
            public Part[] parts;
            public Board board;
        }

        /// <summary>
        /// A public method to start the packing of parts onto boards
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="boards"></param>
        /// <param name="sawkerf"></param>
        /// <param name="partLengthPadding"></param>
        /// <param name="partWidthPadding"></param>
        public override sealed void Pack(Part[] parts, Board[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // do...repeat until no new parts are placed in this iteration
            Stopwatch sw = new Stopwatch();
            sw.Start();
            do
            {
                #region // filter parts and boards for items that are not already packed ...
                Part[] unpackedParts = parts.Where(p => p.Source == null).ToArray();
                Board[] incompleteBoards = boards.Where(p => !p.IsComplete).ToArray(); 
                #endregion

                #region // Fire off a thread per incomplete board to pack that board using the unplaced parts ...
                // remember that if we follow this strategy of packing all boards simultaniously in a multi-threaded fashion
                //   the ojects are passed byref, so be carefull not to edit the same instance from 2 threads, or expect the object to be unchanged...
                // For this reason it is probably best to create a seperate instance if ibjects you intend to change for each of the threads 
                // we are not changing the board instance, so that can be passed in,
                //  but to place the part, we change the source property etc. so each thread need its own instance and after packing, the best packed board's packing info
                //      must be transferred to the original list of parts
                
                Dictionary<Task,context> threads = new Dictionary<Task, context>();

                foreach (var iIncompleteBoard in incompleteBoards)
                {
                    //create copy of parts[]
                    context con = new context {
                        parts = unpackedParts.Select(p => p.Clone()).ToArray(),
                        board = iIncompleteBoard
                    };
                    threads.Add(Task.Factory.StartNew((o) =>
                    {
                        try
                        {
                            context oo = o as context;
                            PackBoard(oo.parts, oo.board, sawkerf, partLengthPadding, partWidthPadding);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"Exception occured while packing:{ex}");
                        }
                    }, con), con);
                }

                #endregion
                
                // Wait for all threads to complete
                Task.WaitAll(threads.Select(t=>t.Key).ToArray());

                #region // Find the best packed board from this iteration ...
                KeyValuePair<Task, context> bestThread = threads.First();
                double bestCoverage = 0;
                foreach (var iThread in threads)
                {
                    var iCoverage = iThread.Value.parts.Where(f=>f.Source == iThread.Value.board).Sum(p=>p.Area) / iThread.Value.board.Area;

                    if (iCoverage > bestCoverage)
                    {
                        bestCoverage = iCoverage;
                        bestThread = iThread;
                    }
                }
                #endregion

                // If no board could be packed, exit
                if (bestCoverage == 0)
                {
                    Trace.WriteLine("No further parts packed - exiting.");
                    break;
                }

                #region // Set the board complete and transfer the part sources back to the original parts array for best packed board ...
                // set board complete
                bestThread.Value.board.IsComplete = true;

                // transfer the part sources & offsets 
                bestThread.Value.parts.Where(f => f.Source != null).ToList().ForEach(p => {
                    var origpart = parts.First(op => op.Name == p.Name);
                    origpart.Source = p.Source;
                    origpart.OffsetLength = p.OffsetLength;
                    origpart.OffsetWidth = p.OffsetWidth;
                    });

                Trace.WriteLine($"Board {bestThread.Value.board.Name} packed best to {bestCoverage:0.00%}");
                #endregion

            } while (true);
            sw.Stop();
#if DEBUG

            if (parts.Any(t => t.Source == null))
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
            foreach (var iBoard in boards)
            {
                TotalStockArea += iBoard.Area;

                var packedParts = parts.Where(p => p.Source == iBoard);
                if (packedParts.Count() == 0)
                    Trace.WriteLine($"   Board {iBoard.Name} [{iBoard.Length,6:0.0} x {iBoard.Width,5:0.0}] : not used.");
                else
                {
                    UsedStockArea += iBoard.Area;
                    UsedStockCount++;
                    Trace.WriteLine($"   Board {iBoard.Name} [{iBoard.Length,6:0.0} x {iBoard.Width,5:0.0}] ({(packedParts.Sum(p=>p.Area) / iBoard.Area):00.0 %}) :");
                    UsedPartsArea += packedParts.Sum(p => p.Area);
                    TotalPackedPartsCount += packedParts.Count();
                    foreach (var iPackedPart in packedParts)
                        Trace.WriteLine($"{iPackedPart.Name,10} [{iPackedPart.Length,7:0.0} x {iPackedPart.Width,5:0.0}] @ ({iPackedPart.OffsetLength,7:0.0},{iPackedPart.OffsetWidth,7:0.0})");    
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
        }

        protected virtual void PackBoard(Part[] parts, Board board, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            throw new Exception("Classes inheritting from PerBoardPackerBase must override PackBoard()");
        }
    }
}
