using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    class StockReport
    {

        public PdfSharp.Pdf.PdfDocument Generate(BindingList<StockItem> Stock)
        {
            // Create a new MigraDoc document
            Document document = new Document();
            document.DefaultPageSetup.Clone();
            // Add a section to the document
            Section mainSection = document.AddSection();

            Unit TitleBlockHeight = Unit.FromCentimeter(3);

            // set up page header
            mainSection.PageSetup.TopMargin = TitleBlockHeight + document.DefaultPageSetup.HeaderDistance;
            mainSection.PageSetup.BottomMargin = Unit.FromCentimeter(1.7);
            Table pageHeaderTable = mainSection.Headers.Primary.AddTable();
            pageHeaderTable.Borders.Color = Colors.Gray;
            pageHeaderTable.AddColumn();
            pageHeaderTable.AddColumn();
            pageHeaderTable.AddColumn();
            pageHeaderTable.AddColumn();
            pageHeaderTable.AddRow();
            pageHeaderTable.AddRow();
            pageHeaderTable.AddRow();
            pageHeaderTable.AddRow();
            pageHeaderTable.AddRow();
            pageHeaderTable.AddRow();

            pageHeaderTable.Rows[0][0].MergeRight = 3;
            pageHeaderTable.Rows[5][0].MergeRight = 3;

            //pageHeaderTable.Rows[0].Borders.Top.Color = Colors.Black;
            //pageHeaderTable.Columns[0].Borders.Left.Color = Colors.Black;
            //pageHeaderTable.Columns[3].Borders.Right.Color = Colors.Black;
            //pageHeaderTable.Rows[5][0].Borders.Top.Color = Colors.Black;

            pageHeaderTable.Rows[0].Height = 20;
            pageHeaderTable.Rows.Height = (TitleBlockHeight - pageHeaderTable.Rows[0].Height) / 4;
            pageHeaderTable.Rows[5].Height = document.DefaultPageSetup.PageHeight 
                - mainSection.PageSetup.TopMargin
                - mainSection.PageSetup.BottomMargin;

            pageHeaderTable.Columns.Width = (document.DefaultPageSetup.PageWidth - document.DefaultPageSetup.LeftMargin - document.DefaultPageSetup.RightMargin)/4;

            var titlePar = pageHeaderTable.Rows[0][0].AddParagraph();
            titlePar.AddFormattedText("Report Title", TextFormat.Bold);
            titlePar.Format.Alignment = ParagraphAlignment.Center;
            titlePar.Format.Font.Size = 15;


            // set up page footer
            Paragraph pageFooter = mainSection.Footers.Primary.AddParagraph();
            pageFooter.AddFormattedText("Generated using Cuttong Plan Maker", TextFormat.Italic);
            pageFooter.Format.Font.Size = 6;
            pageFooter.Format.Alignment = ParagraphAlignment.Center;

            
            

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



            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();


            pdfRenderer.PdfDocument.Save("HelloWorld.pdf");
            return pdfRenderer.PdfDocument;
        }
    }
}
