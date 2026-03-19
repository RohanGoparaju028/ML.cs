using Microsoft.Data.Analysis;
using ML.cs.ModelEvaluation;
using ML.cs.ModelEvaluation;

namespace ML.cs.ModelEvaluation;

public class F1Score
{
    internal double precision;
    internal double recall;

    public double F1_Score(
        PrimitiveDataFrameColumn<double> y_test,
        PrimitiveDataFrameColumn<double> y_pred
    )
    {
        Precision p = new();
        Recall r = new();
        precision = p.Precision_Score(y_test, y_pred);
        recall = r.Recall_Score(y_test, y_pred);
        if (precision == 0 && recall == 0)
        {
            return 0d;
        }
        double f1 = 2 * ((precision * recall) / (precision + recall));
        return Math.Round(f1, 2);
    }
}
