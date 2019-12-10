import pandas as pd
import numpy as np
import sklearn
from sklearn.linear_model import LogisticRegression
from sklearn import metrics

from sklearn.feature_extraction.text import CountVectorizer
from sklearn.naive_bayes import MultinomialNB
from sklearn.metrics import accuracy_score
from sklearn.metrics import confusion_matrix
from sklearn.metrics import recall_score
from sklearn.metrics import precision_score

def run_logistic(train_path = '/home/hitesh/project/data/training.csv', valid_path = '/home/hitesh/project/data/validation.csv', y_val_id = 'Decision', exclude = 'CASE_NUMBER'):
    training = pd.read_csv(train_path)
    cols = training.columns
    x_cols = [cols[i] for i in range(len(cols)) if cols[i] != y_val_id and cols[i] not in exclude]
    y_train = np.array(training[y_val_id])
    x_train = np.array(training[x_cols])

    logisticRegr = LogisticRegression()
    logisticRegr.fit(x_train, y_train)

    val = pd.read_csv(valid_path)
    y_val = np.array(val[y_val_id])
    x_val = np.array(val[x_cols])
    predictions = logisticRegr.predict(x_val)
    score = logisticRegr.score(x_val, y_val)
    cm = metrics.confusion_matrix(y_val,predictions)
    return x_val, y_val, score, predictions, cm, logisticRegr
# python
# import h1b
# score,cm = h1b.run_logistic()


def run_nb_MultinomialNB(train_path = '/home/hitesh/project/data/CombinedObserver.csv', valid_path = '/home/hitesh/project/data/CombinedObserver.csv', y_val_id = 'Decision', coulmn_name = 'Together'):
    training = pd.read_csv(train_path)
    y_train = np.array(training[y_val_id])
    x_text = np.array(training[coulmn_name])
    cv = CountVectorizer(strip_accents='ascii', token_pattern=u'(?ui)\\b\\w*[a-z]+\\w*\\b', lowercase=True, stop_words='english')
    x_train = cv.fit_transform(x_text)
    naive_bayes = MultinomialNB()
    naive_bayes.fit(x_train, y_train)

    val = pd.read_csv(valid_path)
    y_val = np.array(val[y_val_id])
    val = np.array(val[coulmn_name])
    cv = CountVectorizer(strip_accents='ascii', token_pattern=u'(?ui)\\b\\w*[a-z]+\\w*\\b', lowercase=True, stop_words='english')
    x_val = cv.fit_transform(val)
    predictions = naive_bayes.predict(x_val)
    accuracy = accuracy_score(y_val, predictions)
    precision = precision_score(y_val, predictions)
    recall = recall_score(y_val, predictions)
    cm = confusion_matrix(y_val, predictions)
    return val, y_val , accuracy, precision, recall, cm, naive_bayes

# python
# import h1b
# accuracy, precision, recall, cm = h1b.run_nb_MultinomialNB()