using System;
using Microsoft.Data.Analysis;

namespace ML.cs.ModelEvaluation.MAE;

public class MAE
{
    public double MAE_Score(
        PrimitiveDataFrameColumn<double> y_test,
        PrimitiveDataFrameColumn<double> y_pred
    )
    {
        double sum = 0.0;
        for (int i = 0; i < y_test.Length; i++)
        {
            sum += Math.Abs(y_test[i]!.Value - y_pred[i]!.Value);
        }
        return sum / (double)y_test.Length;
    }
}
