//#define drawdbgimages
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    class MAXRECT_DECPERIM : MAXRECT_DESCL
    {
        new public static string AlgorithmName => "MAXRECT_DESCPERIM";
        public MAXRECT_DECPERIM()
        {
            partsorder = "DESPERIM";
        }
    }
    class MAXRECT_DESCA : MAXRECT_DESCL
    {
        new public static string AlgorithmName => "MAXRECT_DESCA";
        public MAXRECT_DESCA()
        {
            partsorder = "DESCA";
        }
    }

    class MAXRECT_DESCW : MAXRECT_DESCL
    {
        new public static string AlgorithmName => "MAXRECT_DESCW";
        public MAXRECT_DESCW()
        {
            partsorder = "DESCW";
        }
    }

    /// <summary>
    /// </summary>
    class MAXRECT_DESCL : PerBoardPackerBase
    {
        new public static string AlgorithmName => "MAXRECT_DESCL";

#if true//drawdbgimages
        private Bitmap Drawboard_debug(Board board, RectangleF[] freerects, Part[] parts)
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
            for (int i = 0; i < parts.Length; i++)
            {
                var iPart = parts[i];
                if (iPart.Source == null) continue;

                // draw the part
                g.FillRectangle(Brushes.Green, (float)(xMargin + iPart.OffsetLength), (float)(yMargin + iPart.OffsetWidth), (float)iPart.Length, (float)iPart.Width);

                // print the part text
                //string partLabel = $"{iPart}";
                //Font partFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
                //g.DrawString(partLabel, partFont, Brushes.Black, (float)(xMargin + iPlacement.dLength), (float)(yMargin + iPlacement.dWidth));
            }

            //draw the board segments
            for (int i = 0; i < freerects.Length; i++)
            {
                Rectangle t = Rectangle.Round(freerects[i]);
                t.Offset((int)xMargin, (int)yMargin);
                //g.DrawRectangle(Pens.Red, t);
                g.FillRectangle(new SolidBrush(Color.FromArgb(200,Color.Red)), t);
            }

            
            // draw the board
            //g.DrawRectangle(Pens.Black, (float)xMargin, (float)yMargin, (float)board.Length, (float)board.Width);
            //Font aFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
            //g.DrawString((board.PackingCoverage).ToString("0.00%"), aFont, Brushes.Black, (float)(xMargin), (float)(bitmap.Height - yMargin));

            g.Flush();
            return bitmap;
        }
#endif
        protected string partsorder = "DESCL";

        internal override void PackBoard(Part[] parts, Board iBoard, double sawkerf = 3.2)
        {
            Part[] orderredParts;
            // order parts by length
            switch (partsorder)
            {
                case "DESCL":
                    orderredParts = parts.OrderByDescending(o => o.Length).ToArray();
                    break;
                case "DESCA":
                    orderredParts = parts.OrderByDescending(o => o.Area).ToArray();
                    break;
                case "DESCW":
                    orderredParts = parts.OrderByDescending(o => o.Width).ToArray();
                    break;
                case "DESPERIM":
                    orderredParts = parts.OrderByDescending(o => o.Width + o.Length).ToArray();
                    break;
                default:
                    orderredParts = parts;
                    break;
            }
            if (parts.Length == 0) return;
            RectangleF[] F = new RectangleF[7 * parts.Length];
            F[0] = new RectangleF(0, 0, (float)(iBoard.Length+sawkerf), (float)(iBoard.Width+sawkerf));
            int F_len = 1;

            var packingId = Guid.NewGuid();

            for (int i = 0; i < orderredParts.Length; i++)
            {
                var iPart = orderredParts[i];
                RectangleF Fi = F.OrderBy(o => o.Width * o.Height).FirstOrDefault(q => q.Width >= iPart.Length+sawkerf && q.Height >= iPart.Width+sawkerf);

                if (Fi == RectangleF.Empty) continue; // if the current rect has been removed, continue to the next rect

                iPart.Source = iBoard;
                iPart.OffsetLength = Fi.Left;
                iPart.OffsetWidth = Fi.Top;

                RectangleF B = new RectangleF(Fi.Left, Fi.Top, (float)iPart.Length + (float)sawkerf, (float)iPart.Width + (float)sawkerf);

                for (int findex = 0; findex < F_len; findex++)
                {
                    Fi = F[findex];
                    if (Fi.IntersectsWith(B))
                    {
                        //compute Fi \ B, subdivided into rectangles G1..G4
                        if (B.Right < Fi.Right) // if the part's right edge is to the left of the free rect's right edge
                            F[F_len++] = new RectangleF(B.Right, Fi.Top, Fi.Right - B.Right, Fi.Height); //add a free rect the full height of the free rect, right of the part

                        if (B.Left > Fi.Left) // if the part's left edge is to the right of the free rect's left edge
                            F[F_len++] = new RectangleF(Fi.Left, Fi.Top, B.Left - Fi.Left, Fi.Height); // add a free rect the full height of the free rect, left of the part

                        if (B.Top > Fi.Top) // if the part's top is lower than the free rect's top
                            F[F_len++] = new RectangleF(Fi.Left, Fi.Top, Fi.Width, B.Top - Fi.Top); // add a free rect the full width of the free rect, above the part

                        if (B.Bottom < Fi.Bottom) // if the part's bottom is higher than the free rect's bottom
                            F[F_len++] = new RectangleF(Fi.Left, B.Bottom, Fi.Width, Fi.Bottom - B.Bottom); // add a free rect the full width of the free rect, below the part

                        F[findex] = RectangleF.Empty; // remove the original free rect
                    }
                }

                //remove free rects fully included in other free rects
                for (int j = 0; j < F.Length; j++)
                {
                    if (F[j] == Rectangle.Empty) continue;
                    for (int k = 0; k < F.Length; k++)
                    {
                        if (F[k] == Rectangle.Empty || k==j) continue;
                        if (F[j].Contains(F[k]))
                            F[k] = Rectangle.Empty;
                    }
                }

                //if this is the board we are interested in
                //  draw the board, free rectangles and the parts placed
                //var bmp = Drawboard_debug(iBoard, F, parts);
                //bmp.Save($"out_{i}.bmp");

#if drawdbgimages
                Drawboard_debug(iBoard, F, F_len).Save($"{iBoard.Name}_{i}.bmp");  
#endif
            }


        }

        private bool ContainedIn(RectangleF outerR, RectangleF inner)
        {
            if (inner.Left < outerR.Left) return false;
            if (inner.Right > outerR.Right) return false;
            if (inner.Top < outerR.Top) return false;
            if (inner.Bottom > outerR.Bottom) return false;

            return true;
        }
    }
}
