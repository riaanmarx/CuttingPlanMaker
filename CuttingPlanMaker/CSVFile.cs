using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingPlanMaker
{
    /// <summary>
    /// A simple class to deal with CSV files
    /// </summary>
    class CSVFile
    {
        /// <summary>
        /// Read a CSV file into a BindingList of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BindingList<T> Read<T>(string path)
            where T : class, new()
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true,
                UseFieldIndexForReadingData = true
            };
            CsvContext cc = new CsvContext();

            BindingList<T> records = new BindingList<T>(
                cc.Read<T>(path, inputFileDescription).ToList());

            return records;
        }

        /// <summary>
        /// Write the contents of a BindingList to a CSV file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="path"></param>
        public static void Write<T>(BindingList<T> list, string path) where T : class, new()
        {
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true,
                UseFieldIndexForReadingData = true,
                
            };
            CsvContext cc = new CsvContext();
            
            cc.Write<T>(list, path, outputFileDescription);
        }
    }
}
