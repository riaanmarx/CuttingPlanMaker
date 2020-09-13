using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker.Packers
{
    class WeLikeToMoveIt_LengthFirst : PerBoardPackerBase
    {
        public new static string AlgorithmName => "WeLikeToMoveItMoveIt_LengthFirst";

        internal override void PackBoard(Part[] parts, Board board, double sawkerf = 3.2)
        {
            SortedSet<double> tX = new SortedSet<double> { 0 };
            SortedSet<double> tY = new SortedSet<double> { 0 };
            List<Part> placedParts = new List<Part>();
            foreach (var iPart in parts)
            {
                if (iPart.Source != null) continue;

                void FindSpace()
                {
                    foreach (var cY in tY)
                    {
                        if (cY + iPart.Width > board.Width) break;
                        foreach (var cX in tX)
                        {
                            if (cX + iPart.Length > board.Length) break;

                            RectangleF candidateRect = new RectangleF((float)cX, (float)cY, (float)(iPart.Length + sawkerf), (float)(iPart.Width + sawkerf));
                            Part iOvelappedPart = placedParts.FirstOrDefault(t => t.IntersectsWith(candidateRect, sawkerf));
                            if (iOvelappedPart == null)
                            {
                                placedParts.Add(iPart);
                                iPart.Source = board;
                                iPart.OffsetLength = cX;
                                iPart.OffsetWidth = cY;
                                tX.Add(candidateRect.Right);
                                tY.Add(candidateRect.Bottom);
                                return;
                            }
                        }
                    }
                }

                FindSpace();

            }
        }
    }
    class WeLikeToMoveIt_WidthFirst : PerBoardPackerBase
    {
        public new static string AlgorithmName => "WeLikeToMoveItMoveIt_WidthFirst";

        internal override void PackBoard(Part[] parts, Board board, double sawkerf = 3.2)
        {
            SortedSet<double> tX = new SortedSet<double> { 0 };
            SortedSet<double> tY = new SortedSet<double> { 0 };
            List<Part> placedParts = new List<Part>();
            foreach (var iPart in parts.OrderByDescending(t => t.Width).ThenByDescending(u => u.Length))
            {
                if (iPart.Source != null) continue;
                foreach (var cX in tX)
                {
                    if (cX + iPart.Length > board.Length) break;
                    foreach (var cY in tY)
                    {
                        if (cY + iPart.Width > board.Width) break;

                        RectangleF candidateRect = new RectangleF((float)cX, (float)cY, (float)(iPart.Length + sawkerf), (float)(iPart.Width + sawkerf));
                        Part iOvelappedPart = placedParts.FirstOrDefault(t => t.IntersectsWith(candidateRect, sawkerf));
                        if (iOvelappedPart == null)
                        {
                            placedParts.Add(iPart);
                            iPart.Source = board;
                            iPart.OffsetLength = cX;
                            iPart.OffsetWidth = cY;
                            tX.Add(candidateRect.Right);
                            tY.Add(candidateRect.Bottom);
                            goto breakbothloops;
                        }
                    }
                }
                breakbothloops:;
            }
        }
    }
}
