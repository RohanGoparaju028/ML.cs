using System;
using System.IO;
using System.Linq;
using Microsoft.Data.Analysis;
namespace ML.cs.DataPreprocessing;
public class PreProcessing  {
    // The main goal of this function is to read a csv file since we
    // perform the data preprocessing and applied machine learning process
    public static DataFrame ReadCSV(string csv) {
          if(!File.Exists(csv)) {
              throw new Exception("File Does not exists");
          }
          return DataFrame.LoadCsv(csv);
    }
    // The function is used to get the  number of null present in the Dataframe
      public static void GetNullSum(DataFrame df) {
          Console.WriteLine("ColumnName:Count");
          foreach(var col in df.Columns) {
              long nullcount = col.NullCount;
              Console.WriteLine($"{col.Name}:{nullcount}");
           }
      }
    //  Dropping the rows containg the null values
    public static DataFrame DropNulls(DataFrame df) {
       return df.DropNulls();
    }
    public static void FillNa(DataFrame df) {
        foreach(var column in df.Columns) {
            if(column is PrimitiveDataFrameColumn<double> col) {
               var mean = col.Mean();
               col.FillNulls(mean,inPlace:true);
            }
        }
    }
}
