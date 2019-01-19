using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    class experimental : IPacker
    {
        public string Name => "Experimental";
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
            //Array.Resize<Part>(ref parts, 30);

            #region // clear current packing ...
            parts.ToList().ForEach(t => t.IsPacked = false);
            boards.ToList().ForEach(t =>
            {
                t.IsComplete = false;
                t.PackedParts = null;
                t.PackedPartsCount = 0;
                t.PackedPartsTotalArea = 0;
            });
            #endregion

            #region // Prepare variables and sort parts ...
            int boardsCount = boards.Length;
            int packedPartsCount = 0;
            int packedBoardsCount = 0;
            int partsCount = parts.Length;

            // order the parts by Area, Ascending
            Part[] orderredParts = parts.OrderBy(t => t.Area).ToArray(); 
            #endregion

            // loop until no parts were packed or all parts packed or all boards used 
            while (packedPartsCount < partsCount && packedBoardsCount < boardsCount)
            {
                #region // launch a thread per board to pack it ...
                List<Task> threads = new List<Task>();
                for (int i = 0; i < boardsCount; i++)
                    if (!boards[i].IsComplete)
                        threads.Add(Task.Factory.StartNew((o) =>
                        {
                            // reference board[i]
                            StockItem iBoard = boards[(int)o];

                            // for the board at hand, generate the top 10 combinations that will fit the board per area
                            int n = 0;
                            partcombo[] topncombos = new partcombo[5];
                            for (int j = 0; j < topncombos.Length; j++)
                                topncombos[j] = new partcombo()
                                {
                                    parts = null,
                                    totalArea = 0,
                                };

                            var topnCombos = GenerateTopNCombos(orderredParts, iBoard.Area,0, new Part[partsCount],0,0, topncombos);

                        }, i));

                // wait until all boards are packed
                Task.WaitAll(threads.ToArray()); 
                #endregion

                #region // Find the best packed board from this iteration ...
                StockItem[] incompleteBoards = boards.Where(q => !q.IsComplete).ToArray();
                StockItem[] newlypackedBoards = incompleteBoards.Where(q => q.PackedPartsCount > 0).ToArray();
                StockItem iBestCoverredBoard = newlypackedBoards.OrderByDescending(t => t.PackedPartsTotalArea / t.Area)
                    .FirstOrDefault();

                if (iBestCoverredBoard == null) return;
                #endregion

                #region // Keep the solution for the best packed board ...
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
                #endregion
            }

        }


        UInt64 cntr = 0;

        class partcombo
        {
            public Part[] parts;
            public double totalArea;
        }
        private partcombo[] GenerateTopNCombos(Part[] parts, double maximumArea, int starti, Part[] currentcombo, int currentlen, double currentarea, partcombo[] topncombos)
        {
            double prevarea = -1;
            for (int i = starti; i < parts.Length; i++)
            {
                if (parts[i].Area == prevarea) continue;
                prevarea = parts[i].Area;

                double newarea = currentarea + parts[i].Area;

                if (newarea >= maximumArea) break;

                currentcombo[currentlen] = parts[i];
                cntr++;
                //if (currentlen == 6)
                //{
                //    Part[] t = new Part[7];
                //    Array.Copy(currentcombo, t, 7);
                //    if (t.ToList().Any(q => q.Name == "001")
                //    && t.ToList().Exists(q => q.Name == "003")
                //    && t.ToList().Exists(q => q.Name == "015")
                //    && t.ToList().Exists(q => q.Name == "016")
                //    && t.ToList().Exists(q => q.Name == "028")
                //    && t.ToList().Exists(q => q.Name == "029")
                //    && t.ToList().Exists(q => q.Name == "038")
                //    )
                //        Debugger.Break();
                //}

                //var orderredtopn = topncombos.OrderBy(o => o.totalArea);
                //var smallest = orderredtopn.First();
                //if (smallest.totalArea < newarea)
                //{
                //    smallest.totalArea = newarea;
                //    smallest.parts = new Part[currentlen + 1];
                //    Array.Copy(currentcombo, smallest.parts, currentlen + 1);
                //}


                GenerateTopNCombos(parts, maximumArea, i + 1, currentcombo, currentlen + 1, newarea, topncombos);

                currentcombo[currentlen + 1] = null;
            }
            return topncombos;
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
