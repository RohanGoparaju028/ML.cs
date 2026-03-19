using System;
using Microsoft.Data.Analysis;

namespace ML.cs.Algorithms.Unsupervised.KMeans;

public class KMeans
{
    private int _k;
    private double[][] _centroids;
    private int _maxIterations;

    public KMeans(int k, int maxIterations = 1000)
    {
        _k = k;
        _maxIterations = maxIterations;
    }

    private double[][] KMeanPlusPlus(double[][] X)
    {
        int n = X.Length;
        double[][] cluster = new double[_k][];
        Random rand = new Random();

        int firstIndex = rand.Next(n);
        cluster[0] = (double[])X[firstIndex].Clone();

        double[] distance = new double[n];
        for (int c = 1; c < _k; c++)
        {
            double totalDistance = 0;
            for (int i = 0; i < n; i++)
            {
                double minDist = double.MaxValue;
                for (int j = 0; j < c; j++)
                {
                    double dist = EuclideanDistance(X[i], cluster[j]);
                    if (dist < minDist)
                        minDist = dist;
                }
                distance[i] = minDist * minDist;
                totalDistance += distance[i];
            }

            double r = rand.NextDouble() * totalDistance;
            double cumulative = 0;
            for (int i = 0; i < n; i++)
            {
                cumulative += distance[i];
                if (cumulative >= r)
                {
                    cluster[c] = (double[])X[i].Clone();
                    break;
                }
            }
        }
        return cluster;
    }

    private int[] AssignClusters(double[][] data)
    {
        int[] assigncluster = new int[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            double minDist = double.MaxValue;
            int bestCluster = 0;
            for (int j = 0; j < _k; j++)
            {
                double currentDist = EuclideanDistance(data[i], _centroids[j]);
                if (currentDist < minDist)
                {
                    minDist = currentDist;
                    bestCluster = j;
                }
            }
            assigncluster[i] = bestCluster;
        }
        return assigncluster;
    }

    private double EuclideanDistance(double[] x, double[] cluster)
    {
        int length = x.Length;
        double sum = 0;
        for (int i = 0; i < length; i++)
        {
            sum += Math.Pow(x[i] - cluster[i], 2);
        }
        return Math.Sqrt(sum);
    }

    private double[][] ConvertDataFrameToDoubleArray(DataFrame X)
    {
        int rows = (int)X.Rows.Count;
        int cols = (int)X.Columns.Count;
        double[][] data = new double[rows][];
        for (int i = 0; i < rows; i++)
        {
            data[i] = new double[cols];
            for (int j = 0; j < cols; j++)
            {
                data[i][j] = Convert.ToDouble(X.Columns[j][i]);
            }
        }
        return data;
    }

    private double[][] UpdateCentroids(double[][] data, int[] assignments)
    {
        int dims = data[0].Length;
        double[][] newCentroids = new double[_k][];
        int[] counts = new int[_k];

        for (int i = 0; i < _k; i++)
            newCentroids[i] = new double[dims];

        for (int i = 0; i < data.Length; i++)
        {
            int clusterIdx = assignments[i];
            for (int d = 0; d < dims; d++)
            {
                newCentroids[clusterIdx][d] += data[i][d];
            }
            counts[clusterIdx]++;
        }

        for (int i = 0; i < _k; i++)
        {
            if (counts[i] == 0)
                continue;
            for (int d = 0; d < dims; d++)
            {
                newCentroids[i][d] /= counts[i];
            }
        }
        return newCentroids;
    }

    private bool HasConverged(double[][] oldC, double[][] newC)
    {
        for (int i = 0; i < _k; i++)
        {
            if (EuclideanDistance(oldC[i], newC[i]) > 1e-6)
                return false;
        }
        return true;
    }

    public double CalculateInertia(DataFrame X)
    {
        double[][] data = ConvertDataFrameToDoubleArray(X);
        double totalDistance = 0;
        for (int i = 0; i < data.Length; i++)
        {
            double minDist = double.MaxValue;
            foreach (var centroid in _centroids)
            {
                double d = EuclideanDistance(data[i], centroid);
                if (d < minDist)
                    minDist = d;
            }
            totalDistance += Math.Pow(minDist, 2);
        }
        return Math.Round(totalDistance, 2);
    }

    public void Fit(DataFrame X)
    {
        double[][] data = ConvertDataFrameToDoubleArray(X);
        _centroids = KMeanPlusPlus(data);

        for (int iter = 0; iter < _maxIterations; iter++)
        {
            int[] assignments = AssignClusters(data);
            double[][] newCentroids = UpdateCentroids(data, assignments);
            if (HasConverged(_centroids, newCentroids))
                break;

            _centroids = newCentroids;
        }
    }

    public Int32DataFrameColumn Predict(DataFrame X)
    {
        double[][] data = ConvertDataFrameToDoubleArray(X);
        int[] assigncluster = AssignClusters(data);
        return new Int32DataFrameColumn("Clusters", assigncluster.Select(x => (int)x));
    }
}
