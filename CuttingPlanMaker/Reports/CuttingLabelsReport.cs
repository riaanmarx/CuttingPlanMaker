using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// A PDF file generating class to generate labels/stickers for the parts
    /// </summary>
    class CuttingLabelReport : ReportBase
    {
        /// <summary>
        /// internal class to host a base64 image
        /// </summary>
        private class Base64Image
        {
            /// <summary>
            /// The base64 string containing the image
            /// </summary>
            public string image;

            /// <summary>
            /// The height of the image contained in the base64 string
            /// </summary>
            public int Height;

            /// <summary>
            /// The width of the image contained in the base64 string
            /// </summary>
            public int Width;
        }

        /// <summary>
        /// Draw a board to an image and return the image as a base64 string
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private Base64Image DrawBoard_base64(Board board, IEnumerable<Part> parts)
        {
            // constants used in drawing the image
            const double xMargin = 5;
            const double yMargin = 20;
            double imageHeight = board.Width + 2 * yMargin;
            double imageWidth = board.Length + 2 * xMargin;

            // create bitmap
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)imageWidth, (int)imageHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            // fill the background
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, (int)imageWidth, (int)imageHeight);

            // draw the board
            //g.FillRectangle(System.Drawing.Brushes.DarkRed, (float)(xMargin), (float)yMargin, (float)board.Length, (float)board.Width);
            g.DrawRectangle(System.Drawing.Pens.Black, (float)(xMargin), (float)yMargin, (float)board.Length, (float)board.Width);

            // loop through all the parts and draw the ones on the current board
            int partnum = 0;
            foreach (var iPart in parts)
            {
                partnum++;
                // draw the part
                //if (iPart.LongName == "defect")
                //    g.FillRectangle(System.Drawing.Brushes.LightGray,
                //        (float)(xMargin + iPart.OffsetLength),
                //        (float)(yMargin + iPart.OffsetWidth),
                //        (float)iPart.Length,
                //        (float)iPart.Width);
                //else
                //    g.FillRectangle(System.Drawing.Brushes.LightGreen,
                //        (float)(xMargin + iPart.OffsetLength),
                //        (float)(yMargin + iPart.OffsetWidth),
                //        (float)iPart.Length,
                //        (float)iPart.Width);
                g.DrawRectangle(System.Drawing.Pens.Black,
                    (float)(xMargin + iPart.OffsetLength),
                    (float)(yMargin + iPart.OffsetWidth),
                    (float)iPart.Length,
                    (float)iPart.Width);
                // print the part text
                string text1 = $"{iPart.Name}";
                
                System.Drawing.Font partFont = new System.Drawing.Font(new System.Drawing.FontFamily("Consolas"), 15);
                System.Drawing.SizeF textSize = g.MeasureString(text1, partFont);
                textSize = g.MeasureString(text1, partFont);
                g.DrawString(text1, partFont, System.Drawing.Brushes.Black,
                    (int)(xMargin + iPart.OffsetLength + iPart.Length / 2.0 - textSize.Width / 2.0),
                    (int)(yMargin + iPart.OffsetWidth + iPart.Width / 2.0 - textSize.Height / 2.0));
            }

            // make sure the cache is empty
            g.Flush();

            // convert image to base64 image
            System.IO.MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byteImage = ms.ToArray();
            var base64img = Convert.ToBase64String(byteImage);

            // return the class for the image
            return new Base64Image()
            {
                image = base64img,
                Height = bitmap.Height,
                Width = bitmap.Width
            };
        }




        /// <summary>
        /// Generate the PDF report
        /// </summary>
        /// <param name="Settings"></param>
        /// <param name="Materials"></param>
        /// <param name="Stock"></param>
        /// <param name="Parts"></param>
        /// <returns></returns>
        public PdfSharp.Pdf.PdfDocument Generate(Settings Settings, BindingList<Material> Materials, BindingList<Board> Stock, BindingList<Part> Parts)
        {
            #region // Configuration settings ...
            int colCount = 5;                       // the number of labels per row
            int rowCount = 10;                      // the number of rows of labels per page
            LeftMargin = Unit.FromMillimeter(10);   // the left margin or lefte most edge of the left most label
            TopMargin = Unit.FromMillimeter(10);    // the top edge of the top most label
            RightMargin = Unit.FromMillimeter(10);  // the right edge of the right most label
            BottomMargin = Unit.FromMillimeter(10); // the bottom edge of the bottom most lable
            #endregion

            #region // clear header and section configurations - this report does not use the title block ...
            document.Sections.Clear();
            mainSection = document.AddSection();

            // set up the new section
            mainSection.PageSetup.PageHeight = document.DefaultPageSetup.PageHeight;
            mainSection.PageSetup.PageWidth = document.DefaultPageSetup.PageWidth;
            mainSection.PageSetup.LeftMargin = LeftMargin;
            mainSection.PageSetup.RightMargin = RightMargin;
            mainSection.PageSetup.TopMargin = TopMargin;
            mainSection.PageSetup.BottomMargin = BottomMargin;
            #endregion

            #region // write content into document ...
            // Create a table spanning whole page
            Table table = mainSection.AddTable();
            table.Borders.Color = Colors.WhiteSmoke;
            table.Borders.Width = Unit.FromMillimeter(0.1);
            Unit colWidth = (mainSection.PageSetup.PageWidth - LeftMargin - RightMargin)/ colCount;
            Unit rowHeight = (mainSection.PageSetup.PageHeight - TopMargin - BottomMargin ) / rowCount - table.Borders.Width * 2;
            for (int i = 0; i < colCount; i++)
                table.AddColumn(colWidth);
            //loop through all the stock items
            foreach (var iStock in Stock)
            {
                // filter the parts for the current stock
                var iParts = Parts.Where(p => p.Source == iStock);
                // if the stock has no parts assigned to it, skip the stock
                if (iParts.Count() == 0) continue;

                // add the board/stock title row
                var stocktitlerow=table.AddRow();
                var titlecell = stocktitlerow.Cells[0];
                titlecell.MergeRight = colCount - 1;
                titlecell.Format.Font.Bold = true;
                titlecell.Format.Font.Size = 12;
                titlecell.AddParagraph($"Board: {iStock.Name} [{iStock.Length} x {iStock.Width}]");
                titlecell.Row.TopPadding = 10;

                // draw the board loayout image
                var bitmap = DrawBoard_base64(iStock, iParts);
                var img = titlecell.AddImage("base64:" + bitmap.image);
                img.LockAspectRatio = true;

                double maximgwidth = mainSection.PageSetup.PageWidth - LeftMargin - RightMargin - Unit.FromCentimeter(1);
                double maximgheight = Unit.FromCentimeter(2.5);
                double xscale = bitmap.Width / maximgwidth;
                double yscale = bitmap.Height / maximgheight;

                if (xscale > yscale)
                    img.Width = mainSection.PageSetup.PageWidth - LeftMargin - RightMargin - Unit.FromCentimeter(1);
                else
                    img.Height = maximgheight;

                // loop throug all the parts placed on the stock item (if any)
                int cntr = 0;
                Row iRow = null;
                int rowcntr = 0;
                foreach (var iPart in iParts)
                {
                    if (cntr % colCount == 0)
                    {
                        rowcntr++;
                        iRow = table.AddRow();
                    }
                    //determine the column and row for the part record in the table

                   var clabel = iRow[cntr % colCount];
                    clabel.Borders.Bottom.Color = Colors.Black;
                    clabel.Borders.Bottom.Width = 1;
                    clabel.Borders.Top.Color = Colors.Black;
                    clabel.Borders.Top.Width = 1;
                    clabel.Borders.Left.Color = Colors.Black;
                    clabel.Borders.Left.Width = 1;
                    clabel.Borders.Right.Color = Colors.Black;
                    clabel.Borders.Right.Width = 1;

                    //create a nested table to organise the part info on the label
                    Table labelTable = new Table();
                    clabel.Elements.Add(labelTable);
                    // add the nested table to the table managing the labels
                    labelTable.AddColumn(colWidth);
                    labelTable.AddRow();
                    labelTable.AddRow();
                    //labelTable.AddRow();

                    Cell c = labelTable[0, 0];  // top row := part's name
                    c.Format.Font.Bold = true;
                    c.Format.Font.Size = 15;
                    c.AddParagraph(iPart.Name);

                    c = labelTable[1, 0];       // Second row := dimensions
                    c.Format.Font.Size = 12;
                    c.AddParagraph($"[{iPart.Length:0.0} x {iPart.Width:0.0}]");

                    c = labelTable[1, 0];       // Third row := location
                    c.Format.Font.Size = 8;
                    c.AddParagraph($"{iPart.Source.Name} {{{iPart.OffsetLength:0.0} ; {iPart.OffsetWidth:0.0}}}");

                    cntr++;
                }

                titlecell.Row.KeepWith = rowcntr;
            }

            #endregion

            return RenderPdf();
        }
    }
}
