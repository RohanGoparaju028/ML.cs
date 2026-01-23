using System;
using System.Linq;
using Microsoft.Data.Analysis;
namespace ML.cs.DataPreprocessing.Normalization;
public class ZScore {
    private double Mean;
    private double Std;
    public PrimitiveDataFrameColumn<double> Normalize(PrimitiveDataFrameColumn<double> x)  {
        Mean = x.ToList().Average() ?? 0.0;
        Std = StandardDeviation(x,Mean);
        for(long i=0;i<x.Length;i++) {
            x[i] = (x[i].Value - Mean) / Std;
        }
        return x;
    }
    private double StandardDeviation(PrimitiveDataFrameColumn<double> x,double mean) {
        var std = x.Select(item => Math.Pow(item.Value - mean,2)).Sum();
        return Math.Sqrt((double)std/x.Length);
    }
    }
public class MinMax {
    private double min;
    private double max;
    public PrimitiveDataFrameColumn<double> Normalize(PrimitiveDataFrameColumn<double> x) {
        double min = (double)x.Min();
        double max =  (double)x.Max();
        if(min == max) {
            throw new Exception("Min and Maximum element cannot be of same value there is a problem with this please fix this issue");
        }
        for(long i=0;i<x.Length;i++) {
            x[i] = (x[i] - min) / (max - min);
        }
        return x;
    }
}
