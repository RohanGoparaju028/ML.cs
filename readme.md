# ML.cs

A lightweight machine learning library written in **C#**, built from scratch to help .NET developers understand how ML algorithms work under the hood. Inspired by the simplicity of [d3.js](https://d3js.org/), ML.cs aims to be an accessible starting point for .NET devs who want to bring machine learning into their applications without leaving the C# ecosystem.

> ⚠️ **Pre-release:** ML.cs has not yet reached v1. The API may change between versions. Contributions and feedback are welcome.

---

## Table of Contents

- [Why ML.cs?](#why-mlcs)
- [Requirements](#requirements)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Usage Guide](#usage-guide)
  - [Data Preprocessing](#data-preprocessing)
  - [Feature Scaling](#feature-scaling)
  - [Algorithms — Supervised](#algorithms--supervised)
  - [Algorithms — Unsupervised](#algorithms--unsupervised)
  - [Model Evaluation](#model-evaluation)
- [Full Example](#full-example)
- [License](#license)

---

## Why ML.cs?

While C# has powerful ML tooling (ML.NET, TensorFlow.NET, etc.), they can be complex for beginners. ML.cs fills the gap by:

- Implementing algorithms **from scratch** so you can see exactly how they work
- Offering a **simple, intuitive API** that feels familiar to Python ML users
- Being a **pure learning resource** — great for students and .NET devs exploring ML for the first time

---

## Requirements

| Requirement | Version |
|---|---|
| .NET | 10.0+ |
| Microsoft.Data.Analysis | 0.23.0 |

---

## Installation

Clone the repository and add it as a reference to your project:

```bash
git clone https://github.com/your-username/ML.cs.git
```

Then reference the project in your `.csproj`:

```xml
<ProjectReference Include="../ML.cs/ML.cs.csproj" />
```

---

## Project Structure

```
ML.cs/
├── Algorithms/
│   ├── Supervised/
│   │   ├── LinearRegression.cs
│   │   ├── LogisticRegression.cs
│   │   └── NaiveBayers.cs          # (in progress)
│   └── Unsupervised/
│       └── KMeans.cs
├── DataPreprocessing/
│   ├── PreProcessing.cs
│   ├── FeatureScaling.cs
│   ├── Accuracy.cs
│   ├── F1Score.cs
│   ├── MAE.cs
│   ├── MSE.cs
│   ├── Precision.cs
│   ├── R2Score.cs
│   └── Recall.cs
└── ML.cs.csproj
```

---

## Usage Guide

### Data Preprocessing

**Namespace:** `ML.cs.DataPreprocessing`

```csharp
using ML.cs.DataPreprocessing;

var pp = new Preprocessing();
```

#### `ReadCsv(string path)`
Loads a CSV file into a `DataFrame`. The CSV must have a header row.

```csharp
DataFrame df = pp.ReadCsv("data/housing.csv");
```

#### `GetNullSum(DataFrame df)`
Prints the null count for every column to the console.

```csharp
pp.GetNullSum(df);
// Output:
// ColumnName:Count
// Age:3
// Salary:0
```

#### `DropNulls(DataFrame df)`
Returns a new `DataFrame` with all rows containing null values removed.

```csharp
DataFrame clean = pp.DropNulls(df);
```

#### `FillNa(DataFrame df)`
Fills null values in numeric columns with the **column mean** (in place). Supports `double` and `float` columns.

```csharp
pp.FillNa(df);
```

#### `DropColumns(DataFrame df, string[] columns)`
Returns a new `DataFrame` with the specified columns removed.

```csharp
DataFrame trimmed = pp.DropColumns(df, new[] { "Id", "Name" });
```

#### `TrainTestSplit(DataFrame X, PrimitiveDataFrameColumn<double> y, double testSize = 0.2)`
Randomly splits features and labels into training and test sets. Returns a tuple `(X_Train, X_Test, y_train, y_test)`.

```csharp
var (X_Train, X_Test, y_train, y_test) = pp.TrainTestSplit(X, y, testSize: 0.2);
```

| Parameter | Type | Default | Description |
|---|---|---|---|
| `X` | `DataFrame` | — | Feature matrix |
| `y` | `PrimitiveDataFrameColumn<double>` | — | Target labels |
| `testSize` | `double` | `0.2` | Fraction of data to use for testing (0–1) |

---

### Feature Scaling

**Namespace:** `ML.cs.DataPreprocessing.Normalization`

Scaling features before training significantly improves gradient-descent-based algorithms. ML.cs provides two scalers.

#### `ZScore` — Standardization

Transforms a column to have **mean = 0** and **standard deviation = 1**.

```csharp
using ML.cs.DataPreprocessing.Normalization;

var scaler = new ZScore();
var scaledColumn = scaler.Normalize(df["Age"] as PrimitiveDataFrameColumn<double>);
```

#### `MinMax` — Min-Max Normalization

Scales a column to the **[0, 1]** range.

```csharp
var scaler = new MinMax();
var scaledColumn = scaler.Normalize(df["Salary"] as PrimitiveDataFrameColumn<double>);
```

> **Note:** Both scalers mutate the column in place and also return it for convenience. Throws an exception if min == max.

---

### Algorithms — Supervised

#### Linear Regression

**Namespace:** `ML.cs.Algorithms.Supervised.LinearRegression`

Implements gradient descent with **L2 regularization (Ridge)**. Stops early when the loss improvement drops below the tolerance threshold.

```csharp
using ML.cs.Algorithms.Supervised.LinearRegression;

var model = new LinearRegression(
    iterations: 1000,
    learningrate: 0.01,
    tolerance: 1e-6,
    lambda: 0.01       // L2 regularization strength; set to 0 to disable
);
```

| Parameter | Type | Description |
|---|---|---|
| `iterations` | `int` | Maximum number of gradient descent steps |
| `learningrate` | `double` | Step size for weight updates |
| `tolerance` | `double` | Early-stopping threshold on loss improvement |
| `lambda` | `double` | L2 regularization coefficient |

**`.Fit(DataFrame X, PrimitiveDataFrameColumn<double> y)`**
Trains the model. Throws if X and y have different lengths or contain null values.

```csharp
model.Fit(X_Train, y_train);
```

**`.Predict(DataFrame X)`**
Returns a `PrimitiveDataFrameColumn<double>` of predictions.

```csharp
var predictions = model.Predict(X_Test);
```

---

#### Logistic Regression

**Namespace:** `ML.cs.Algorithms.Supervised.LogisticRegression`

Binary classifier using the **sigmoid function** and **log-loss** minimization via gradient descent. Supports both `double` and `bool` label columns.

```csharp
using ML.cs.Algorithms.Supervised.LogisticRegression;

var model = new LogisticRegression(
    solver: "binomial",      // currently "binomial" only
    max_iteration: 10000,
    tolerance: 1e-6,
    learning_rate: 0.001
);
```

| Parameter | Type | Default | Description |
|---|---|---|---|
| `solver` | `string` | `"binomial"` | Solver type (binary classification) |
| `max_iteration` | `int` | `10000` | Max gradient descent steps |
| `tolerance` | `double` | `1e-6` | Early-stopping threshold |
| `learning_rate` | `double` | `0.001` | Step size for weight updates |

**`.Fit(DataFrame X, PrimitiveDataFrameColumn<double> y)`**
Train with numeric labels (`0.0` / `1.0`).

**`.Fit(DataFrame X, PrimitiveDataFrameColumn<bool> y)`**
Train with boolean labels — automatically converts `true → 1.0`, `false → 0.0`.

**`.Predict(DataFrame X)`**
Returns a `PrimitiveDataFrameColumn<double>` where values are `0.0` or `1.0` (threshold = 0.5).

```csharp
model.Fit(X_Train, y_train);
var predictions = model.Predict(X_Test);
```

---

### Algorithms — Unsupervised

#### K-Means Clustering

**Namespace:** `ML.cs.Algorithms.Unsupervised.KMeans`

Clusters data into `k` groups using **K-Means++ initialization** and Euclidean distance. Converges when centroids move less than `1e-6` between iterations.

```csharp
using ML.cs.Algorithms.Unsupervised.KMeans;

var model = new KMeans(k: 3, maxIterations: 1000);
```

| Parameter | Type | Default | Description |
|---|---|---|---|
| `k` | `int` | — | Number of clusters |
| `maxIterations` | `int` | `1000` | Max iterations before stopping |

**`.Fit(DataFrame X)`**
Trains the model and stores learned centroids.

```csharp
model.Fit(X);
```

**`.Predict(DataFrame X)`**
Returns an `Int32DataFrameColumn` with the cluster index (`0` to `k-1`) for each row.

```csharp
var clusters = model.Predict(X);
```

**`.CalculateInertia(DataFrame X)`**
Returns the **within-cluster sum of squared distances** (inertia). Useful for the Elbow Method when choosing `k`.

```csharp
double inertia = model.CalculateInertia(X);
Console.WriteLine($"Inertia: {inertia}");
```

---

### Model Evaluation

#### Classification Metrics

**Namespace:** `ML.cs.ModelEvaluation`

All classification metrics take `y_test` and `y_pred` as `PrimitiveDataFrameColumn<double>` where values are `0.0` or `1.0`.

| Class | Method | Returns | Description |
|---|---|---|---|
| `Accuracy` | `Accuracy_Score(y_test, y_pred, tolerance)` | `double` | Fraction of correct predictions |
| `Precision` | `Precision_Score(y_test, y_pred)` | `double` | TP / (TP + FP), rounded to 2 decimal places |
| `Recall` | `Recall_Score(y_test, y_pred)` | `double` | TP / (TP + FN), rounded to 2 decimal places |
| `F1Score` | `F1_Score(y_test, y_pred)` | `double` | Harmonic mean of precision and recall |

```csharp
using ML.cs.ModelEvaluation;

var acc  = new Accuracy().Accuracy_Score(y_test, predictions);
var prec = new Precision().Precision_Score(y_test, predictions);
var rec  = new Recall().Recall_Score(y_test, predictions);
var f1   = new F1Score().F1_Score(y_test, predictions);

Console.WriteLine($"Accuracy:  {acc:P2}");
Console.WriteLine($"Precision: {prec}");
Console.WriteLine($"Recall:    {rec}");
Console.WriteLine($"F1 Score:  {f1}");
```

---

#### Regression Metrics

| Namespace | Class | Method | Returns | Description |
|---|---|---|---|---|
| `ML.cs.ModelEvaluation.MSE` | `MSE` | `MSE_Score(y_test, y_pred)` | `double` | Mean Squared Error |
| `ML.cs.ModelEvaluation.MSE` | `MSE` | `RMSE_Score(y_test, y_pred)` | `double` | Root Mean Squared Error |
| `ML.cs.ModelEvaluation.MAE` | `MAE` | `MAE_Score(y_test, y_pred)` | `double` | Mean Absolute Error |
| `ML.cs.ModelEvaluation` | `R2` | `R2_Score(y_test, y_pred)` | `double` | R² coefficient of determination |

```csharp
using ML.cs.ModelEvaluation.MSE;
using ML.cs.ModelEvaluation.MAE;
using ML.cs.ModelEvaluation;

double mse  = new MSE().MSE_Score(y_test, predictions);
double rmse = new MSE().RMSE_Score(y_test, predictions);
double mae  = new MAE().MAE_Score(y_test, predictions);
double r2   = new R2().R2_Score(y_test, predictions);

Console.WriteLine($"MSE:  {mse:F4}");
Console.WriteLine($"RMSE: {rmse:F4}");
Console.WriteLine($"MAE:  {mae:F4}");
Console.WriteLine($"R²:   {r2:F4}");
```

---

## Full Example

Below is an end-to-end example training a Logistic Regression classifier on a CSV dataset.

```csharp
using ML.cs.DataPreprocessing;
using ML.cs.DataPreprocessing.Normalization;
using ML.cs.Algorithms.Supervised.LogisticRegression;
using ML.cs.ModelEvaluation;

// 1. Load data
var pp = new Preprocessing();
DataFrame df = pp.ReadCsv("data/titanic.csv");

// 2. Inspect and clean
pp.GetNullSum(df);
df = pp.DropNulls(df);
df = pp.DropColumns(df, new[] { "Name", "Ticket", "Cabin" });

// 3. Separate features and labels
var y = df["Survived"] as PrimitiveDataFrameColumn<double>;
var X = pp.DropColumns(df, new[] { "Survived" });

// 4. Scale features
var ageCol  = X["Age"]  as PrimitiveDataFrameColumn<double>;
var fareCol = X["Fare"] as PrimitiveDataFrameColumn<double>;
new ZScore().Normalize(ageCol);
new ZScore().Normalize(fareCol);

// 5. Split
var (X_Train, X_Test, y_train, y_test) = pp.TrainTestSplit(X, y, testSize: 0.2);

// 6. Train
var model = new LogisticRegression(max_iteration: 5000, learning_rate: 0.01);
model.Fit(X_Train, y_train);

// 7. Predict & Evaluate
var predictions = model.Predict(X_Test);

Console.WriteLine($"Accuracy:  {new Accuracy().Accuracy_Score(y_test, predictions):P2}");
Console.WriteLine($"Precision: {new Precision().Precision_Score(y_test, predictions)}");
Console.WriteLine($"Recall:    {new Recall().Recall_Score(y_test, predictions)}");
Console.WriteLine($"F1 Score:  {new F1Score().F1_Score(y_test, predictions)}");
```

---

## License

This project is licensed under the **MIT License** — see [LICENSE](./LICENSE) for details.
