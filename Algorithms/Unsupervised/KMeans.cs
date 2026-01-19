using System;
using Microsoft.Data.Analysis;
namespace ML.cs.Unsupervised.KMeans;
public class KMeans {
    private int noOfCluster;
    private int random_state;
    public KMeans(int noOfClusters=3,int random_state=42) {
        this.noOfCluster = noOfCluster;
        this.random_state = random_state;
    }
    public void Fit() {

    }
}
