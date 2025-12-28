# ML.cs
In the world of c# and dotnet we have ML.net framework to implement machine learning models but the problem for me is the style of writing the models feels sufisticated compared to writing same idea in languages like python and scikit-learn. So my main idea is to write a new machine learning library which look and feel simple to use instead of being overwhelming.
##Example 
In the world of machine learning one of the simplest algorithm is **`linear regression`** so I will provide a screenshot that I want to do for the library.
<img width="1435" height="890" alt="Screenshot 2025-12-28 at 12 04 53 PM" src="https://github.com/user-attachments/assets/ee1d5c7c-d9c3-4e61-910e-72abf0a08f31" />

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

