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
            #region // Configuration settings ...

            int colCount = 4;                       // the number of labels per row
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
            mainSection.PageSetup.LeftMargin = LeftMargin;
            mainSection.PageSetup.RightMargin = RightMargin;
            mainSection.PageSetup.TopMargin = TopMargin;
            mainSection.PageSetup.BottomMargin = BottomMargin;
            #endregion

            #region // write content into document ...
            // Create a table spanning whole page
            Table table = mainSection.AddTable();
            Unit colWidth = (document.DefaultPageSetup.PageWidth - LeftMargin - RightMargin)/ colCount;
            Unit rowHeight = (document.DefaultPageSetup.PageHeight - TopMargin - BottomMargin) / rowCount;
            for (int i = 0; i < colCount; i++)
                table.AddColumn(colWidth);
            for (int i = 0; i * colCount < Parts.Count; i++)
                table.AddRow().Height = rowHeight;

            //loop through all the stock items
            int cntr = 0;
            for (int i = 0; i < Stock.Count; i++)
            {
                var iStock = Stock[i];  // reference the item

                // loop throug all the parts placed on the stock item (if any)
                for (int j = 0; j < iStock.PackedPartsCount; j++)
                {
                    var iPart = iStock.PackedParts[j];  // reference the packed part item

                    // determine the column and row for the part record in the table
                    int cindex = cntr % (colCount);
                    int rindex = cntr / (colCount);
                    
                    // create a nested table to organise the part info on the label
                    Table labelTable = new Table();
                    // add the nested table to the table managing the labels
                    table[rindex, cindex].Elements.Add(labelTable);
                    labelTable.AddColumn(colWidth);
                    labelTable.AddRow();
                    labelTable.AddRow();
                    labelTable.AddRow();
                    labelTable.AddRow();
                    labelTable.AddRow();


                    Cell c = labelTable[0, 0];  // reference the label content table's top cell
                    c.Format.Font.Bold = true;
                    c.Format.Font.Size = 15;
                    c.AddParagraph(iPart.Name);

                    c = labelTable[1, 0];       // reference the second row
                    c.Format.Font.Size = 12;
                    c = labelTable[2, 0];
                    c.Format.Font.Size = 8;
                    c = labelTable[4, 0];
                    c.Format.Font.Size = 8;
                    c.AddParagraph($"[{iPart.Length:0.0} x {iPart.Width:0.0}]");

                    // populate third row
                    labelTable[2, 0].AddParagraph($" + [{(2*Settings.PartPaddingLength):0.0} x {(2*Settings.PartPaddingWidth):0.0}]");

                    // populate fourth row
                    labelTable[3, 0].AddParagraph().AddFormattedText("on ").AddFormattedText($"{iStock.Name}",TextFormat.Bold);

                    // populate sixth row
                    labelTable[4, 0].AddParagraph($"@ ({iStock.PackedPartdLengths[j]:0.0}, {iStock.PackedPartdWidths[j]:0.0})");

                    cntr++;
                }
            }

            #endregion

            return RenderPdf();
        }
    }
}
