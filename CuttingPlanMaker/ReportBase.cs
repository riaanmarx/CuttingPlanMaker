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
    class ReportBase
    {
        // references needed by inheritants
        internal Document document;
        internal Section mainSection;
        internal Table headerTable;

        // constant sizes
        internal Unit TitleBlockHeight = Unit.FromCentimeter(2.8);
        internal Unit FooterHeight = Unit.FromCentimeter(0.4);
        internal Unit LeftMargin = Unit.FromCentimeter(1.0);
        internal Unit RightMargin = Unit.FromCentimeter(1.0);
        internal Unit TopMargin = Unit.FromCentimeter(1.0);
        internal Unit BottomMargin = Unit.FromCentimeter(0.7);
        internal Unit TitleRowHeight = Unit.FromCentimeter(1);
        internal Unit TitleFontSize = Unit.FromCentimeter(0.6);
        internal Unit FooterFontSize = Unit.FromCentimeter(0.3);
        internal Unit TitleBlockFontSize = Unit.FromCentimeter(0.3);
        internal Unit ColWidthLabels = Unit.FromCentimeter(2.0);

        public ReportBase()
        {
            #region // Set up report document and page setup ...
            // Create a new document
            document = new Document();

            // just in case we change something on the default page setup, lets clone it first
            document.DefaultPageSetup.Clone();

            // Add a section to the document
            mainSection = document.AddSection();
            
            // get references to the section and default page setup
            PageSetup psSection = mainSection.PageSetup;
            PageSetup psDefault = document.DefaultPageSetup;

            // set up margins for content
            psSection.PageWidth = psDefault.PageWidth;
            psSection.PageHeight = psDefault.PageHeight;
            psSection.HeaderDistance = TopMargin;
            psSection.FooterDistance = BottomMargin;
            psSection.LeftMargin = LeftMargin;
            psSection.RightMargin = RightMargin;
            psSection.TopMargin = TitleBlockHeight + TopMargin + Unit.FromCentimeter(0.2);
            psSection.BottomMargin = BottomMargin + FooterHeight;
            #endregion

            #region // draw title block ...
            // add a (4x6) table to the header for the title block
            HeaderFooter header = mainSection.Headers.Primary;
            headerTable = header.AddTable();
            headerTable.Format.Font.Size = TitleBlockFontSize;
            for (int i = 0; i < 4; i++) headerTable.AddColumn();
            for (int i = 0; i < 6; i++) headerTable.AddRow();

            // merge the columns for the top row and the row surrounding the content
            headerTable[0,0].MergeRight = 3;
            headerTable[5,0].MergeRight = 3;

            // set cell sizes
            headerTable.Columns.Width = ColWidthLabels;
            headerTable.Columns[1].Width =
            headerTable.Columns[3].Width = (psDefault.PageWidth - LeftMargin - RightMargin - 2 * ColWidthLabels) / 2;

            headerTable.Columns[0].Format.Font.Bold = 
            headerTable.Columns[2].Format.Font.Bold = true;

            headerTable.Rows[0].Height = TitleRowHeight;
            headerTable.Rows.Height = (TitleBlockHeight - TitleRowHeight) / 4;
            headerTable.Rows[5].Height = psDefault.PageHeight
                - psSection.TopMargin
                - psSection.BottomMargin;

            // set borders for the cells
            //headerTable.Borders.Color = Colors.Gray;
            headerTable[0, 0].Borders.Top.Width = 2;
            headerTable.Columns[0].Borders.Left.Width = 2;
            headerTable.Columns[3].Borders.Right.Width = 2;
            headerTable[5, 0].Borders.Top.Width = 2;
            headerTable[5, 0].Borders.Bottom.Width = 2;
            headerTable[0, 0].Format.Alignment = ParagraphAlignment.Center;
            headerTable[0, 0].Format.Font.Size = TitleFontSize;
            #endregion

            #region // set some default header values ...
            headerTable[0, 0].AddParagraph().AddInfoField(MigraDoc.DocumentObjectModel.Fields.InfoFieldType.Title);
            headerTable[1, 0].AddParagraph("Project:");
            headerTable[1, 1].AddParagraph("Kitchen cabinets");
            headerTable[1, 2].AddParagraph("Job ref:");
            headerTable[1, 3].AddParagraph("201810-01"); 
            #endregion


            #region // set up footer ...
            var FooterTable = mainSection.Footers.Primary.AddTable();
            FooterTable.Format.Font.Size = FooterFontSize;
            FooterTable.Format.Alignment = ParagraphAlignment.Center;
            FooterTable.AddColumn("1cm");
            FooterTable.AddColumn(psSection.PageWidth - Unit.FromCentimeter(6) - LeftMargin - RightMargin);
            FooterTable.AddColumn("5cm");
            FooterTable.AddRow();
            FooterTable[0, 0].AddParagraph().AddPageField();
            FooterTable[0, 1].AddParagraph().AddFormattedText("Generated using Cuttong Plan Maker", TextFormat.Italic);
            FooterTable[0, 2].AddParagraph().AddFormattedText($"printed {DateTime.Now.ToString("HH:mm, dd MMM yyyy")}", TextFormat.Italic);
            #endregion


        }

        public PdfSharp.Pdf.PdfDocument RenderPdf()
        {
            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.PageLayout = PdfSharp.Pdf.PdfPageLayout.SinglePage;
            return pdfRenderer.PdfDocument;
        }

    }


    
}
