using System;
using Microsoft.Data.Analysis;

namespace ML.cs.Algorithms.Supervised.LogisticRegression;

public class LogisticRegression
{
    internal string solver;
    internal const double threshold = 0.5;
    internal double[] wts;
    internal double bias;
    internal int max_iterations;
    internal double tolerance;
    internal double learning_rate;

    public LogisticRegression(
        string solver = "binomial",
        int max_iteration = 10000,
        double tolerance = 1e-6,
        double learning_rate = 0.001)
    {
        this.solver = solver;
        this.max_iterations = max_iteration;
        this.tolerance = tolerance;
        this.learning_rate = learning_rate;
    }

    internal double Sigmoid(double z)
        => 1.0 / (1.0 + Math.Exp(-z));

    internal double LogLoss(DataFrame X, PrimitiveDataFrameColumn<double> y)
    {
        int n = (int)X.Rows.Count;
        double total_loss = 0.0;
        double epsilon = 1e-15;

        for (int i = 0; i < n; i++)
        {
            double z = bias;

            for (int j = 0; j < wts.Length; j++)
                z += wts[j] * Convert.ToDouble(X.Columns[j][i]);

            double p = Sigmoid(z);
            p = Math.Clamp(p, epsilon, 1 - epsilon);

            double actual = y[i]!.Value;
            total_loss += -(actual * Math.Log(p) +
                            (1 - actual) * Math.Log(1 - p));
        }

        return total_loss / n;
    }

    public void Fit(DataFrame X, PrimitiveDataFrameColumn<double> y)
    {
        if (X.Rows.Count != y.Length)
            throw new Exception("Size mismatch");

        int n = (int)X.Rows.Count;
        int m = (int)X.Columns.Count;

        wts = new double[m];
        bias = 0.0;

        double prevLoss = double.MaxValue;

        for (int iter = 0; iter < max_iterations; iter++)
        {
            double[] dw = new double[m];
            double dc = 0.0;

            for (int i = 0; i < n; i++)
            {
                double z = bias;

                for (int j = 0; j < m; j++)
                    z += wts[j] * Convert.ToDouble(X.Columns[j][i]);

                double y_pred = Sigmoid(z);
                double error = y_pred - y[i]!.Value;

                for (int j = 0; j < m; j++)
                    dw[j] += error * Convert.ToDouble(X.Columns[j][i]);

                dc += error;
            }

            for (int j = 0; j < m; j++)
                wts[j] -= learning_rate * (dw[j] / n);

            bias -= learning_rate * (dc / n);

            double currLoss = LogLoss(X, y);

            if (Math.Abs(prevLoss - currLoss) < tolerance)
                break;

            prevLoss = currLoss;
        }
    }

    public void Fit(DataFrame X, PrimitiveDataFrameColumn<bool> y)
    {
        var doub_y = new PrimitiveDataFrameColumn<double>("double values", y.Length);

        for (int i = 0; i < y.Length; i++)
            doub_y[i] = y[i] == true ? 1.0 : 0.0;

        Fit(X, doub_y);
    }

    public PrimitiveDataFrameColumn<double> Predict(DataFrame X)
    {
        int n = (int)X.Rows.Count;
        var pred = new PrimitiveDataFrameColumn<double>("Predictions", n);

        for (int i = 0; i < n; i++)
        {
            double z = bias;

            for (int j = 0; j < wts.Length; j++)
                z += wts[j] * Convert.ToDouble(X.Columns[j][i]);

            pred[i] = Sigmoid(z) >= threshold ? 1.0 : 0.0;
        }

        return pred;
    }
}
