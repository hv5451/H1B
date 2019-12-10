# H1B
Public code for CS229 project Predicting Work visa

## Folder Structure:
- scripts/powershell contains adhoc processing scripts in powershell
- scripts/python contains logistic and naive bayes python code for predictions that takes below csv files as input.
- source/DataCleaningLib contains code to process and summarize the data. Cleanup.cs is the startup class.
- source/DataCleaningConsole is a simple console app that contains some adhoc methods for quick processing by calling Cleanup class from DataCleaningLib.
- Tested for both for Linux and Windows Machines, to run scripts: Python,pip,scikit-learn and pandas. To run code: dotnet framework and powershell for windows, dotnet core and powershell core for linux.

## 2014-2019 Dataset processed:
- Source CSVs at https://1drv.ms/u/s!Av-7kBjGzwHTgcRFzW2rrlBAUiR2sw?e=nKX0n6 by year name e.g. 2014.csv is cleaned dataset for year 2014 that can be processed from DataCleaning Project
- merged4.csv contains wage and calcualted wage feature with decision from 2014-2019 that can be used for logistic or kernels.
- mergedtxt4.csv contains decision with text features separated by spaces that can be used for naive bayes model.
- transformed2019 folder contains stats about 2019 year (explaination below).

## 2019 Dataset processed:
 - Source csv for 2019 dataset is https://1drv.ms/u/s!Av-7kBjGzwHTgcRLMOyadgWi1Cmrcg?e=4cEmwd converted from https://www.foreignlaborcert.doleta.gov/pdf/PerformanceData/2019/H-1B_Disclosure_Data_FY2019.xlsx
 - Other files at https://1drv.ms/u/s!Av-7kBjGzwHTgcRP4ZO1qMOB7HnS0A?e=Kba9tK processed from code
 - *Observer.json are summarization on features with details
 - *Observer.csv are features that can be plugged in model directly
 - CombinedObserver.csv are text tokens for naive bayes model
 - training.csv for logistic training
 - validation.csv for logistic model evaluation
 - transformed.csv is training + validation dataset for evaluating model fitting
 - errors.json are the skipped rows with details due to errors
 - WageTransformation_error.csv are the skipped wages datapoints due to error
