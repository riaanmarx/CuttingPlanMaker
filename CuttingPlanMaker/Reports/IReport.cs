using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// Interface required of all report implementing classes
    /// </summary>
    interface IReport
    {
        /// <summary>
        /// Generate the PDF for the report
        /// </summary>
        /// <param name="Settings"></param>
        /// <param name="Materials"></param>
        /// <param name="Stock"></param>
        /// <param name="Parts"></param>
        /// <returns></returns>
        PdfSharp.Pdf.PdfDocument Generate(Settings Settings, BindingList<Material> Materials, BindingList<StockItem> Stock, BindingList<Part> Parts);
    }
}
