using Microsoft.Data.Analysis;

namespace ML.cs.ModelEvaluation;

public class Recall
{
    public double Recall_Score(PrimitiveDataFrameColumn<double> y_test,PrimitiveDataFrameColumn<double> y_pred)
    {
        double tp = 0.0;
        double tn = 0.0;
        double fp = 0.0;
        double fn = 0.0;
        for(int i=0;i<y_test.Length;i++)
        {
            double actual = y_test[i]!.Value;
            double pred = y_pred[i]!.Value;
            if(actual == 1 && pred == 1)
            {
                tp++;
            }
            else if(actual == 1 && pred == 0)
            {
                fn++;
            }
            else if(actual == 0 && pred == 1)
            {
                fp++;
            }
            else if(actual == 0 && pred == 0)
            {
                tn++;
            }
        }
        if(tp == 0 && fn == 0)
        {
            return 0d;
        }
        return Math.Round(tp/(tp+fn),2);
    }
}
