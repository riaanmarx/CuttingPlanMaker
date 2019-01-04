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
    class StockReport : ReportBase
    {
        public PdfSharp.Pdf.PdfDocument Generate(Settings Settings, BindingList<Material> Materials, BindingList<StockItem> Stock, BindingList<Part> Parts)
        {
            #region // populate header text ...
            document.Info.Title = "Stock List Report";
            headerTable[2, 0].AddParagraph("Client:");
            headerTable[2, 1].AddParagraph(Settings.ClientName);
            //headerTable[2, 2].AddParagraph("Material:");
            //headerTable[2, 3].AddParagraph("Kiaat (AB)-25mm");

            headerTable[3, 0].AddParagraph("Tel nr.:");
            headerTable[3, 1].AddParagraph(Settings.ClientTelNr);
            headerTable[3, 2].AddParagraph();
            headerTable[3, 3].AddParagraph();

            headerTable[4, 0].AddParagraph("Address:");
            headerTable[4, 1].AddParagraph(Settings.ClientAddr);
            headerTable[4, 2].AddParagraph("Date:");
            headerTable[4, 3].AddParagraph(Settings.TargetDate);
            #endregion

            #region // write content into document ...
            // Create table 
            Table table = mainSection.AddTable();
            table.Format.Font.Size = 9;
            //table.Borders.Width = 1;
            //Name Material Length Width Thickness Vol unitCost cost
            for (int i = 0; i < 8; i++)
                table.AddColumn().Format.Alignment= i<2? ParagraphAlignment.Left: ParagraphAlignment.Right;
            table.Columns.Width = Unit.FromCentimeter(1.5);
            table.Columns[0].Width =
            table.Columns[1].Width = Unit.FromCentimeter(3.5);
            table.Columns[5].Width =
                table.Columns[6].Width =
                table.Columns[7].Width = Unit.FromCentimeter(2.0);

            //loop through all the stock items and add a new header for every new page

            var iRow = table.AddRow();
            table.Rows.LeftIndent = 10;
            iRow.HeadingFormat = true;
            iRow.Format.Font.Bold = true;
            iRow.Shading.Color = Colors.LightGray;
            iRow[0].AddParagraph("Name"); iRow[1].AddParagraph("Material"); iRow[2].AddParagraph("Length"); iRow[3].AddParagraph("Width"); iRow[4].AddParagraph("Thick"); iRow[6].AddParagraph("Vol"); iRow[5].AddParagraph("Cost/m3"); iRow[7].AddParagraph("Cost");
            double totVol = 0;
            double totCost = 0;

            for (int i = 0; i < Stock.Count; i++)
            {
                var iStock = Stock[i];
                var iMaterial = Materials.First(t => t.Name == Stock[i].Material);

                iRow = table.AddRow();
                if (i % 2 == 1) iRow.Shading.Color = Colors.WhiteSmoke;
                iRow[0].AddParagraph(iStock.Name);
                iRow[1].AddParagraph(iStock.Material);
                iRow[2].AddParagraph(iStock.Length.ToString("0.0"));
                iRow[3].AddParagraph(iStock.Width.ToString("0.0"));
                iRow[4].AddParagraph(iMaterial.Thickness.ToString("0.0"));
                double vol = iStock.Length * iStock.Width * iMaterial.Thickness / 1e9f;
                totVol += vol;
                iRow[6].AddParagraph(vol.ToString("0.000"));
                iRow[5].AddParagraph(iMaterial.Cost.ToString("0.00"));

                double cost = vol * iMaterial.Cost;
                totCost += cost;
                iRow[7].AddParagraph(cost.ToString("0.00"));
            }

            iRow = table.AddRow();
            iRow[6].Borders.Top.Width = 2;
            iRow[6].AddParagraph().AddFormattedText(totVol.ToString("0.000"),TextFormat.Bold);
            iRow[7].Borders.Top.Width = 2;
            iRow[7].AddParagraph().AddFormattedText(totCost.ToString("0.00"),TextFormat.Bold);


            #endregion
            return RenderPdf();
        }
    }
}
