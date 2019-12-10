import pandas as pd
import numpy
import sklearn
from sklearn.linear_model import LogisticRegression
from sklearn import metrics

def run_logistic(train_path = '/home/hitesh/project/data/training.csv', valid_path = '/home/hitesh/project/data/validation.csv', y_val_id = 'Decision', exclude = 'CASE_NUMBER'):
    training = pd.read_csv(train_path)
    cols = training.columns
    x_cols = [cols[i] for i in range(len(cols)) if cols[i] != y_val_id and cols[i] not in exclude]
    y_train = numpy.array(training[y_val_id])
    x_train = numpy.array(training[x_cols])

    logisticRegr = LogisticRegression()
    logisticRegr.fit(x_train, y_train)

    val = pd.read_csv(valid_path)
    y_val = numpy.array(val[y_val_id])
    x_val = numpy.array(val[x_cols])
    predictions = logisticRegr.predict(x_val)
    score = logisticRegr.score(x_val, y_val)
    cm = metrics.confusion_matrix(y_val,predictions)
    return score, cm
# python
# import h1b
# score,cm = h1b.run_logistic()