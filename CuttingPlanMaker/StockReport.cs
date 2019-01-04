using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    class StockReport : ReportBase
    {
        public PdfSharp.Pdf.PdfDocument Generate(BindingList<StockItem> Stock)
        {
            #region // populate header text ...
            document.Info.Title = "Stock Report";
            headerTable[2, 0].AddParagraph("Client:");
            headerTable[2, 1].AddParagraph("Riaan Marx");
            headerTable[2, 2].AddParagraph("Material:");
            headerTable[2, 3].AddParagraph("Kiaat (AB)-25mm");

            headerTable[3, 0].AddParagraph("Tel nr.:");
            headerTable[3, 1].AddParagraph("0828088900");
            headerTable[3, 2].AddParagraph();
            headerTable[3, 3].AddParagraph();

            headerTable[4, 0].AddParagraph("Address:");
            headerTable[4, 1].AddParagraph("129 Kestrel str., The Reeds");
            headerTable[4, 2].AddParagraph("Date:");
            headerTable[4, 3].AddParagraph(DateTime.Now.ToString("dd MMM yyyy"));
            #endregion

            #region // write content into document ...


            // Add a paragraph to the section
            var paragraph = mainSection.AddParagraph();

            // Add some text and an image to the paragraph
            paragraph.AddFormattedText("Hello, World!", TextFormat.Italic);
            //paragraph.AddImage("SomeImage.png");



            //// Create table 
            //Table table = section.AddTable();

            //// Define two columns 
            //Column column = table.AddColumn();
            //column = table.AddColumn();

            //// Create two rows with content 
            //Row row = table.AddRow();
            //row.Borders.Color = Colors.Black;
            //row.Cells[0].AddParagraph("Text row 0, column 0");

            //// Nifty trick to get nested table 
            //Table innerTableLeft = row.Cells[0].Elements.AddTable();
            //innerTableLeft.AddColumn();
            //Row innerRow = innerTableLeft.AddRow();
            //innerRow.Cells[0].AddParagraph("some text in the inner table here");

            //row.Cells[1].AddParagraph("Text in row 0, column 1");

            //row = table.AddRow();
            //row.Cells[0].AddParagraph("Text in row 1, column 0");
            //row.Cells[1].AddParagraph("Text in row 1, column 1");


            for (int i = 0; i < 200; i++)
            {
                mainSection.AddParagraph($"paragraph {i}");
            }
            #endregion

            return RenderPdf();
        }
    }
}
