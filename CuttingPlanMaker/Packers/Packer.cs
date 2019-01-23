using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    /// <summary>
    /// first(failed) attempt at a packer...takes too long when iterating through all possible combinations
    /// replaced with another simpler, quicker algorithm
    /// </summary>
    class Packer : PackerBase
    {
        new public static string AlgorithmName => "MAXRECT0-BL";

        private Bitmap Drawboard_debug(StockItem board, OffsetStockItem[] segments, int segcount, Part[] parts, double[] dLengths, double[] dWidths, int partcount, double partsArea)
        {
            double xMargin = 50;
            double yMargin = 50;

            double imageHeight = board.Width + 2 * yMargin;
            double imageWidth = board.Length + 2 * xMargin;

            // create bitmap
            Bitmap bitmap = new Bitmap((int)imageWidth, (int)imageHeight);
            Graphics g = Graphics.FromImage(bitmap);
            // draw the board
            g.DrawRectangle(Pens.Black, (float)xMargin, (float)yMargin, (float)board.Length, (float)board.Width);

            //draw the board segments
            for (int i = 0; i < segcount; i++)
            {
                OffsetStockItem iseg = segments[i];
                if (!iseg.IsInUse)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Red)), (float)(xMargin + iseg.dLength), (float)(yMargin + iseg.dWidth), (float)iseg.Length, (float)iseg.Width);
            }

            // draw the parts placed
            for (int i = 0; i < partcount; i++)
            {
                Part iPlacement = parts[i];
                double dLength = dLengths[i];
                double dWidth = dWidths[i];

                // draw the part
                g.FillRectangle(Brushes.Green, (float)(xMargin + dLength), (float)(yMargin + dWidth), (float)iPlacement.Length, (float)iPlacement.Width);

                // print the part text
                string partLabel = $"{iPlacement.Name}";
                Font partFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
                g.DrawString(partLabel, partFont, Brushes.Black, (float)(xMargin + dLength), (float)(yMargin + dWidth));
            }
            // draw the board
            //g.DrawRectangle(Pens.Black, (float)xMargin, (float)yMargin, (float)board.Length, (float)board.Width);
            Font aFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
            g.DrawString((partsArea / board.Area * 100).ToString("0.0") + "%", aFont, Brushes.Black, (float)(xMargin), (float)(bitmap.Height - yMargin));

            g.Flush();
            return bitmap;
        }

        private class OffsetStockItem : StockItem
        {
            //public StockItem Board { get; set; }
            public double dLength { get; set; }
            public double dWidth { get; set; }
            public bool IsInUse { get; set; }
            public OffsetStockItem AssociatedBoard { get; set; }
        }



        protected override void PackBoard(Part[] parts, StockItem iBoard, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            int partsCount = parts.Length;
            Part[] orderredParts = parts.OrderBy(t => t.Area).ToArray();

            // init a packer object
            Packer_internal iPacker = new Packer_internal()
            {
                sawkerf = sawkerf,
                Board = iBoard,
                Parts = orderredParts,
                PartsCount = partsCount,
                BoardSections = new OffsetStockItem[2 * partsCount + 2],
                BoardSectionsCount = 1,
                CurrentSolution = new Placement[partsCount]
            };

            iPacker.BoardSections[0] = new OffsetStockItem()
            {
                Name = iBoard.Name,
                Length = iBoard.Length,
                Width = iBoard.Width,
                dLength = 0,
                dWidth = 0
            };

            // pack the board recursively, starting at the first part and an empty solution
            iPacker.StartPacking(0);
        }

        private class Packer_internal
        {
            public StockItem Board;
            public Part[] Parts;
            public int PartsCount;

            public OffsetStockItem[] BoardSections;
            public int BoardSectionsCount;

            public Placement[] CurrentSolution;
            public int CurrentSolutionPartCount;
            public double CurrentSolutionTotalArea;

            public double sawkerf;
            public int iteration = 0;
            public int cn = 0;
            public void StartPacking(int iStart)
            {
                double lastPartLength = -1;
                double lastPartWidth = -1;

                // loop through the parts, from big to small
                for (int i = iStart; i < PartsCount; i++)
                {
                    Part iPart = Parts[i];

                    #region // check if the part is a viable candidate ...
                    // ignore parts already packed
                    if (iPart.IsPacked) continue;
                    // ignore parts already temporarily packed
                    if (CurrentSolution.Any(t=>t?.Part == iPart)) continue;
                    // ignore parts larger than the largest board section
                    if (iPart.Area > Board.Area) break;
                    // short-circuit repeat parts
                    if (iPart.Length == lastPartLength && iPart.Width == lastPartWidth) continue;



                    lastPartLength = iPart.Length;
                    lastPartWidth = iPart.Width;
                    #endregion

                    #region // Find first board that will fit the part ...
                    // find first board that will accomodate the part
                    int j = 0;
                    //while (j < BoardSectionsCount && BoardSections[j].Area < iPart.Area) j++;
                    while (j < BoardSectionsCount && (BoardSections[j].IsInUse || iPart.Length > BoardSections[j].Length || iPart.Width > BoardSections[j].Width)) j++;

                    // if no boards will accomodate the part, continue to next part
                    if (j >= BoardSectionsCount) continue;
                    OffsetStockItem iBoardSection = BoardSections[j];
                    #endregion

                    #region // place the part in the current bin ...
                    CurrentSolution[CurrentSolutionPartCount++] = new Placement()
                    {
                        Part = iPart,
                        dLength = iBoardSection.dLength,
                        dWidth = iBoardSection.dWidth
                    };
                    CurrentSolutionTotalArea += iPart.Area;
                    iBoardSection.IsInUse = true;
                    #endregion

                    #region // store best solution ...
                    //if this is a better solution than the current best one ... replace the current best one
                    if (CurrentSolutionTotalArea > Board.PackedPartsTotalArea)
                    {
                        Board.PackedParts = CurrentSolution.Clone() as Placement[];
                        Board.PackedPartsCount = CurrentSolutionPartCount;
                        Board.PackedPartsTotalArea = CurrentSolutionTotalArea;
                    }
                    #endregion

                    #region // Break association and adjust associated boards if a board is used that is associated with another to prevent overlapping placements ...
                    OffsetStockItem iAssocBoardSection = iBoardSection.AssociatedBoard;
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
                    // create new sections
                    OffsetStockItem boardSection1 = new OffsetStockItem()
                    {
                        Name = iBoardSection.Name,
                        Length = iBoardSection.Length - iPart.Length - sawkerf,
                        Width = iBoardSection.Width,
                        dLength = iBoardSection.dLength + iPart.Length + sawkerf,
                        dWidth = iBoardSection.dWidth
                    };
                    OffsetStockItem boardSection2 = new OffsetStockItem()
                    {
                        Name = iBoardSection.Name,
                        Length = iBoardSection.Length,
                        Width = iBoardSection.Width - iPart.Width - sawkerf,
                        dLength = iBoardSection.dLength,
                        dWidth = iBoardSection.dWidth + iPart.Width + sawkerf
                    };
                    boardSection1.AssociatedBoard = boardSection2;
                    boardSection2.AssociatedBoard = boardSection1;
                    int boardSection1Index = 0;
                    int boardSection2Index = 0;

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
                        boardSection1Index = BoardSectionsCount + 999;
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
                        boardSection2Index = BoardSectionsCount + 999;
                        if (boardSection1 != null) boardSection1.AssociatedBoard = null;
                    }

                    #endregion
#if DEBUG
                    //Drawboard_debug(
                    //    Board,
                    //    BoardSections, BoardSectionsCount,
                    //    CurrentSolution, CurrentSolutionDLengths, CurrentSolutionDWidths, CurrentSolutionPartCount, CurrentSolutionTotalArea).Save($"dbgimages\\{Board.Name}_{cn++}.bmp");
#endif
                    #region // pack the remaining parts on the remaining boards ...
                    // pack the remaining parts on the remaining boards
                    if (i + 1 < PartsCount)
                        StartPacking(i + 1);
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
                    iBoardSection.IsInUse = false;

                    // remove the part from the temporary solution
                    CurrentSolution[--CurrentSolutionPartCount] = null;
                    CurrentSolutionTotalArea -= iPart.Area;

                    #endregion
                }

            }

        }

    }
}
