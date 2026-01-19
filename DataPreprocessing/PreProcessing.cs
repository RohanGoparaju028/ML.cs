using System;
using System.IO;
using System.Linq;
using System.Globalization;
using Microsoft.Data.Analysis;
namespace ML.cs.DataPreprocessing;
public class PreProcessing  {
    // The main goal of this function is to read a csv file since we
    // perform the data preprocessing and applied machine learning process
    public  DataFrame ReadCSV(string csv) {
     if (!File.Exists(csv))
        throw new FileNotFoundException($"CSV file does not exist: {csv}");
       return DataFrame.LoadCsv(csv,header:true,dataTypes:default);
    }
    // The function is used to get the  number of null present in the Dataframe
      public void GetNullSum(DataFrame df) {
          Console.WriteLine("ColumnName:Count");
          foreach(var col in df.Columns) {
              long nullcount = col.NullCount;
              Console.WriteLine($"{col.Name}:{nullcount}");
           }
      }
    //  Dropping the rows containg the null values
    public  DataFrame DropNulls(DataFrame df) {
       return df.DropNulls();
    }
    public DataFrame DropColumn(DataFrame df,string col) {
        DataFrame newDf = df.Clone();
        newDf.Remove(col);
        return newDf;
    }
    public (DataFrame,DataFrame,PrimitiveDataFrameColumn<double>,PrimitiveDataFrameColumn<double>) TrainTestSplit(DataFrame df,PrimitiveDataFrameColumn<double> y,double testsize=0.8) {
     int noOfRows = (int)df.Rows.Count;
     int testDataSetSize = (int) (noOfRows * testsize);
     int trainDataSetSize = noOfRows - testDataSetSize;
     var x = df.Clone();
     var targetColumn = y.Name;
     PrimitiveDataFrameColumn<double> newY = new PrimitiveDataFrameColumn<double>(targetColumn,noOfRows);

    for (long i = 0; i < noOfRows; i++)
    {
        newY[i] = y[i];
    }
     var trainIndicies = Enumerable.Range(0,trainDataSetSize).Select(i => (long)i);
     var testIndicies = Enumerable.Range(trainDataSetSize,testDataSetSize).Select(i => (long)i);
     DataFrame X_Train = x[trainIndicies];
     DataFrame X_Test = x[testIndicies];
     PrimitiveDataFrameColumn<double> y_train = new PrimitiveDataFrameColumn<double>(targetColumn,trainDataSetSize);
     for(int i=0;i<trainDataSetSize;i++) {
         y_train[i] = newY[i];
     }
     PrimitiveDataFrameColumn<double> y_test = new PrimitiveDataFrameColumn<double>(targetColumn,testDataSetSize);
     for(int i=0;i<testDataSetSize;i++) {
         y_test[i] = newYy[i];
     }
     return (X_Train,X_Test,y_train,y_test);
    }
    public  void FillNa(DataFrame df) {
        foreach(var column in df.Columns) {
            if(column is PrimitiveDataFrameColumn<double> col) {
               var mean = col.Mean();
               col.FillNulls(mean,inPlace:true);
            }
        }
    }
}
