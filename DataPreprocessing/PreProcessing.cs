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
    public static (DataFrame,DataFrame,PrimitiveDataFrameColumn<double>,PrimitiveDataFrameColumn<double>) TrainTestSplit(DataFrame df,string targetColumn,double testsize=0.8) {
     int noOfRows = (int)df.Rows.Count;
     int testDataSetSize = (int) (noOfRows * testsize);
     int trainDataSetSize = noOfRows - testDataSetSize;
     var targetFeatureIndex = df.Columns.IndexOf(targetColumn);
     var x = df.Clone();
     x.Columns.RemoveAt(targetFeatureIndex);
     var originalTarget = df.Columns[targetColumn];
     PrimitiveDataFrameColumn<double> y = new PrimitiveDataFrameColumn<double>(targetColumn, noOfRows);

    for (long i = 0; i < noOfRows; i++)
    {
        y[i] = Convert.ToDouble(originalTarget[i]);
    }
     var trainIndicies = Enumerable.Range(0,trainDataSetSize).Select(i => (long)i);
     var testIndicies = Enumerable.Range(trainDataSetSize,testDataSetSize).Select(i => (long)i);
     DataFrame X_Train = x[trainIndicies];
     DataFrame X_Test = x[testIndicies];
     PrimitiveDataFrameColumn<double> y_train = new PrimitiveDataFrameColumn<double>(targetColumn,trainDataSetSize);
     for(int i=0;i<trainDataSetSize;i++) {
         y_train[i] = y[i];
     }
     PrimitiveDataFrameColumn<double> y_test = new PrimitiveDataFrameColumn<double>(targetColumn,testDataSetSize);
     for(int i=0;i<testDataSetSize;i++) {
         y_test[i] = y[i];
     }
     return (X_Train,X_Test,y_train,y_test);
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
