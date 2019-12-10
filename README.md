# H1B
Public code for CS229 project Predicting Work visa

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
