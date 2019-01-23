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
    class Packer_MAXRECT_LSBF : PackerBase
    {
        new public static string AlgorithmName => "MAXRECT_LSBF";

        protected override void PackBoard(Part[] parts, StockItem iBoard, double sawkerf = 3.2, double partLengthPadding = 0, double partWidthPadding = 0)
        {
            // order parts by length
            Part[] orderredParts = parts.OrderByDescending(o => o.Length).ToArray();
            iBoard.PackedParts = new Placement[orderredParts.Length];

            RectangleF[] F = new RectangleF[500];
            F[0] = new RectangleF(0,0,(float)iBoard.Length,(float)iBoard.Width);
            int F_len = 1;

            for (int i = 0; i < orderredParts.Length; i++)
            {
                var iPart = orderredParts[i];
                RectangleF Fi = F.OrderBy(o=>o.Width).FirstOrDefault(q => q.Width >= iPart.Length && q.Height >= iPart.Width);
                
                if (Fi == RectangleF.Empty) continue;

                iBoard.PackedParts[iBoard.PackedPartsCount++] = new Placement() {
                    dLength = Fi.Left,
                    dWidth = Fi.Top,
                    Part = iPart
                };
                iBoard.PackedPartsTotalArea += iPart.Area;

                RectangleF B = new RectangleF(Fi.Left, Fi.Top, (float)iPart.Length, (float)iPart.Width);

                RectangleF Fp1 = new RectangleF(B.Right+(float)sawkerf,Fi.Top,Fi.Right-B.Right-(float)sawkerf,Fi.Height);
                RectangleF Fp2 = new RectangleF(Fi.Left,B.Bottom+(float)sawkerf,Fi.Width,Fi.Bottom-B.Bottom-(float)sawkerf);
                F[F_len++] = Fp1;
                F[F_len++] = Fp2;
                
                int Fi_index = Array.IndexOf(F, Fi);
                F[Fi_index] = RectangleF.Empty;

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

            }

        }

    }
}
