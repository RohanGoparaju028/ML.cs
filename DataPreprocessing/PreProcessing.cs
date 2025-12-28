using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
namespace ML.cs.DataPreprocessing;
public class PreProcessing  {
    // The main goal of this function is to read a csv file since we
    // perform the data preprocessing and applied machine learning process
    public static IDictionary<string,List<string?>> ReadCSV(string csv) {
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
      var result = new Dictionary<string,List<string?>>();
      foreach(var head in header) {
          result[head] = new List<string?>();
      }
      while(_csv.Read()) {
          foreach(var head in header) {
              var record = _csv.GetField(head);
              result[head].Add(string.IsNullOrWhiteSpace(record) ? null : record);
          }
      }
      return result;
      }
    // The function is used to get the  number of null present in the dictionary
      public static void GetNullSum(IDictionary<string,List<string?>> input) {
          Console.WriteLine("ColumnName:Count");
          foreach(var key in input.Keys) {
              List<string?> values = input[key];
              int nullcount = values.Count(v => v == null || string.IsNullOrWhiteSpace(v));
              Console.WriteLine($"{key}:{nullcount}");

          }
      }
    // THe functoion used to drop the row that contains null value
    public static IDictionary<string,List<string>> DropRow(IDictionary<string,List<string?>> input) {
         int rowcount = input.First().Value.Count;
         var notNull = Enumerable.Range(0,rowcount)
             .Where(i => input.All(col => col.Value[i] != null ));
         return input.ToDictionary (
             col => col.Key,
             col => notNull.Select(i => col.Value[i]).ToList()
         );
    }
    // This function used to drop the entire column that contains null value
}
