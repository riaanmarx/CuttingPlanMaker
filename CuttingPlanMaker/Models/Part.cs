using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// Model class for Part objects
    /// </summary>
    public class Part
    {
        /// <summary>
        /// A name for the part
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The material from which this part is to be made
        /// </summary>
        public string Material { get; set; }

        /// <summary>
        /// Length of the part (dimension parallel with grain)
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Width of the part (dimension accross the grain)
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The surface area of the part (length * width)
        /// </summary>
        public double Area => Length * Width;

        /// <summary>
        /// Flag to indicate if this part has been placed on a board already
        /// </summary>
        public bool IsFrozen { get; set; }

        public Board Source { get; set; }
        public double OffsetLength { get; set; }
        public double OffsetWidth { get; set; }

        /// <summary>
        /// string repressentation of the part
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} [{Length,7:0.0} x {Width,5:0.0}]";
        }

        /// <summary>
        /// inflates the part's size with the specified amounts
        /// </summary>
        /// <param name="deltaWidth"></param>
        /// <param name="deltaLength"></param>
        public void Inflate(double deltaWidth, double deltaLength)
        {
            Width += 2 * deltaWidth;
            Length += 2 * deltaLength;
        }

        public Part Clone()
        {
            return new Part
            {
                Name = this.Name,
                Material = this.Material,
                Length = this.Length,
                Width = this.Width,
                IsFrozen = this.IsFrozen,
                Source = this.Source,
                OffsetLength = this.OffsetLength,
                OffsetWidth = this.OffsetWidth
            };
        }

        //todo: convert to doubles...
        public bool IntersectsWith(RectangleF rectangleF, double sawkerf)
        {
            RectangleF partRect = new RectangleF((float)OffsetLength, (float)OffsetWidth, (float)(Length + sawkerf), (float)(Width + sawkerf));
            return partRect.IntersectsWith(rectangleF);
        }
        public bool IntersectsWith(double offset_l, double offset_w, double length, double width)
        {
            RectangleF partRect = new RectangleF((float)OffsetLength, (float)OffsetWidth, (float)Length, (float)Width);
            RectangleF testRect = new RectangleF((float)offset_l, (float)offset_w, (float)length, (float)width);

            return partRect.IntersectsWith(testRect);
        }
        //todo: convert to doubles...
        public bool Contains(PointF PointF)
        {
            RectangleF partRect = new RectangleF((float)OffsetLength, (float)OffsetWidth, (float)(Length), (float)(Width));
            return partRect.Contains(PointF);
        }

        //public bool Contains(double Offset_l, double Offset_w)
        //{
        //    if (Offset_l < OffsetLength) return false;
        //    if (Offset_l >= OffsetLength + Length) return false;
        //    if (Offset_w < OffsetWidth) return false;
        //    if (Offset_w >= OffsetWidth + Width) return false;
        //    return true;
        //}
    }
}
