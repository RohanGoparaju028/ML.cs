# ML.cs
ML.cs is a C# library designed to provide a simple way to create and build machine learning models. This project is not intended to replace Microsoft's ML.NET, but instead provides an alternative for machine learning in .NET

The name is inspired by d3.js, but has no connection with it.
---

## Current Status

The library is currently in the preprocessing stage. Implemented functions include:

* **`readCSV`**: Reads CSV files (similar to pandas `read_csv`) and returns a `Dictionary<string, List<string?>>`, where keys are column headers and values are the corresponding data. Supports `null` values.
* **`getNullSum`**: Returns the number of null values present in each column.
* **`dropRow`**: Returns a dictionary after dropping rows containing null values.

### Planned preprocessing functions

* Drop columns that contain null values.
* Replace null values with the mean for numeric columns.

## Planned Algorithms

The initial focus will be on implementing the following basic but important algorithms:

### Supervised Learning

1. Linear Regression
2. Logistic Regression
3. Naive Bayes
4. Exact Bayes
5. Support Vector Machine (SVM)
6. Random Forest

### Unsupervised Learning
1. K-Means
2. Hierarchical Clustering
3. DBSCAN
Future versions** may include deep learning models.

## Contribution Guidelines

If you want to contribute:
1. Clone the repository.
2. Create a folder named `Algorithm`, then two subfolders:
   * `supervised`
   * `unsupervised`
3. Implement the algorithms in their respective folders.
4. Be **kind and respectful** towards other contributors.

Notes
1) Currently, there is **no plan to integrate with other .NET libraries** such as ASP.NET Core or Azure. This may change if the project grows.

