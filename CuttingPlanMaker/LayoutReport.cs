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
    class LayoutReport:ReportBase
    {
        public PdfSharp.Pdf.PdfDocument Generate(BindingList<StockItem> Stock, BindingList<Material> Materials)
        {
            #region // populate header text ...
            document.Info.Title = "Layout Report";
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
            // Create table 
            Table table = mainSection.AddTable();
            table.Format.Font.Size = 9;
            table.AddColumn("4cm").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("4cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("2cm").Format.Alignment = ParagraphAlignment.Right;
            table.AddColumn("1.5cm").Format.Alignment = ParagraphAlignment.Right;
            
            var iRow = table.AddRow();
            //table.Borders.Width = 1;
            table.Rows.LeftIndent = 10;
            iRow.HeadingFormat = true;
            iRow.Format.Font.Bold = true;
            iRow.Shading.Color = Colors.Gray;
            iRow[0].AddParagraph("Stock"); iRow[1].AddParagraph("Part"); iRow[2].AddParagraph("Length"); iRow[3].AddParagraph("Width"); iRow[4].AddParagraph("Thick"); iRow[5].AddParagraph("% / dLen"); iRow[6].AddParagraph("dWid");
            for (int i = 0; i < Stock.Count; i++)
            {
                var iStock = Stock[i];
                var iMaterial = Materials.First(t => t.Name == Stock[i].Material);

                iRow = table.AddRow();
                iRow.Shading.Color = Colors.LightGray;
                iRow.Format.Font.Bold = true;
                iRow[0].MergeRight = 1;
                iRow[0].Format.Alignment = ParagraphAlignment.Left;
                iRow[0].AddParagraph($"{iStock.Name} ({iStock.Material})");
                iRow[2].AddParagraph(iStock.Length.ToString("0.0"));
                iRow[3].AddParagraph(iStock.Width.ToString("0.0"));
                iRow[4].AddParagraph(iMaterial.Thickness.ToString("0.0"));
                iRow[5].AddParagraph($"({(0.87).ToString("0.0%")})");

                for (int j = 0; j < 10; j++)
                {
                    var iPart = new Part()
                    {
                        Name = j.ToString("000"),
                        Length = (float)(new Random().NextDouble()*2100.0),
                        Width= (float)(new Random().NextDouble()*2100.0),
                        Material = "Kiaat(AB)-25mm"
                    };


                    iRow = table.AddRow();
                    if (j % 2 == 1) iRow.Shading.Color = Colors.WhiteSmoke;
                    iRow[0].MergeRight = 1;
                    iRow[0].Format.Alignment = ParagraphAlignment.Right;
                    iRow[0].AddParagraph(iPart.Name);
                    iRow[2].AddParagraph(iPart.Length.ToString("0.0"));
                    iRow[3].AddParagraph(iPart.Width.ToString("0.0"));
                    iRow[4].AddParagraph("@");
                    iRow[5].AddParagraph(iPart.Length.ToString("0.0"));
                    iRow[6].AddParagraph(iPart.Width.ToString("0.0"));
                }
                
            }

            #endregion
            return RenderPdf();
        }
    }
}
