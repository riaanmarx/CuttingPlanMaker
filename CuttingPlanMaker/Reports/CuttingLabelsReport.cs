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
    /// <summary>
    /// A PDF file generating class to generate labels/stickers for the parts
    /// </summary>
    class CuttingLabelReport : ReportBase
    {
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
            for (int i = 0; i * colCount < Parts.Count; i++)
                table.AddRow().Height = rowHeight;

            //loop through all the stock items
            int cntr = 0;
            foreach (var iStock in Stock)
            {
                // loop throug all the parts placed on the stock item (if any)
                foreach(var iPart in Parts.Where(p => p.Source == iStock))
                {
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

                    Cell c = labelTable[0, 0];  // top row := part's name
                    c.Format.Font.Bold = true;
                    c.Format.Font.Size = 15;
                    c.AddParagraph(iPart.Name);

                    c = labelTable[1, 0];       // Second row := dimensions
                    c.Format.Font.Size = 12;
                    c.AddParagraph($"[{iPart.Length:0.0} x {iPart.Width:0.0}]");

                    //c = labelTable[2, 0];       // Third row := padding dimensions
                    //c.Format.Font.Size = 8;
                    //c.AddParagraph($" + [{(2 * Settings.PartPaddingLength):0.0} x {(2 * Settings.PartPaddingWidth):0.0}]");

                    //c = labelTable[3, 0];       // Fourth row := board name
                    //c.AddParagraph().AddFormattedText("on ").AddFormattedText($"{iStock.Name}", TextFormat.Bold);

                    //c = labelTable[4, 0];       // Fifth row := placement offset
                    //c.Format.Font.Size = 8;
                    //c.AddParagraph($"@ ({iPart.OffsetLength:0.0}, {iPart.OffsetWidth:0.0})");

                    cntr++;
                }
            }

            #endregion

            return RenderPdf();
        }
    }
}
