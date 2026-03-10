using Microsoft.Data.Analysis;
using System;

namespace ML.cs.Algorithms.Unsupervised.KMeans;

public class KMeans
{
    private int _k; // number of clusters
    private double[][] _centroids; // centroids data points
    private int _maxIterations; // maximum iterations
    public KMeans(int k,int maxIterations=1000)
    {
        _k = k;
        _maxIterations = maxIterations;
    }
    //KMean++ is a cluster initialized stategy to pick appropriate K-centroids
    //that we need to spread accross the dataframe to better represent the dataset.
    private double[][] KMeanPlusPlus(double[][] X)
    {
        int n = X.Length;
        double[][] cluster = new double[_k][];
        Random rand = new Random();
        int fistindex = rand.Next(n);
        cluster[0] = (double[]) X[fistindex].Clone();
        double[] distance = new double[n];
        for(int c=1;c<_k;c++)
        {
            double totalDistance = 0;
            for(int i=0;i<n;i++)
            {
                double minDist = double.MaxValue;
                for(int j=0;j<c;j++)
                {
                    double dist = EuclideanDistance(X[i],cluster[j]);
                    if(dist < minDist)
                    {
                        minDist = dist;
                    }
                }
                distance[i] = minDist * minDist;
                totalDistance += distance[i];
            }
            double r = rand.NextDouble() * totalDistance;
            double cumilative  = 0;
            for(int i=0;i<n;i++)
            {
                cumilative += distance[i];
                if(cumilative >= r)
                {
                    cluster[c] = (double[])data[i].Clone();
                    break;
                }
            }
        }
        return cluster;
    }
    private double EuclideanDistance(double[] x,double[] cluster)
    {
        const int rows = x.Length;
        double sum = 0;
        for(int i=0;i<rows;i++)
        {
            sum += Math.Pow(x[i] - cluster[i],2);
        }
        return Math.Sqrt(sum);
    }
    private double[][] ConvertDataFrameColumn(DataFrame X)
    {
        int rows = (int)X.Rows.Count;
        int cols = (int)X.Cols.Count;
        double[][] data = new double[rows][];
        for(int i=0;i<rows;i++)
        {
            data[i] = new double[cols];
            for(int j=0;j<cols;j++)
            {
                data[i][j] = Convert.ToDouble(X.Columns[j][i]);
            }
        }
        return data;
    }
    private double[][] UpdateCentroids()
    {
        int
    }
    public DataFrame Fit(DataFrame X)
    {
        double[][] data = ConvertDataFrameColumn(X);
        _centroid = KMeanPlusPlus(data);

    }
    public void Predict(DataFrame X)
    {
    }
}
