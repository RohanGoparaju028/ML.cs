using System;
using Microsoft.Data.Analysis;
namespace ML.cs.ModelEvaluation.MSE;
public class MSE {
    public double MSE_Score(PrimitiveDataFrameColumn<double> y_test,PrimitiveDataFrameColumn<double> y_pred) {
        double sum = 0.0;
        for(int i=0;i<y_test.Length;i++) {
            double test = y_test[i]!.Value - y_pred[i]!.Value;
            sum += Math.Pow(test,2.0);
        }
        return sum / y_test.Length;
    }
    public double RMSE_Score(PrimitiveDataFrameColumn<double> y_test,PrimitiveDataFrameColumn<double> y_pred) {
        double mse = MSE_Score(y_test,y_pred);
        return Math.Sqrt(mse);
    }
}
