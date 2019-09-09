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
    /// Part list report generating class
    /// </summary>
    class PartListReport : ReportBase
    {
        /// <summary>
        /// Generate the part list report
        /// </summary>
        /// <param name="Settings"></param>
        /// <param name="Materials"></param>
        /// <param name="Stock"></param>
        /// <param name="Parts"></param>
        /// <returns></returns>
        public PdfSharp.Pdf.PdfDocument Generate(Settings Settings, BindingList<Material> Materials, BindingList<Board> Stock, BindingList<Part> Parts)
        {
            #region // populate header text ...
            document.Info.Title = "Parts List Report";
            headerTable[1, 0].AddParagraph("Project:");
            headerTable[1, 1].AddParagraph(Settings.ProjectName);
            headerTable[1, 2].AddParagraph("Job ref:");
            headerTable[1, 3].AddParagraph(Settings.JobID);

            headerTable[2, 0].AddParagraph("Client:");
            headerTable[2, 1].AddParagraph(Settings.ClientName);

            headerTable[3, 0].AddParagraph("Tel nr.:");
            headerTable[3, 1].AddParagraph(Settings.ClientTelNr);
            headerTable[3, 2].AddParagraph("Target Date:");
            headerTable[3, 3].AddParagraph(Settings.TargetDate);

            headerTable[4, 0].AddParagraph("Address:");
            headerTable[4, 1].AddParagraph(Settings.ClientAddr);
            
            headerTable[4, 2].AddParagraph("Part-padding:");
            headerTable[4, 3].AddParagraph($"{Settings.PartPaddingLength} x {Settings.PartPaddingWidth} ({(Settings.IncludePaddingInReports?"included":"not included")})");
            headerTable.Columns[2].Width = Unit.FromCentimeter(2.6);
            #endregion

            #region // write content into document ...
            // Create table 
            Table table = mainSection.AddTable();
            table.Format.Font.Size = 9;

            for (int i = 0; i < 8; i++)
                table.AddColumn().Format.Alignment = i < 2 ? ParagraphAlignment.Left : ParagraphAlignment.Right;
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

            for (int i = 0; i < Parts.Count; i++)
            {
                var iPart = Parts[i];
                if (Settings.IncludePaddingInReports) iPart.Inflate(Settings.PartPaddingWidth, Settings.PartPaddingLength);
                var iMaterial = Materials.First(t => t.Name == Parts[i].Material);

                iRow = table.AddRow();
                if (i % 2 == 1) iRow.Shading.Color = Colors.WhiteSmoke;
                iRow[0].AddParagraph(iPart.Name);
                iRow[1].AddParagraph(iPart.Material);
                iRow[2].AddParagraph(iPart.Length.ToString("0.0"));
                iRow[3].AddParagraph(iPart.Width.ToString("0.0"));
                iRow[4].AddParagraph(iMaterial.Thickness.ToString("0.0"));
                double vol = iPart.Length * iPart.Width * iMaterial.Thickness / 1e9f;
                totVol += vol;
                iRow[6].AddParagraph(vol.ToString("0.000"));
                iRow[5].AddParagraph(iMaterial.Cost.ToString("0.00"));

                double cost = vol * iMaterial.Cost;
                totCost += cost;
                iRow[7].AddParagraph(cost.ToString("0.00"));
                if (Settings.IncludePaddingInReports) iPart.Inflate(-Settings.PartPaddingWidth, -Settings.PartPaddingLength);

            }

            iRow = table.AddRow();
            iRow[6].Borders.Top.Width = 2;
            iRow[6].AddParagraph().AddFormattedText(totVol.ToString("0.000"), TextFormat.Bold);
            iRow[7].Borders.Top.Width = 2;
            iRow[7].AddParagraph().AddFormattedText(totCost.ToString("0.00"), TextFormat.Bold);

            mainSection.AddParagraph("Boards required (calculated at 20% waste and 25mm thickness)");
            table = mainSection.AddTable();
            table.Rows.LeftIndent = 10;
            for (int i = 0; i < 8; i++) table.AddColumn().Width = Unit.FromCentimeter(1.5);
            for (int i = 0; i < 10; i++) table.AddRow().Shading.Color = i%2==0 ? Colors.WhiteSmoke : Colors.Transparent;

            table.Format.Alignment = ParagraphAlignment.Center;
            table.Rows[0].Format.Font.Bold = true;
            table.Rows[0].Shading.Color = Colors.LightGray;
            table.Columns[0].Format.Font.Bold = true;
            table.Columns[0].Shading.Color = Colors.LightGray;
            table.Columns[0].Width = Unit.FromCentimeter(3.5);
            table.Columns[0].Format.Alignment = ParagraphAlignment.Right;


            double[] lengths_m = new double[] { 1800, 2000, 2100, 2200, 2500, 3000, 3500 };
            double[] widths_mm = new double[] { 80, 100, 120, 150, 180, 200, 220, 230, 250 };

            table[0, 0].AddParagraph("AVG Width\\Length");
            for (int i = 0; i < lengths_m.Length; i++) table[0, i + 1].AddParagraph($"{lengths_m[i] / 1000:0.0}");
            for (int i = 0; i < widths_mm.Length; i++) table[i + 1, 0].AddParagraph($"{widths_mm[i]:0}");
            for (int i = 0; i < lengths_m.Length; i++)
            {
                for (int j = 0; j < widths_mm.Length; j++)
                {
                    double val = Math.Ceiling( totVol * (1.2) / 0.025 / (lengths_m[i]/1000) / (widths_mm[j] / 1000) );
                    table[j+1, i+1].AddParagraph($"{val:0}");
                }
            }
            #endregion

            return RenderPdf();
        }
    }
}
