#Note: This code works for 2019 data with following result
# Implements naive bayes by combining the strings in the input data

# Reference website for some commands
#https://towardsdatascience.com/naive-bayes-document-classification-in-python-e33ff50f937e

# First run result
'''
array([[   169,   1283],
       [  1877, 162821]], dtype=int64)
'''

filename = "C:\\Users\\sidp\\Documents\\personal\\StanfordApplication\\cs229_project\\2019.xlsx"
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

df = pd.read_excel(filename)

df2 = pd.DataFrame(df2, columns=[
'CASE_STATUS',
'JOB_TITLE',
'EMPLOYER_NAME',
'SOC_CODE',
'SOC_NAME',
'NAICS_CODE'
])

df3 = df2.copy()
df2 = df2[df2['CASE_STATUS'].isin(['DENIED','CERTIFIED'])]
df3['label'] = df3['CASE_STATUS'].apply(lambda x: 0 if x=='DENIED' else 1)
df3['CombinedX'] =df3['JOB_TITLE'].astype(str)+' ' + df3['EMPLOYER_NAME'].astype(str)+ ' ' + df3['SOC_CODE'].astype(str)+ ' ' + df3['SOC_NAME'].astype(str)+ ' ' + df3['NAICS_CODE'].astype(str)
from sklearn.model_selection import train_test_split
X_train, X_test, y_train, y_test = train_test_split(df3['CombinedX'], df3['label'], random_state=1)
from sklearn.feature_extraction.text import CountVectorizer
cv = CountVectorizer(strip_accents='ascii', token_pattern=u'(?ui)\\b\\w*[a-z]+\\w*\\b', lowercase=True, stop_words='english')
X_train_cv = cv.fit_transform(X_train)
X_test_cv = cv.transform(X_test)

# OPTIONAL TO INVESTIGATE THE DATA
# Using https://towardsdatascience.com/naive-bayes-document-classification-in-python-e33ff50f937e
#word_freq_df = pd.DataFrame(X_train_cv.toarray(), columns=cv.get_feature_names())
#top_words_df = pd.DataFrame(word_freq.sum()).sort_values(0, ascending=False)

from sklearn.naive_bayes import MultinomialNB
from sklearn.naive_bayes import MultinomialNB
naive_bayes = MultinomialNB()
naive_bayes.fit(X_train_cv, y_train)
predictions = naive_bayes.predict(X_test_cv)
from sklearn.metrics import accuracy_score
from sklearn.metrics import confusion_matrix
from sklearn.metrics import recall_score
from sklearn.metrics import precision_score
print('Accuracy score:', accuracy_score(y_test, predictions))
print('Precision score:', precision_score(y_test, predictions))
print('Recall score:', recall_score(y_test, predictions))
cm = confusion_matrix(y_test, predictions)
cm

