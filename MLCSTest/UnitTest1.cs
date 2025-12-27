using System;
using Xunit;
using ML.cs.DataPreprocessing;
using System.Collections.Generic;

namespace Example;

public class TestCases
{
    [Fact]
    public void Test_readCSV()
    {
        string file = "Test/test.csv";
        var data = MX.readCSV(file); // or NullHandling.ReadCsv(file)

        // Print headers
        foreach (var key in data.Keys)
        {
            Console.Write(key + "\t");
        }
        Console.WriteLine();

        // Determine number of rows
        int rowCount = 0;
        foreach (var col in data.Values)
        {
            rowCount = col.Count;
            break;
        }

        // Print each row
        for (int i = 0; i < rowCount; i++)
        {
            foreach (var key in data.Keys)
            {
                var value = data[key][i];
                Console.Write((value ?? "NULL") + "\t");
            }
            Console.WriteLine();
        }
        MX.getNullSum(data);
        data = MX.DropRow(data);
        MX.getNullSum(data);
    }
}
