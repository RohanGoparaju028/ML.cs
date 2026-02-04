using System;
using Microsoft.Data.Analysis;
namespace ML.cs.ModelEvaluation.Accuracy;
public class Accuracy {
  public double Accuracy_Score(PrimitiveDataFrameColumn<double> y_test, PrimitiveDataFrameColumn<double> y_pred, double tolerance = 1e-2){
    double correct_predictions = 0;
    for (var i = 0; i < y_test.Length; i++){
        if (Math.Abs(y_test[i]!.Value - y_pred[i]!.Value) <= tolerance)
            correct_predictions++;
    }
    return correct_predictions / (double)y_test.Length;
}

}
