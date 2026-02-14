using System;
using System.Linq;
using Microsoft.Data.Analysis;
namespace ML.cs.ModelEvaluation;
public class R2 {
    public  double R2_Score(PrimitiveDataFrameColumn<double> y_test,PrimitiveDataFrameColumn<double> y_pred) {
        var y_mean = y_test.Average()!.Value;
        double ss_res = 0.0d;
        double ss_total = 0.0;
        for(int i=0;i<y_test.Length;i++) {
            double residual_diff = y_test[i]!.Value - y_pred[i]!.Value;
            double total_diff =  y_test[i]!.Value - y_mean;
            ss_res += residual_diff * residual_diff;
            ss_total += total_diff * total_diff;
        }
        return 1 - ss_res/ss_total;
    }
}
