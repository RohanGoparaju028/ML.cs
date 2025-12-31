using System;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.Analysis;
namespace ML.cs.DataPreprocessing;
public class PreProcessing  {
    // The main goal of this function is to read a csv file since we
    // perform the data preprocessing and applied machine learning process
    public static DataFrame ReadCSV(string csv) {
       if(!File.Exists(csv)) {
           throw new FileNotFoundException("passed csv file does not exit in the current directory please provide the correct address");
       }
      using var reader = new StreamReader(csv);
      using var _csv = new CsvReader(reader,new CsvConfiguration(CultureInfo.InvariantCulture)
      {
          MissingFieldFound = null,
          BadDataFound = null
      });
      _csv.Read();
      _csv.ReadHeader();
      var header = _csv.HeaderRecord;
      var columns = header
          .Select(h => new StringDataFrameColumn(h))
          .ToArray(); // We are converting TO array because DataFrame constructor
      var df = new DataFrame(columns);
      while(_csv.Read()) {
          for(int i=0;i<header.Length;i++) {
              string? val = i < _csv.Parser.Count ? _csv.GetField(i) : null;
              ((StringDataFrameColumn)df.Columns[i]).Append(
                string.IsNullOrWhiteSpace(val) ? null : val
              );
          }
      }
      return df;
      }
    // The function is used to get the  number of null present in the dictionary
      public static void GetNullSum(DataFrame df) {
          Console.WriteLine("ColumnName:Count");
          foreach(var col in df.Columns) {
              long nullcount = col.NullCount;
              Console.WriteLine($"{col.Name}:{nullcount}");
          }
      }
    // THe functoion used to drop the row that contains null value
    //public static DataFrame DropRow(DataFrame input) {

    //}
    // This function used to drop the entire column that contains null value
   // public static Dictionary<string,List<string>> Dropcolumn(Dictionary<string,List<string?>> data,string column) {

    //}
    // THis function is used to replace all the null values with mean
    //public static IDictionary<string,List<string>> FillNull(IDictionary<string,List<string?>> input) {

    //}

}
