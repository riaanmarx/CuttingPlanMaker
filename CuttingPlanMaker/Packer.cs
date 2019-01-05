using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    public class Packer
    {
        /// <summary>
        ///  do the internal preperation to pack a set of parts onto a set of boards with a collection of options
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="boards"></param>
        /// <param name="sawkerf"></param>
        /// <param name="boardMarginLength"></param>
        /// <param name="boardMarginWidth"></param>
        /// <param name="partLengthPadding"></param>
        /// <param name="partWidthPadding"></param>
        /// <returns></returns>
        static public void Pack(Part[] parts, StockItem[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // order the parts and boards by Area, Ascending
            int partsCount = parts.Length;
            Part[] orderredParts = parts.OrderBy(t => t.Area).ToArray();
            int boardsCount = boards.Length;
            StockItem[] orderredBoards = boards.OrderBy(t => t.Area).ToArray();

            // add padding to all parts
            if (partLengthPadding > 0 || partWidthPadding > 0)
                orderredParts.ToList().ForEach(t => t.Inflate(partWidthPadding, partLengthPadding));

            // keep count of the parts and boards used
            int packedPartsCount = 0;
            int packedBoardsCount = 0;

            // repeat until all parts are placed, or all boards packed
            while (packedPartsCount < partsCount && packedBoardsCount < boardsCount)
            {
                Task[] threads = new Task[boardsCount];
                for (int i = 0; i < boardsCount; i++)
                {
                    threads[i] = Task.Factory.StartNew((o) =>
                    {
                        // reference board[i]
                        StockItem iBoard = orderredBoards[(int)o];
                        if (iBoard.isComplete) return;

                        // init a packer object
                        Packer_internal iPacker = new Packer_internal()
                        {
                            sawkerf = sawkerf,
                            Board = iBoard,
                            Parts = orderredParts,
                            PartsCount = partsCount,
                            BoardSections = new StockItem[2 * partsCount + 2],
                            BoardSectionsCount = 1,
                            CurrentSolution = new Part[partsCount],
                            CurrentSolutionDLengths = new double[partsCount],
                            CurrentSolutionDWidths = new double[partsCount]
                        };
                        iPacker.BoardSections[0] = new StockItem(iBoard.Name, iBoard.Length, iBoard.Width, iBoard.dLength, iBoard.dWidth);

                        // pack the board recursively, starting at the first part and an empty solution
                        iPacker.StartPacking();
                    }, i);
                }
                Task.WaitAll(threads);

                // Find the best packed board from this iteration
                IEnumerable<StockItem> incompleteBoards = orderredBoards.Where(q => !q.isComplete);
                StockItem BestCoverredBoard = incompleteBoards.OrderByDescending(t => t.PackedPartsTotalArea / t.Area).FirstOrDefault();

                // If no board could be packed, exit
                if (BestCoverredBoard == null)
                    break;

                // Clear the inferior packings
                incompleteBoards.Where(t => t != BestCoverredBoard).ToList().ForEach(t =>
                {
                    t.PackedParts = null;
                    t.PackedPartdLengths = null;
                    t.PackedPartdWidths = null;
                    t.PackedPartsCount = 0;
                    t.PackedPartsTotalArea = 0;
                });

                // set the complete flag for the board with the best coverage
                BestCoverredBoard.isComplete = true;
                packedBoardsCount++;

                //Compact the packed parts array of the board
                BestCoverredBoard.PackedParts = BestCoverredBoard.PackedParts.Where(t => t != null).ToArray();

                // set the packed flag for the packed parts
                BestCoverredBoard.PackedParts.ToList().ForEach(t => t.isPacked = true);
                packedPartsCount += BestCoverredBoard.PackedPartsCount;
            }

            // remove padding to all parts
            if (partLengthPadding > 0 || partWidthPadding > 0)
                orderredParts.ToList().ForEach(t => t.Inflate(-partWidthPadding, -partLengthPadding));
        }

        private class Packer_internal
        {
            public StockItem Board;
            public Part[] Parts;
            public int PartsCount;

            public StockItem[] BoardSections;
            public int BoardSectionsCount;

            public Part[] CurrentSolution;
            public double[] CurrentSolutionDLengths;
            public double[] CurrentSolutionDWidths;
            public int CurrentSolutionPartCount;
            public double CurrentSolutionTotalArea;

            public double sawkerf;

            public void StartPacking()
            {
                double lastPartLength = -1;
                double lastPartWidth = -1;

                // loop through the parts, from big to small
                for (int i = PartsCount - 1; i >= 0; i--)
                {
                    Part iPart = Parts[i];

                    #region // check if the part is a viable candidate ...
                    // ignore parts already packed
                    if (iPart.isPacked) continue;
                    // ignore parts larger than the largest board section
                    if (iPart.Area > Board.Area) continue;
                    // short-circuit repeat parts
                    if (iPart.Length == lastPartLength && iPart.Width == lastPartWidth) continue;
                    // ignore parts already temporarily packed
                    if (CurrentSolution.Contains(iPart)) continue;

                    lastPartLength = iPart.Length;
                    lastPartWidth = iPart.Width;
                    #endregion

                    #region // Find first board that will fit the part ...
                    // find first board that will accomodate the part
                    int j = 0;
                    while (j < BoardSectionsCount && BoardSections[j].Area < iPart.Area) j++;
                    while (j < BoardSectionsCount && (BoardSections[j].isInUse || iPart.Length > BoardSections[j].Length || iPart.Width > BoardSections[j].Width)) j++;

                    // if no boards will accomodate the part, continue to next part
                    if (j >= BoardSectionsCount) continue;
                    StockItem iBoardSection = BoardSections[j];
                    #endregion

                    #region // place the part in the current bin ...
                    CurrentSolutionDLengths[CurrentSolutionPartCount] = iBoardSection.dLength;
                    CurrentSolutionDWidths[CurrentSolutionPartCount] = iBoardSection.dWidth;
                    CurrentSolution[CurrentSolutionPartCount++] = iPart;
                    CurrentSolutionTotalArea += iPart.Area;
                    #endregion

                    #region // store best solution ...
                    //if this is a better solution than the current best one ... replace the current best one
                    if (CurrentSolutionTotalArea > Board.PackedPartsTotalArea)
                    {
                        Board.PackedParts = CurrentSolution.Clone() as Part[];
                        Board.PackedPartdLengths = CurrentSolutionDLengths.Clone() as double[];
                        Board.PackedPartdWidths = CurrentSolutionDWidths.Clone() as double[];
                        Board.PackedPartsCount = CurrentSolutionPartCount;
                        Board.PackedPartsTotalArea = CurrentSolutionTotalArea;
                    }
                    #endregion

                    #region // Break association and adjust associated boards if a board is used that is associated with another to prevent overlapping placements ...
                    StockItem iAssocBoardSection = iBoardSection.AssociatedBoard;
                    double oAssocLength = 0,
                        oAssocWidth = 0,
                        oiBoardLength = 0,
                        oiBoardWidth = 0;
                    // if the board section used has a buddy from a previous placement
                    if (iAssocBoardSection != null)
                    {
                        // keep old sizes so we can revert them at the end of the iteration
                        oAssocLength = iAssocBoardSection.Length;
                        oAssocWidth = iAssocBoardSection.Width;
                        oiBoardLength = iBoardSection.Length;
                        oiBoardWidth = iBoardSection.Width;

                        // if the part was placed on rem1 (the left most board section)
                        if (iBoardSection.dWidth < iAssocBoardSection.dWidth)
                        {
                            //if the part overlaps into rem2
                            if (iBoardSection.dWidth + iPart.Width + sawkerf > iAssocBoardSection.dWidth)
                                // adjust the length of rem2 so it does not overlap this part
                                iAssocBoardSection.Length -= (iBoardSection.Length + sawkerf);
                            else
                                // adjust rem1 so it does not overlap rem2
                                iBoardSection.Width -= (iAssocBoardSection.Width + sawkerf);
                        }
                        else
                        {
                            // ...part was placed on rem2 (the right most board section)
                            // if the part overlaps onto rem1
                            if (iBoardSection.dLength + iPart.Length + sawkerf > iAssocBoardSection.dLength)
                                // adjust rem1 so it does not overlap the part
                                iAssocBoardSection.Width -= (iBoardSection.Width + sawkerf);
                            else
                                // adjust rem2 so it does not overlap rem1
                                iBoardSection.Length -= (iAssocBoardSection.Length + sawkerf);
                        }

                        // break the association
                        iAssocBoardSection.AssociatedBoard = null;
                        iBoardSection.AssociatedBoard = null;
                    }
                    #endregion

                    #region // replace the used board with 2 overlapping remainder pieces after subtracting the part ...
                    // divide the board into two overlapping remainder sections
                    iBoardSection.isInUse = true;

                    // create new sections
                    StockItem boardSection1 = new StockItem(iBoardSection.Name, iBoardSection.Length - iPart.Length - sawkerf, iBoardSection.Width, iBoardSection.dLength + iPart.Length + sawkerf, iBoardSection.dWidth);
                    StockItem boardSection2 = new StockItem(iBoardSection.Name, iBoardSection.Length, iBoardSection.Width - iPart.Width - sawkerf, iBoardSection.dLength, iBoardSection.dWidth + iPart.Width + sawkerf);
                    boardSection1.AssociatedBoard = boardSection2;
                    boardSection2.AssociatedBoard = boardSection1;
                    int boardSection1Index = BoardSectionsCount;
                    int boardSection2Index = BoardSectionsCount;

                    if (boardSection1.Area > Parts[0].Area)
                    {
                        // insert the new rem1 section so the boardsections remain sorted by area
                        for (boardSection1Index = BoardSectionsCount; ; boardSection1Index--)
                            if (boardSection1Index > 0 && BoardSections[boardSection1Index - 1].Area > boardSection1.Area)
                                BoardSections[boardSection1Index] = BoardSections[boardSection1Index - 1];
                            else
                            {
                                BoardSections[boardSection1Index] = boardSection1;
                                break;
                            }
                        BoardSectionsCount++;
                    }
                    else
                    {
                        boardSection1 = null;
                        boardSection2.AssociatedBoard = null;
                    }


                    if (boardSection2.Area > Parts[0].Area)
                    {
                        // insert the new rem2 section so the boardsections remain sorted by area
                        for (boardSection2Index = BoardSectionsCount; ; boardSection2Index--)
                            if (boardSection2Index > 0 && BoardSections[boardSection2Index - 1].Area > boardSection2.Area)
                                BoardSections[boardSection2Index] = BoardSections[boardSection2Index - 1];
                            else
                            {
                                BoardSections[boardSection2Index] = boardSection2;
                                break;
                            }
                        BoardSectionsCount++;
                    }
                    else
                    {
                        boardSection2 = null;
                        if (boardSection1 != null) boardSection1.AssociatedBoard = null;
                    }

                    #endregion

                    #region // pack the remaining parts on the remaining boards ...
                    // pack the remaining parts on the remaining boards
                    StartPacking();
                    #endregion

                    #region // undo the placement so we can iterate to the next part and test with it ...

                    // remove the remainder board sections we added...
                    if (boardSection2Index < BoardSectionsCount)
                    {
                        for (int irem = boardSection2Index; irem < BoardSectionsCount; irem++)
                            BoardSections[irem] = BoardSections[irem + 1];
                        BoardSectionsCount--;
                    }

                    if (boardSection1Index < BoardSectionsCount)
                    {
                        for (int irem = boardSection1Index; irem < BoardSectionsCount; irem++)
                            BoardSections[irem] = BoardSections[irem + 1];
                        BoardSectionsCount--;
                    }

                    // restore associations, and the original associated board sections' sizes
                    if (iAssocBoardSection != null)
                    {
                        iBoardSection.AssociatedBoard = iAssocBoardSection;
                        iAssocBoardSection.AssociatedBoard = iBoardSection;
                        iAssocBoardSection.Length = oAssocLength;
                        iAssocBoardSection.Width = oAssocWidth;
                        iBoardSection.Length = oiBoardLength;
                        iBoardSection.Width = oiBoardWidth;
                    }

                    // place the board back in play
                    iBoardSection.isInUse = false;

                    // remove the part from the temporary solution
                    CurrentSolution[--CurrentSolutionPartCount] = null;
                    CurrentSolutionTotalArea -= iPart.Area;

                    #endregion
                }

            }

        }

    }
}
