using Microsoft.Data.Analysis;
namespace ML.cs.Algorithms.Supervised.LinearRegression;
public class LinearRegression {
    private  double learningrate;
    private  int iterations;
    private  double tolerance;
    private double[] wts;
    private  double bias;
    public LinearRegression() {
        this.learningrate = 0.01;
        this.iterations = 1000;
        this.tolerance = 1e-6;
    }
    public LinearRegression(int iterations,double learningrate,double tolerance) {
        this.learningrate = learningrate;
        this.iterations = iterations;
        this.tolerance = tolerance;
    }
     private  double MSE(DataFrame x,PrimitiveDataFrameColumn<double> y) {
         int n = (int)x.Rows.Count;
         double loss = 0.0;
         for(int i=0;i<n;i++) {
             double yPred = bias;
             for(int j=0;j<x.Columns.Count;j++) {
                 yPred += wts[j] * Convert.ToDouble(x.Columns[j][i]);
             }
             double error = yPred - y[i]!.Value;
             loss += error*error;
         }
         return loss/n;
     }
      public void Fit(DataFrame x,PrimitiveDataFrameColumn<double> y) {
        if(x.Rows.Count != y.Length) {
            throw new ArgumentException("Dependent and independent features are of different lengths please fix the problem");
        }
        foreach(var col in x.Columns) {
            if(col.NullCount > 0) {
                throw new ArgumentException("Independent variable contains null values");
            }
        }
        if(y.NullCount > 0) {
            throw new Exception("Dependent variable contains null");
        }
        int numberOfSamples = (int)x.Rows.Count;
        int numberOfFeatures  = (int)x.Columns.Count;
        wts = new double[numberOfFeatures];
        bias = 0.0;
        double prevloss = double.MaxValue;
        for(int iter = 0;iter < iterations;iter++) {
            var dw = new double[numberOfFeatures];
            var dc = 0.0d;
            for(int i=0;i<numberOfSamples;i++) {
               var yPred = bias;
               for(int j=0;j<numberOfFeatures;j++) {
                   yPred += wts[j] * Convert.ToDouble(x.Columns[j][i]);
               }
               double error = yPred - y[i]!.Value;
               for(int j=0;j<numberOfFeatures;j++) {
                   dw[j] += error*Convert.ToDouble(x.Columns[j][i]);
               }
               dc += error;
               }
               for(int j=0;j<numberOfFeatures;j++) {
                   wts[j] -= learningrate*dw[j]/numberOfSamples;
               }
               bias -= learningrate*dc/numberOfSamples;
               double loss  = MSE(x,y);
               if(Math.Abs(prevloss - loss) < tolerance) {
                   break;
               }
               prevloss = loss;
        }
    }
   public PrimitiveDataFrameColumn<double>  Predict(DataFrame x) {
       if(wts == null) {
        throw new Exception("The model is not fitted yet plese fit and try again");
       }
       int numberOfSample = (int)x.Rows.Count;
       int numberOfPredictions = (int)x.Columns.Count;
       if(numberOfPredictions != wts.Length) {
           throw new ArgumentException("The trained model and predicted model contains different featrure length");
       }
       foreach(var col in x.Columns) {
           if(col.NullCount > 0) {
               throw new Exception("Contains null values");
           }
       }
       PrimitiveDataFrameColumn<double> pred = new("Predictios",numberOfSample);
       for(int i=0;i<numberOfSample;i++) {
           double yPred = bias;
           for(int j=0;j<numberOfPredictions;j++) {
               yPred += wts[j] * Convert.ToDouble(x.Columns[j][i]);
           }
           pred[i] = yPred;
       }
       return pred;
   }
}
