As I previously mentioned the purpose of ML.cs here are the roadmap that I want to have for my machine learning library.
Any good machine learning library should have a good preprocessing capabilites that could handles missing values by droping the rows that contains the nulls or drop the column that contains the null values or fill the null with mean of numeric columns.
The methods for doing the above mentioned preprocessing resides in the DataPreprocessing folder/Preprocessing.cs.
So before going any further I wanna mention the naming convention for methods
  1) The method name always follows c# naming convention
  2) Name should be simple enough that user can easily understand and able to guess the purpose of the method that they are using
The main data type that I wanna work in this library is DataFrame because DataFrame is simple to handle for machine learning than any other data type in c#.
