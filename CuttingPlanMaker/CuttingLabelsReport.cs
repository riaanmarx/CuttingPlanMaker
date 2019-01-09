using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    class CuttingLabelReport : ReportBase
    {
        public PdfSharp.Pdf.PdfDocument Generate(Settings Settings, BindingList<Material> Materials, BindingList<StockItem> Stock, BindingList<Part> Parts)
        {
            #region // clear header ...
            document.Sections.Clear();
            mainSection = document.AddSection();
            #endregion

            #region // write content into document ...
            int colCount = 4;
            int rowCount = 10;
            LeftMargin = Unit.FromMillimeter(10);
            TopMargin = Unit.FromMillimeter(10);
            RightMargin = Unit.FromMillimeter(10);
            BottomMargin = Unit.FromMillimeter(10);

            // set up document
            document.DefaultPageSetup.LeftMargin = LeftMargin;
            document.DefaultPageSetup.RightMargin = RightMargin;
            document.DefaultPageSetup.TopMargin = TopMargin;
            document.DefaultPageSetup.BottomMargin = BottomMargin;

            // Create table 
            Table table = mainSection.AddTable();
            //table.Borders.Color = Colors.Blue;
            Unit colWidth = (document.DefaultPageSetup.PageWidth - LeftMargin - RightMargin)/ colCount;
            Unit rowHeight = (document.DefaultPageSetup.PageHeight - TopMargin - BottomMargin) / rowCount;
            for (int i = 0; i < colCount; i++)
                table.AddColumn(colWidth);

            for (int i = 0; i * colCount < Parts.Count; i++)
                table.AddRow().Height = rowHeight;

            //loop through all the stock items and add a new header for every new page

            int cntr = 0;
            for (int i = 0; i < Stock.Count; i++)
            {
                var iStock = Stock[i];
                for (int j = 0; j < iStock.PackedPartsCount; j++)
                {
                    var iPart = iStock.PackedParts[j];

                    int cindex = cntr % (colCount);
                    int rindex = cntr / (colCount);
                    
                    Table labelTable = new Table();
                    //labelTable.Borders.Color = Colors.LightGray;
                    table[rindex, cindex].Elements.Add(labelTable);

                    labelTable.AddColumn(colWidth);
                    labelTable.AddRow();
                    labelTable.AddRow();
                    labelTable.AddRow();
                    labelTable.AddRow();
                    labelTable.AddRow();
                    Cell c = labelTable[0, 0];
                    c.Format.Font.Bold = true;
                    c.Format.Font.Size = 15;
                    c.AddParagraph(iPart.Name);
                    labelTable[1, 0].AddParagraph($"[{iPart.Length:0.0} x {iPart.Width:0.0}]");
                    c = labelTable[1, 0];
                    c.Format.Font.Size = 12;
                    c = labelTable[2, 0];
                    c.Format.Font.Size = 8;
                    c = labelTable[4, 0];
                    c.Format.Font.Size = 8;
                    labelTable[2, 0].AddParagraph($" + [{(2*double.Parse(Settings.PartPaddingLength)):0.0} x {(2*double.Parse(Settings.PartPaddingWidth)):0.0}]");
                    labelTable[3, 0].AddParagraph().AddFormattedText("on ").AddFormattedText($"{iStock.Name}",TextFormat.Bold);
                    labelTable[4, 0].AddParagraph($"@ ({iStock.PackedPartdLengths[j]:0.0}, {iStock.PackedPartdWidths[j]:0.0})");
                    cntr++;
                }
            }

            #endregion
            return RenderPdf();
        }
    }
}
