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
    /// PDF file generating report class for the layout report
    /// </summary>
    class LayoutReport : ReportBase
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
            const double xMargin = 0;
            const double yMargin = 20;
            double imageHeight = board.Width + 2 * yMargin;
            double imageWidth = board.Length + 2 * xMargin;

            // create bitmap
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)imageWidth, (int)imageHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            // fill the background
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, (int)imageWidth, (int)imageHeight);

            // draw the board
            g.FillRectangle(System.Drawing.Brushes.DarkRed, (float)(xMargin), (float)yMargin, (float)board.Length, (float)board.Width);

            // loop through all the parts and draw the ones on the current board
            foreach (var iPart in parts)
            {
                // draw the part
                g.FillRectangle(System.Drawing.Brushes.Green,
                    (float)(xMargin + iPart.OffsetLength),
                    (float)(yMargin + iPart.OffsetWidth),
                    (float)iPart.Length,
                    (float)iPart.Width);

                // print the part text
                string text1 = $"{iPart.Name} [{iPart.Length} x {iPart.Width}]";
                System.Drawing.Font partFont = new System.Drawing.Font(new System.Drawing.FontFamily("Consolas"), 15);
                System.Drawing.SizeF textSize = g.MeasureString(text1, partFont);
                if (textSize.Width > iPart.Length) text1 = iPart.Name;
                textSize = g.MeasureString(text1, partFont);
                g.DrawString(text1, partFont, System.Drawing.Brushes.White,
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
        /// Generate the pdf report
        /// </summary>
        /// <param name="Settings"></param>
        /// <param name="Materials"></param>
        /// <param name="Stock"></param>
        /// <param name="Parts"></param>
        /// <returns></returns>
        public PdfSharp.Pdf.PdfDocument Generate(Settings Settings, BindingList<Material> Materials, BindingList<Board> Stock, BindingList<Part> Parts)
        {
            #region // populate header text ...
            document.Info.Title = "Layout Report";
            headerTable[1, 0].AddParagraph("Project:");
            headerTable[1, 1].AddParagraph(Settings.ProjectName ?? "");
            headerTable[1, 2].AddParagraph("Job ref:");
            headerTable[1, 3].AddParagraph(Settings.JobID ?? "");

            headerTable[2, 0].AddParagraph("Client:");
            headerTable[2, 1].AddParagraph(Settings.ClientName ?? "");

            headerTable[3, 0].AddParagraph("Tel nr.:");
            headerTable[3, 1].AddParagraph(Settings.ClientTelNr ?? "");
            headerTable[3, 2].AddParagraph("Kerf:");
            headerTable[3, 3].AddParagraph(Settings.BladeKerf.ToString());

            headerTable[4, 0].AddParagraph("Address:");
            headerTable[4, 1].AddParagraph(Settings.ClientAddr ?? "");
            headerTable[4, 2].AddParagraph("Part-padding:");
            headerTable[4, 3].AddParagraph($"{Settings.PartPaddingLength} x {Settings.PartPaddingWidth} ({(Settings.IncludePaddingInReports ? "included" : "not included")})");
            headerTable.Columns[2].Width = Unit.FromCentimeter(2.6);
            #endregion

            #region // write content into document ...
            var heading = mainSection.AddParagraph("Solution Detail");
            heading.Format.Font.Bold = true;
            heading.Format.Font.Size = 10;
            heading.Format.Font.Underline = Underline.Single;

            // Create table 
            Table table = mainSection.AddTable();
            table.Format.Font.Size = 9;
            table.AddColumn("4cm").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("4cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("2cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;

            var iRow = table.AddRow();
            table.Rows.LeftIndent = 10;
            iRow.HeadingFormat = true;
            iRow.Format.Font.Bold = true;
            iRow.Shading.Color = Colors.Gray;
            iRow[0].AddParagraph("Stock"); iRow[1].AddParagraph("Part"); iRow[2].AddParagraph("Length"); iRow[3].AddParagraph("Width"); iRow[4].AddParagraph("Thick"); iRow[5].AddParagraph("%/dLen"); iRow[6].AddParagraph("dWid");

            foreach (var iStock in Stock)
            {
                var iMaterial = Materials.First(t => t.Name == iStock.Material);
                var packedParts = Parts.Where(p => p.Source == iStock);
                int packedPartsCount = packedParts.Count();
                double TotalpackedArea = packedParts.Sum(p => p.Area);

                iRow = table.AddRow();
                iRow.KeepWith = packedPartsCount == 0 ? 1 : packedPartsCount + 2;
                iRow.Shading.Color = Colors.LightGray;
                iRow.Format.Font.Bold = true;
                iRow[0].MergeRight = 1;
                iRow[0].Format.Alignment = ParagraphAlignment.Left;
                iRow[0].AddParagraph($"{iStock.Name} ({iStock.Material})");
                iRow[2].AddParagraph(iStock.Length.ToString("0.0"));
                iRow[3].AddParagraph(iStock.Width.ToString("0.0"));
                iRow[4].AddParagraph(iMaterial.Thickness.ToString("0.0"));
                iRow[5].AddParagraph($"({(TotalpackedArea / iStock.Area).ToString("0.0%")})");

                bool altRow = false;
                foreach (var iPart in packedParts)
                {
                    if (Settings.IncludePaddingInReports)
                        iPart.Inflate(Settings.PartPaddingWidth, Settings.PartPaddingLength);
                    iRow = table.AddRow();
                    iRow.Format.Font.Size = 8;
                    if (altRow = !altRow) iRow.Shading.Color = Colors.WhiteSmoke;

                    iRow[0].MergeRight = 1;
                    iRow[0].Format.Alignment = ParagraphAlignment.Right;
                    iRow[0].AddParagraph(iPart.Name);
                    iRow[2].AddParagraph(iPart.Length.ToString("0.0"));
                    iRow[3].AddParagraph(iPart.Width.ToString("0.0"));
                    iRow[4].AddParagraph("@");
                    iRow[5].AddParagraph(iPart.OffsetLength.ToString("0.0"));
                    iRow[6].AddParagraph(iPart.OffsetWidth.ToString("0.0"));
                }

                if (packedPartsCount > 0)
                {
                    iRow = table.AddRow();
                    iRow[0].MergeRight = 6;
                    var bitmap = DrawBoard_base64(iStock, packedParts);
                    var img = iRow[0].AddImage("base64:" + bitmap.image);
                    img.LockAspectRatio = true;

                    double maximgwidth = mainSection.PageSetup.PageWidth - LeftMargin - RightMargin - Unit.FromCentimeter(1);
                    double maximgheight = Unit.FromCentimeter(2.5);
                    double xscale = bitmap.Width / maximgwidth;
                    double yscale = bitmap.Height / maximgheight;

                    if (xscale > yscale)
                        img.Width = mainSection.PageSetup.PageWidth - LeftMargin - RightMargin - Unit.FromCentimeter(1);
                    else
                        img.Height = maximgheight;

                    if (Settings.IncludePaddingInReports)
                        Parts.ToList().ForEach(t => t.Inflate(-Settings.PartPaddingWidth, -Settings.PartPaddingLength));
                }
                else
                {
                    iRow = table.AddRow();
                    iRow[0].MergeRight = 6;
                    iRow[0].AddParagraph().AddFormattedText("  (Board not used in this layout.)", TextFormat.Italic);
                }

                table.AddRow();
            }

            Part[] unpackedParts = Parts.Where(t => t.Source == null).ToArray();
            if (unpackedParts.Length > 0)
            {
                iRow = table.AddRow();
                iRow.Shading.Color = Colors.DarkRed;
                iRow.Format.Font.Bold = true;
                iRow.Format.Font.Color = Colors.White;
                iRow[0].MergeRight = 1;
                iRow[0].Format.Alignment = ParagraphAlignment.Left;
                iRow[0].AddParagraph($"Parts not packed");

                for (int j = 0; j < unpackedParts.Length; j++)
                {
                    var iPart = unpackedParts[j];
                    if (Settings.IncludePaddingInReports)
                        iPart.Inflate(Settings.PartPaddingWidth, Settings.PartPaddingLength);
                    iRow = table.AddRow();
                    iRow.Format.Font.Size = 8;
                    if (j % 2 == 1) iRow.Shading.Color = Colors.WhiteSmoke;
                    iRow[0].MergeRight = 1;
                    iRow[0].Format.Alignment = ParagraphAlignment.Right;
                    iRow[0].AddParagraph($"{iPart.Name} ({iPart.Material})");
                    iRow[2].AddParagraph(iPart.Length.ToString("0.0"));
                    iRow[3].AddParagraph(iPart.Width.ToString("0.0"));
                }
                iRow = table.AddRow();

            }

            table = mainSection.AddTable();
            table.Rows.LeftIndent = 10;
            table.AddColumn("3cm").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("0.3cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1cm").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("0.3cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.0cm").Format.Alignment = ParagraphAlignment.Left;
            table.AddRow()[0].MergeRight = 6;
            table.AddRow();
            table.AddRow().Shading.Color = Colors.WhiteSmoke;
            table.AddRow();
            table.AddRow().Shading.Color = Colors.WhiteSmoke;
            table.AddRow();
            table.AddRow().Shading.Color = Colors.WhiteSmoke;

            heading = table[0, 0].AddParagraph("Solution Summary");
            heading.Format.Font.Bold = true;
            heading.Format.Font.Size = 10;
            heading.Format.Font.Underline = Underline.Single;
            table.Rows[0].KeepWith = 6;

            table[1, 0].AddParagraph("Stock boards");
            table[2, 0].AddParagraph("Used boards");
            table[3, 0].AddParagraph("Parts");
            table[4, 0].AddParagraph("Placed parts");
            table[5, 0].AddParagraph("Waste");
            table[6, 0].AddParagraph("Coverage");

            

            double UsedStockArea = Parts.Where(t=>t.Source!= null).Select(p => p.Source).Distinct().Sum(s => s.Area) / 1e6;
            double PlacedPartsArea = Parts.Where(q => q.Source != null).Sum(t => t.Area) / 1e6;

            table[1, 2].AddParagraph(Stock.Count.ToString());
            table[1, 5].AddParagraph((Stock.Sum(t => t.Area) / 1e6).ToString("0.000"));

            table[2, 2].AddParagraph(Parts.Where(t=>t.Source != null).Select(p => p.Source).Distinct().Count().ToString());
            table[2, 5].AddParagraph(UsedStockArea.ToString("0.000"));

            table[3, 2].AddParagraph(Parts.Count.ToString());
            table[3, 5].AddParagraph((Parts.Sum(t => t.Area) / 1e6).ToString("0.000"));

            table[4, 2].AddParagraph(Parts.Count(t => t.Source != null).ToString());
            table[4, 5].AddParagraph(PlacedPartsArea.ToString("0.000"));

            table[5, 2].Format.Font.Bold = true;
            table[5, 3].Format.Font.Bold = true;
            table[5, 2].AddParagraph(((UsedStockArea - PlacedPartsArea) / UsedStockArea * 100).ToString("0.0"));
            table[5, 5].AddParagraph((UsedStockArea - PlacedPartsArea).ToString("0.000"));

            table[6, 2].Format.Font.Bold = true;
            table[6, 3].Format.Font.Bold = true;
            table[6, 2].AddParagraph((PlacedPartsArea / UsedStockArea * 100).ToString("0.0"));
            table[6, 5].AddParagraph(PlacedPartsArea.ToString("0.000"));

            for (int i = 1; i < 7; i++)
            {
                table[i, 1].AddParagraph(":");
                table[i, 4].AddParagraph("(");
                table[i, 6].AddParagraph("m\u00b2)");
            }
            table[5, 3].AddParagraph("%");
            table[6, 3].AddParagraph("%");


            #endregion

            return RenderPdf();
        }
    }
}
