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
    class MAXRECT_DESCA: MAXRECT_DESCL
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
    class MAXRECT_DESCL : PackerBase
    {
        new public static string AlgorithmName => "MAXRECT_DESCL";

#if drawdbgimages
        private Bitmap Drawboard_debug(StockItem board, RectangleF[] freerects, int len)
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
            for (int i = 0; i < len; i++)
            {
                Rectangle t = Rectangle.Round(freerects[i]);
                t.Offset((int)xMargin, (int)yMargin);
                //g.DrawRectangle(Pens.Red, t);
                g.FillRectangle(new SolidBrush(Color.FromArgb(200,Color.Red)), t);

            }

            // draw the parts placed
            for (int i = 0; i < board.PackedPartsCount; i++)
            {
                Placement iPlacement = board.PackedParts[i];

                // draw the part
                g.FillRectangle(Brushes.Green, (float)(xMargin + iPlacement.dLength), (float)(yMargin + iPlacement.dWidth), (float)iPlacement.Part.Length, (float)iPlacement.Part.Width);

                // print the part text
                string partLabel = $"{iPlacement.Part.Name}";
                Font partFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
                g.DrawString(partLabel, partFont, Brushes.Black, (float)(xMargin + iPlacement.dLength), (float)(yMargin + iPlacement.dWidth));
            }
            // draw the board
            //g.DrawRectangle(Pens.Black, (float)xMargin, (float)yMargin, (float)board.Length, (float)board.Width);
            Font aFont = new Font(new FontFamily("Microsoft Sans Serif"), 10);
            g.DrawString((board.PackingCoverage).ToString("0.00%"), aFont, Brushes.Black, (float)(xMargin), (float)(bitmap.Height - yMargin));

            g.Flush();
            return bitmap;
        }
#endif
        protected string partsorder = "DESCL";

        protected override void PackBoard(Part[] parts, StockItem iBoard, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
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
             
            iBoard.PackedParts = new Placement[orderredParts.Length];

            RectangleF[] F = new RectangleF[5 * parts.Length];
            F[0] = new RectangleF(0,0,(float)iBoard.Length,(float)iBoard.Width);
            int F_len = 1;

            for (int i = 0; i < orderredParts.Length; i++)
            {
                var iPart = orderredParts[i];
                RectangleF Fi = F.OrderBy(o=>o.Width*o.Height).FirstOrDefault(q => q.Width >= iPart.Length && q.Height >= iPart.Width);
                
                if (Fi == RectangleF.Empty) continue;

                iBoard.PackedParts[iBoard.PackedPartsCount++] = new Placement() {
                    dLength = Fi.Left,
                    dWidth = Fi.Top,
                    Part = iPart
                };

                RectangleF B = new RectangleF(Fi.Left, Fi.Top, (float)iPart.Length, (float)iPart.Width);

                //F[F_len++] = new RectangleF(B.Right + (float)sawkerf, Fi.Top, Fi.Right - B.Right - (float)sawkerf, Fi.Height);
                //F[F_len++] = new RectangleF(Fi.Left, B.Bottom + (float)sawkerf, Fi.Width, Fi.Bottom - B.Bottom - (float)sawkerf);

                //int Fi_index = Array.IndexOf(F, Fi);
                //F[Fi_index] = RectangleF.Empty;

                for (int findex = 0; findex < F_len; findex++)
                {
                    Fi = F[findex];
                    if (Fi.IntersectsWith(B))
                    {
                        //compute Fi \ B, subdivided into rectangles G1..G4
                        if (B.Right < Fi.Right)
                            F[F_len++] = new RectangleF(B.Right+(float)sawkerf,Fi.Top,Fi.Right-B.Right -(float)sawkerf,Fi.Height);
                        if (B.Left > Fi.Left)
                            F[F_len++] = new RectangleF(Fi.Left, Fi.Top, B.Left - Fi.Left - (float)sawkerf, Fi.Height);
                        if (B.Top > Fi.Top)
                            F[F_len++] = new RectangleF(Fi.Left, Fi.Top, Fi.Width, B.Top - Fi.Top - (float)sawkerf);
                        if (B.Bottom < Fi.Bottom)
                            F[F_len++] = new RectangleF(Fi.Left, B.Bottom + (float)sawkerf, Fi.Width, Fi.Bottom-B.Bottom - (float)sawkerf);

                        F[findex] = RectangleF.Empty;

                    }
                }

                // order by Left,Top ascending
                RectangleF[] Forderred = F.Where(q => q != RectangleF.Empty).OrderBy(o => o.Left * iBoard.Width + o.Top).ToArray();
                for (int j = 0; j < Forderred.Length - 1; j++)
                {
                    int k = j + 1;
                    while (k < Forderred.Length && ContainedIn(Forderred[j], Forderred[k]))
                    {
                        int index = Array.IndexOf(F, Forderred[k]);
                        if(index>=0) F[index] = RectangleF.Empty;
                        k++;
                    }
                }

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
