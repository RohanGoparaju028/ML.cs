using Microsoft.Data.Analysis;

namespace ML.cs.ModelEvaluation;

public class Precision
{
  public double Precision_Score(PrimitiveDataFrameColumn<double> y_test,PrimitiveDataFrameColumn<double> y_pred)
    {
      double tp = 0;
      double tn = 0;
      double fp = 0;
      double fn = 0;
      for(int i=0;i<y_test.Length;i++)
      {
         double actual = y_test[i]!.Value;
         double predicted = y_pred[i]!.Value;
         if(actual == 1 && predicted == 1)
         {
                 tp++;
         }
         else if(actual == 1 && predicted == 0)
         {
             fn++;
         }
         else if(actual == 0 && predicted == 1)
         {
             fp++;
         }
         else if(actual == 0 && predicted == 0)
          {
              tn++;
          }
      }
      if(tp == 0.0d && fp == 0.0d)
      {
          return 0.0d;
      }
      return Math.Round(tp/(tp+fp),2);
  }
}
