using System;
using System.IO;
using System.Linq;
using System.Globalization;
using Microsoft.Data.Analysis;
namespace ML.cs.DataPreprocessing;
public class Preprocessing  {
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
    public DataFrame DropColumn(DataFrame df,string[] col) {
        DataFrame newDf = new DataFrame();
        foreach(var column in df.Columns) {
            if(!col.Contains(column.Name)) {
                newDf.Columns.Add(column.Clone());
            }
        }
        return newDf;
    }
    public (DataFrame,DataFrame,PrimitiveDataFrameColumn<double>,PrimitiveDataFrameColumn<double>) TrainTestSplit(DataFrame X,PrimitiveDataFrameColumn<double> y,double testSize=0.8) {
        int n = (int) X.Rows.Count;
        Random rand = new();
        var indices = Enumerable.Range(0,n).OrderBy(x => rand.Next()).ToList();
        int testCount = (int) (n * testSize);
        int trainCount = n - testCount;
        var trainIndicies = indices.Take(trainCount).Select(i => (long)i);
        var testIndicies = indices.Skip(trainCount).Select(i => (long)i);
        DataFrame X_Train = X[trainIndicies];
        DataFrame X_Test = X[testIndicies];
        PrimitiveDataFrameColumn<double> y_train = new(y.Name,trainCount);
        PrimitiveDataFrameColumn<double> y_test = new(y.Name,testCount);
        var trainIdx = 0;
        var testIdx = 0;
        foreach(long i in trainIndicies) {
            y_train[trainIdx++] = y[i];
        }
        foreach(long i in testIndicies) {
            y_test[testIdx++] = y[i];
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
