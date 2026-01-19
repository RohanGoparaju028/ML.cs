using System;
using System.Linq;
using Microsoft.Data.Analysis;
namespace ML.cs.DataPreprocessing.FeatureScaling;
public class FeatureScaling {
    private double Diviation(PrimitiveDataFrameColumn<double> x,double mean){
        int n = x.Rows.Count;
        double std = 0.0;
        foreach(var datapoint in x) {
            std += Math.Pow(datapoint - mean,2);
        }
        return Math.Sqrt(std/n);
    }
    public PrimitiveDataFrameColumn<double> ZScore(PrimitiveDataFrameColumn<double> x) {
        var mean = x.Average();
        var std = Diviation(x,mean);
        PrimitiveDataFrameColumn<double> z = new PrimitiveDataFrameColumn<double>();
        foreach(var col in x) {
            z[col] = (col.Value - mean) / std;
        }
        return z;
    }
}
