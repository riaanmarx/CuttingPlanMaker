//#define drawdbgimages

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    /// <summary>
    /// An algorithm to pack parts into boards using diagonal points generated per board and per placement
    /// </summary>
    /// <remarks>
    /// Algorithm:
    /// This packer uses a collection of points as a means to place the parts
    /// Every board is packed using all the available parts, the best packed board and the parts used is then removed from the usable lists and the process is repeated using the new subset of boards and parts.
    /// To pack a board, a collection of points is generated, each with an X;Y coordinate and a disabled flag indicating if a part can be placed on the point.
    /// Initially the collection contains only the diagonal corners of the board (0,0) and (length,width)
    /// The (length,width) coord is disabled.
    /// We repeatedly loop through all the enabled points
    ///     check what is the area available at that point
    ///     place the largest part that would fit the area at the point
    ///     and insert two new points at the top right and bottom left corners of the part
    ///     the point at which the part is placed is disabled.
    /// Some other special scenarios is also coverred
    /// When all the points are disabled, the board is complete
    /// PROBLEM: It is a greedy algorithm. it will not attempt to use two smaller parts with higher total area, instead of one part with bigger area than one of them.
    /// </remarks>
    class Packer_points : IPacker
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name => "Diagonal Points";

        /// <summary>
        /// An internal class for the points used to manage placements
        /// </summary>
        private class PointD
        {
            public double dWidth;
            public double dLength;
            public bool disabled;

            public PointD(double dwidth, double dlength)
            { this.dWidth = dwidth; this.dLength = dlength; }

            public override string ToString()
            {
                return $"{(disabled ? "!" : "")}{dLength},{dWidth}";
            }
        }

        /// <summary>
        /// Pack the parts on the boards
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="boards"></param>
        /// <param name="sawkerf"></param>
        /// <param name="partLengthPadding"></param>
        /// <param name="partWidthPadding"></param>
        public void Pack(Part[] parts, StockItem[] boards, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            //clear current packing
            parts.ToList().ForEach(t => t.IsPacked = false);
            boards.ToList().ForEach(t =>
            {
                t.IsComplete = false;
                t.PackedParts = null;
                t.PackedPartsCount = 0;
                t.PackedPartsTotalArea = 0;
            });


            // order the parts by Area, Ascending
            int boardsCount = boards.Length;
            int packedPartsCount = 0;
            int packedBoardsCount = 0;
            int partsCount = parts.Length;
            Part[] orderredParts = parts.OrderBy(t => t.Area).ToArray();

            // loop until no parts were packed or all parts packed or all boards used 
            while (packedPartsCount < partsCount && packedBoardsCount < boardsCount)
            {
                // launch a thread per board to pack it
                List<Task> threads = new List<Task>();
                for (int i = 0; i < boardsCount; i++)
                    if (!boards[i].IsComplete)
                        threads.Add(Task.Factory.StartNew((o) =>
                           {
                               // reference board[i]
                               StockItem iBoard = boards[(int)o];

                               // clear the solution for the board
                               iBoard.PackedParts = new Placement[partsCount];
                               iBoard.PackedPartsCount = 0;
                               iBoard.PackedPartsTotalArea = 0;
                               iBoard.IsComplete = false;

                               // create the two original points for the board
                               PointD[] points = new PointD[partsCount * 2 + 2];
                               points[0] = new PointD(0, 0);
                               points[1] = new PointD(iBoard.Width + sawkerf, iBoard.Length + sawkerf) { disabled = true };
                               int pointCount = 2;
                               int iPointIndex = -1;
#if drawdbgimages
                               int cntr = 0;
#endif
                               // continuously iterate throught the points until we reach the end of the list of points (restart from first point if a part is placed)
                               while (++iPointIndex < pointCount)
                               {
                                   PointD iPoint = points[iPointIndex];
                                   if (iPoint.disabled) continue; // ignore disabled points

                                   #region // determine hight and width of available area at the point...
                                   IEnumerable<PointD> limitinpoints = points.Where(q => q?.dLength > iPoint.dLength && q?.dWidth >= iPoint.dWidth);


                                   PointD limitingPoint = points[pointCount - 1];
                                   if (limitinpoints.Count() > 0) limitingPoint = limitinpoints.OrderBy(so => so.dWidth).First();
                                   double maxx = limitingPoint.dWidth;
                                   double maxy = iBoard.Length;

                                   double maxWidth = maxx - sawkerf - iPoint.dWidth;
                                   double maxLength = maxy - iPoint.dLength;// - sawkerf;
                                   if (maxWidth <= 0 || maxLength <= 0)
                                   {
                                       iPoint.disabled = true;
                                       continue;
                                   }
                                   #endregion

                                   #region // search for a part that will fit on the area on the board ...
                                   // test each part for fit on the area for the point
                                   bool partplaced = false;
                                   for (int iPartIndex = partsCount - 1; iPartIndex >= 0; iPartIndex--)
                                   {
                                       Part iPart = orderredParts[iPartIndex];
                                       // ignore parts already packed
                                       if (iPart.IsPacked || iBoard.PackedParts.Any(t => t?.Part == iPart)) continue;

                                       // if the part will fit
                                       if (iPart.Length + partLengthPadding <= maxLength && iPart.Width + partWidthPadding <= maxWidth)
                                       {
                                           #region // place the part onto the board at the point ...
                                           iBoard.PackedParts[iBoard.PackedPartsCount] = new Placement()
                                           {
                                               Part = iPart,
                                               dLength = iPoint.dLength,
                                               dWidth = iPoint.dWidth
                                           };
                                           iBoard.PackedPartsTotalArea += iPart.Area;
                                           iBoard.PackedPartsCount++;
                                           #endregion

                                           #region // create new points for the top-right and bottom left corners of the part ...
                                           PointD newBL = new PointD(iPoint.dWidth, iPoint.dLength + iPart.Length + sawkerf + partLengthPadding);
                                           PointD newTR = new PointD(iPoint.dWidth + iPart.Width + sawkerf + partWidthPadding, iPoint.dLength);
                                           #endregion

                                           #region // disable the existing points coverred by the new part
                                           for (int j = 0; j < pointCount; j++)
                                           {
                                               PointD jPoint = points[j];
                                               if (jPoint.dWidth >= iPoint.dWidth && jPoint.dWidth <= iPoint.dWidth + iPart.Width && jPoint.dLength >= iPoint.dLength) jPoint.disabled = true;
                                           }
                                           #endregion

                                           #region // insert new points into orderred array ...
                                           int di = pointCount - 1;
                                           if (newBL != null)
                                           {
                                               while (points[di].dLength > newBL.dLength || points[di].dLength == newBL.dLength && points[di].dWidth > newBL.dWidth)
                                                   points[1 + di] = points[di--];
                                               points[1 + di] = newBL;
                                               pointCount++;
                                           }

                                           if (newTR != null)
                                           {
                                               di = pointCount - 1;
                                               while (points[di].dLength > newTR.dLength || points[di].dLength == newTR.dLength && points[di].dWidth > newTR.dWidth)
                                                   points[1 + di] = points[di--];
                                               points[1 + di] = newTR;
                                               pointCount++;
                                           }
                                           #endregion
#if drawdbgimages
                                           Drawboard_debug(
                                               iBoard,
                                               points, pointCount,
                                               iBoard.PackedParts, iBoard.PackedPartsCount,
                                               iBoard.PackedPartsTotalArea,
                                               new RectangleF((float)iPoint.dLength, (float)iPoint.dWidth, (float)maxLength, (float)maxWidth)).Save($"dbgimages\\{iBoard.Name}_{cntr++}.bmp");
#endif
                                           partplaced = true;
                                           break;
                                       }
                                   }
                                   #endregion

                                   // if no parts fit this point's area
                                   if (!partplaced)
                                   {
                                       // disable this point
                                       iPoint.disabled = true;

                                       // if this part's area was not limited by the board's edge
                                       if (limitingPoint.dLength < iBoard.Length + sawkerf)
                                       {
                                           // create a new point at the same dLength value as the one that limited the width - maybe the extra width will allow a part to be placed there
                                           PointD newPoint = new PointD(iPoint.dWidth, limitingPoint.dLength);
                                           // insert the new point into the orderred array
                                           int di = pointCount - 1;
                                           while (points[di].dLength > newPoint.dLength || points[di].dLength == newPoint.dLength && points[di].dWidth > newPoint.dWidth)
                                               points[1 + di] = points[di--];

                                           points[1 + di] = newPoint;
                                           pointCount++;
                                       }
                                   }
                                   else
                                       // we placed a part - traverse all the points again
                                       iPointIndex = -1;
                               }
                           }, i));

                // wait until all boards are packed
                Task.WaitAll(threads.ToArray());

                // Find the best packed board from this iteration
                StockItem[] incompleteBoards = boards.Where(q => !q.IsComplete).ToArray();
                StockItem[] newlypackedBoards = incompleteBoards.Where(q => q.PackedPartsCount > 0).ToArray();
                StockItem iBestCoverredBoard = newlypackedBoards.OrderByDescending(t => t.PackedPartsTotalArea / t.Area)
                    .FirstOrDefault();

                if (iBestCoverredBoard == null) return;

                // set the complete flag for the board with the best coverage
                iBestCoverredBoard.IsComplete = true;
                packedBoardsCount++;

                //Compact the packed parts array of the board
                Array.Resize<Placement>(ref iBestCoverredBoard.PackedParts, iBestCoverredBoard.PackedPartsCount);

                // set the packed flag for the packed parts
                iBestCoverredBoard.PackedParts.ToList().ForEach(t => t.Part.IsPacked = true);
                packedPartsCount += iBestCoverredBoard.PackedPartsCount;


                // clear the packing for all the unsuccessfull boards
                for (int iPacked = 0; iPacked < boardsCount; iPacked++)
                {
                    StockItem iBoard = boards[iPacked];
                    // if non of te parts packed on the board have been packed on a previous board
                    if (!iBoard.IsComplete)
                    {
                        iBoard.PackedParts = null;
                        iBoard.PackedPartsCount = 0;
                        iBoard.PackedPartsTotalArea = 0;
                        iBoard.IsComplete = false;
                    }
                }

            }

        }

        private static Bitmap Drawboard_debug(StockItem board, PointD[] points, int pointcount, Placement[] parts, int placementcount, double partsArea, RectangleF lastarea)
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



            // draw the parts placed
            for (int i = 0; i < placementcount; i++)
            {
                Placement iPlacement = parts[i];

                // draw the part
                g.FillRectangle(Brushes.Green, (float)(xMargin + iPlacement.dLength), (float)(yMargin + iPlacement.dWidth), (float)iPlacement.Part.Length, (float)iPlacement.Part.Width);

                // print the part text
                string partLabel = $"{iPlacement.Part.Name}";
                Font partFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
                g.DrawString(partLabel, partFont, Brushes.Black, (float)(xMargin + iPlacement.dLength), (float)(yMargin + iPlacement.dWidth));
            }

            // draw the last area where a part was placed
            if (lastarea != RectangleF.Empty)
            {
                lastarea.Offset((float)xMargin, (float)yMargin);
                g.FillRectangle(new SolidBrush(Color.FromArgb(140, Color.Yellow)), Rectangle.Round(lastarea));
            }

            //draw the placement points
            for (int i = 0; i < pointcount; i++)
            {
                PointD iPoint = points[i];
                if (iPoint.disabled)
                    g.FillEllipse(new SolidBrush(Color.FromArgb(220, Color.Black)), (float)(xMargin + iPoint.dLength - 10), (float)(yMargin + iPoint.dWidth - 10), 20, 20);
                else
                    g.FillEllipse(new SolidBrush(Color.FromArgb(220, Color.Red)), (float)(xMargin + iPoint.dLength - 10), (float)(yMargin + iPoint.dWidth - 10), 20, 20);
            }

            Font aFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
            g.DrawString((partsArea / board.Area * 100).ToString("0.0") + "%", aFont, Brushes.Black, (float)(xMargin), (float)(bitmap.Height - yMargin + 15));


            g.Flush();
            return bitmap;
        }

    }
}
