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
    class LayoutReport : ReportBase
    {
        private class Base64Image
        {
            public string image;
            public int Height;
            public int Width;
        }
        private Base64Image DrawBoard_base64(StockItem board)
        {
            double xMargin = 0;
            double yMargin = 20;
            double imageHeight = board.Width + 2 * yMargin;
            double imageWidth = board.Length + 2 * xMargin;

            // create bitmap
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)imageWidth, (int)imageHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            // fill the background with black
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, (int)imageWidth, (int)imageHeight);

            // draw the board
            g.FillRectangle(System.Drawing.Brushes.DarkRed, (float)(xMargin), (float)yMargin, (float)board.Length, (float)board.Width);

            // loop through all the parts and draw the ones on the current board
            for (int i = 0; i < board.PackedPartsCount; i++)
            {
                Part iPlacement = board.PackedParts[i];

                // draw the part
                g.FillRectangle(System.Drawing.Brushes.Green,
                    (float)(xMargin + board.PackedPartdLengths[i]),
                    (float)(yMargin + board.PackedPartdWidths[i]),
                    (float)iPlacement.Length,
                    (float)iPlacement.Width);

                // print the part text
                string text1 = $"{iPlacement.Name} [{iPlacement.Length} x {iPlacement.Width}]";

                System.Drawing.Font partFont = new System.Drawing.Font(new System.Drawing.FontFamily("Consolas"), 15);
                System.Drawing.SizeF textSize = g.MeasureString(text1, partFont);

                if (textSize.Width > iPlacement.Length) text1 = iPlacement.Name;
                textSize = g.MeasureString(text1, partFont);

                g.DrawString(text1, partFont, System.Drawing.Brushes.White,
                    (int)(xMargin + board.PackedPartdLengths[i] + iPlacement.Length / 2.0 - textSize.Width / 2.0),
                    (int)(yMargin + board.PackedPartdWidths[i] + iPlacement.Width / 2.0 - textSize.Height / 2.0));

            }

            g.Flush();

            System.IO.MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byteImage = ms.ToArray();
            var base64img = Convert.ToBase64String(byteImage);

            return new Base64Image()
            {
                image = base64img,
                Height = bitmap.Height,
                Width = bitmap.Width
            };
        }

        public PdfSharp.Pdf.PdfDocument Generate(Settings Settings, BindingList<Material> Materials, BindingList<StockItem> Stock, BindingList<Part> Parts)
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
            headerTable[3, 3].AddParagraph(Settings.BladeKerf ?? "");

            headerTable[4, 0].AddParagraph("Address:");
            headerTable[4, 1].AddParagraph(Settings.ClientAddr ?? "");
            headerTable[4, 2].AddParagraph("Part-padding:");
            headerTable[4, 3].AddParagraph($"{Settings.PartPaddingLength} x {Settings.PartPaddingWidth} ({(Settings.IncludePaddingInReports == "true" ? "included" : "not included")})");
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
            for (int i = 0; i < Stock.Count; i++)
            {
                var iStock = Stock[i];
                var iMaterial = Materials.First(t => t.Name == Stock[i].Material);

                iRow = table.AddRow();
                iRow.KeepWith = iStock.PackedPartsCount == 0 ? 1 : iStock.PackedPartsCount + 2;
                iRow.Shading.Color = Colors.LightGray;
                iRow.Format.Font.Bold = true;
                iRow[0].MergeRight = 1;
                iRow[0].Format.Alignment = ParagraphAlignment.Left;
                iRow[0].AddParagraph($"{iStock.Name} ({iStock.Material})");
                iRow[2].AddParagraph(iStock.Length.ToString("0.0"));
                iRow[3].AddParagraph(iStock.Width.ToString("0.0"));
                iRow[4].AddParagraph(iMaterial.Thickness.ToString("0.0"));
                iRow[5].AddParagraph($"({(iStock.PackedPartsTotalArea / iStock.Area).ToString("0.0%")})");

                for (int j = 0; j < iStock.PackedPartsCount; j++)
                {
                    var iPart = iStock.PackedParts[j];
                    if (Settings.IncludePaddingInReports == "true")
                        iPart.Inflate(double.Parse(Settings.PartPaddingWidth), double.Parse(Settings.PartPaddingLength));
                    iRow = table.AddRow();
                    iRow.Format.Font.Size = 8;
                    if (j % 2 == 1) iRow.Shading.Color = Colors.WhiteSmoke;
                    iRow[0].MergeRight = 1;
                    iRow[0].Format.Alignment = ParagraphAlignment.Right;
                    iRow[0].AddParagraph(iPart.Name);
                    iRow[2].AddParagraph(iPart.Length.ToString("0.0"));
                    iRow[3].AddParagraph(iPart.Width.ToString("0.0"));
                    iRow[4].AddParagraph("@");
                    iRow[5].AddParagraph(iStock.PackedPartdLengths[j].ToString("0.0"));
                    iRow[6].AddParagraph(iStock.PackedPartdWidths[j].ToString("0.0"));
                }

                if (iStock.PackedPartsCount > 0)
                {
                    iRow = table.AddRow();
                    iRow[0].MergeRight = 6;
                    var bitmap = DrawBoard_base64(iStock);
                    var img = iRow[0].AddImage("base64:" + bitmap.image);
                    img.LockAspectRatio = true;

                    double maximgwidth = mainSection.PageSetup.PageWidth - LeftMargin - RightMargin - Unit.FromCentimeter(1);
                    double maximgheight = Unit.FromCentimeter(2.5);
                    double xscale = bitmap.Width / maximgwidth;
                    double yscale = bitmap.Height / maximgheight;

                    if (xscale > yscale)
                        img.Width = mainSection.PageSetup.PageWidth - LeftMargin - RightMargin - Unit.FromCentimeter(1);
                    else
                        img.Height = Unit.FromCentimeter(3);

                    if (Settings.IncludePaddingInReports == "true")
                        iStock.PackedParts.ToList().ForEach(t => t.Inflate(-double.Parse(Settings.PartPaddingWidth), -double.Parse(Settings.PartPaddingLength)));
                }
                else
                {
                    iRow = table.AddRow();
                    iRow[0].MergeRight = 6;
                    iRow[0].AddParagraph().AddFormattedText("  (Board not used in this layout.)", TextFormat.Italic);
                }

                table.AddRow();
            }

            Part[] unpackedParts = Parts.Where(t => t.isPacked == false).ToArray();
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
                    if (Settings.IncludePaddingInReports == "true")
                        iPart.Inflate(double.Parse(Settings.PartPaddingWidth), double.Parse(Settings.PartPaddingLength));
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

            double UsedStockArea = Stock.Where(q => q.PackedPartsCount > 0).Sum(t => t.Area) / 1e6;
            double PlacedPartsArea = Parts.Where(q => q.isPacked).Sum(t => t.Area) / 1e6;

            table[1, 2].AddParagraph(Stock.Count.ToString());
            table[1, 5].AddParagraph((Stock.Sum(t => t.Area) / 1e6).ToString("0.000"));

            table[2, 2].AddParagraph(Stock.Count(t => t.PackedPartsCount > 0).ToString());
            table[2, 5].AddParagraph(UsedStockArea.ToString("0.000"));

            table[3, 2].AddParagraph(Parts.Count.ToString());
            table[3, 5].AddParagraph((Parts.Sum(t => t.Area) / 1e6).ToString("0.000"));

            table[4, 2].AddParagraph(Parts.Count(t => t.isPacked).ToString());
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
