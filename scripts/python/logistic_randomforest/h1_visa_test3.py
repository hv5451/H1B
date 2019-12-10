'''
This code takes the data takes all the rejected cases which is 2841 and then takes only 2841 random approved cases
It then merges them into a single dataframe, shuffles them to make them random and then does logistic regression on them

We then try it on random forest as well.

TODO: Try the prediction on entire dataset of ~600k minus the training set

'''

import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

filename = "C:\\Users\\sidp\\Documents\\personal\\StanfordApplication\\cs229_project\\2019.xlsx"
df = pd.read_excel(filename)
features = \
[
'CASE_STATUS',       # Take only approved and Denied
'VISA_CLASS',        # Take only H1-B
'EMPLOYER_NAME',      # Has 70K values, might have to ignore or at least compress.
'EMPLOYER_CITY',     # Has 5K values, might have to ignore
'SECONDARY_ENTITY',  # Y/N
'AGENT_REPRESENTING_EMPLOYER', # Y/N
'SOC_CODE',          # Take only first two digits of the string.
'NAICS_CODE',        # Take only first two digits of string, if possible first three digits.for
'CONTINUED_EMPLOYMENT', # Y/N
'FULL_TIME_POSITION', # Y/N
'PREVAILING_WAGE',    # Total 2000 values, try to group into ranges or ignore
'WORKSITE_CITY',     # About 12K cities, try to group or ignore
#'PW_UNIT_OF_PAY ' Ignoring this because unit of pay for all values is yearly
'PW_WAGE_LEVEL',     # Use to get an average wage level
#'WAGE_RATE_OF_PAY_FROM'     # Use to derive an average value per year. Unit of pay is not properly aligned, so a little hard to do. Removing na reduces to 300K rows.
#'WAGE_RATE_OF_PAY_TO' # Also calculate the difference between official wage and actual wage payed (actual = avg of from and to wages)
#'WAGE_UNIT_OF_PAY'
'H1B_DEPENDENT',      # Y/n
'WORKSITE_CITY',      # Has 12K vlaues, might have to ignore.for
'WILLFUL_VIOLATOR',   # Y/N
#'STATUTORY_BASIS'   # Only three values wage, both and degree. A lot of blanks
]

features_small = \
[
'CASE_STATUS',       # Take only approved and Denied
'SECONDARY_ENTITY',  # Y/N
'AGENT_REPRESENTING_EMPLOYER', # Y/N
'SOC_CODE',          # Take only first two digits of the string.
'NAICS_CODE',        # Take only first two digits of string, if possible first three digits.for
'CONTINUED_EMPLOYMENT', # Y/N
'FULL_TIME_POSITION', # Y/N
'PW_WAGE_LEVEL',      # 5 count, Use to get an average wage level
'H1B_DEPENDENT',      # Y/n
'WILLFUL_VIOLATOR',   # Y/N
]

# Limit to selected features which we are going to use and H1B status in denied or certified values.
df1 = df.copy()
df1 = df[features_small]
df1 = df1[df1['CASE_STATUS'].isin(['DENIED','CERTIFIED'])]
df1['CASE_STATUS'] = df1['CASE_STATUS'].map(dict(CERTIFIED=1, DENIED=0))

# For SOC Code, retain only the two values before "-" to get major categories
# This creates some dirty rows with strings and invalid values like 291122, so we remove all rows with such values.
df1 = df1[pd.isna(df1['SOC_CODE'])!=True]
#df1['SOC_CODE'] = df1['SOC_CODE'].str.split('-', 0, expand=True)[0].astype(int, errors='ignore')
df1['SOC_CODE'] = pd.to_numeric(df1['SOC_CODE'].str.split('-', 0, expand=True)[0], errors='coerce').fillna(0).astype(np.int64)
df1['SOC_CODE'] = df1['SOC_CODE'][df1['SOC_CODE'].between(1, 100, inclusive=True)]
df1 = df1[pd.isna(df1['SOC_CODE'])!=True]

# For NAICS_CODE, take only first two digits of the string
df1 = df1[pd.isna(df1['NAICS_CODE'])!=True]
df1['NAICS_CODE'] = df1['NAICS_CODE'].mod(100) # Generates roughly 84 distinct values.

# For CONTINUED_EMPLOYMENT remove all values other than 0 and 1.
# Remove all na
df1 = df1[pd.isna(df1['CONTINUED_EMPLOYMENT'])!=True]
df1['CONTINUED_EMPLOYMENT'] = pd.to_numeric(df1['CONTINUED_EMPLOYMENT'], errors='coerce').fillna(0).astype(np.int64)
df1['CONTINUED_EMPLOYMENT'] = df1['CONTINUED_EMPLOYMENT'][df1['CONTINUED_EMPLOYMENT'].between(0, 1, inclusive=True)]
df1 = df1[pd.isna(df1['CONTINUED_EMPLOYMENT'])!=True]

# FULL_TIME_POSITION  Replace Y and N with 1 and 0.
# # Remove all na and remove all rows which are not 0 and 1
df1 = df1[pd.isna(df1['FULL_TIME_POSITION'])!=True]
df1['FULL_TIME_POSITION'] = df1['FULL_TIME_POSITION'].map(dict(Y=1, N=0))
df1['FULL_TIME_POSITION'] = df1['FULL_TIME_POSITION'][df1['FULL_TIME_POSITION'].between(0, 1, inclusive=True)]

# PW_WAGE_LEVEL
df1 = df1[pd.isna(df1['PW_WAGE_LEVEL'])!=True]
df1['PW_WAGE_LEVEL'] = df1['PW_WAGE_LEVEL'].between(1.0, 5.0, inclusive=True)


#H1B_DEPENDENT
df1 = df1[pd.isna(df1['H1B_DEPENDENT'])!=True]
df1['H1B_DEPENDENT'] = df1['H1B_DEPENDENT'].map(dict(Y=1, N=0))
df1 = df1[pd.isna(df1['H1B_DEPENDENT'])!=True]


#WILLFUL_VIOLATOR
df1 = df1[pd.isna(df1['WILLFUL_VIOLATOR'])!=True]
df1['WILLFUL_VIOLATOR'] = df1['WILLFUL_VIOLATOR'].map(dict(Y=1, N=0))
df1 = df1[pd.isna(df1['WILLFUL_VIOLATOR'])!=True]

df1 = df1[pd.isna(df1['SECONDARY_ENTITY'])!=True]
df1['SECONDARY_ENTITY'] = df1['SECONDARY_ENTITY'].map(dict(Y=1, N=0))
df1 = df1[pd.isna(df1['SECONDARY_ENTITY'])!=True]

df1 = df1[pd.isna(df1['AGENT_REPRESENTING_EMPLOYER'])!=True]
df1['AGENT_REPRESENTING_EMPLOYER'] = df1['AGENT_REPRESENTING_EMPLOYER'].map(dict(Y=1, N=0))
df1 = df1[pd.isna(df1['AGENT_REPRESENTING_EMPLOYER'])!=True]

# TODO: This shoudl ideally be done for each column separately, but due to shortage of time, nuking all na rows here
df1 = df1.dropna()


rejected = df1[df1['CASE_STATUS']==0]
accepted = df1[df1['CASE_STATUS']==1]
accepted = accepted.sample(n=2841)
total = pd.concat([accepted, rejected])
from sklearn.utils import shuffle
total = shuffle(total)
msk = np.random.rand(len(total)) < 0.8
X_train = total[msk].iloc[:,1:9]
X_test = total[~msk].iloc[:,1:9]
y_train = total[msk].iloc[:,0]
y_test = total[~msk].iloc[:,0]

from sklearn.preprocessing import OneHotEncoder
X_train_ordinal = X_train.values
X_test_ordinal = X_test.values
enc = OneHotEncoder(handle_unknown='ignore')
enc.fit(X_train_ordinal)
X_train_one_hot = enc.transform(X_train_ordinal)
X_test_one_hot = enc.transform(X_test_ordinal)
# After this, the fit step takes forever so trying to optimize the matrix before running it
from sklearn.linear_model import LogisticRegression
l = LogisticRegression()
l.fit(X_train_one_hot,y_train)
y_pred = l.predict_proba(X_test_one_hot)
confusion_matrix(y_test, y_pred[:, 1] > 0.5)
#OUTPUT
#array([[366, 178],
#       [233, 311]], dtype=int64)

r.fit(X_train_one_hot,y_train)
RandomForestClassifier(bootstrap=True, class_weight=None, criterion='gini',
            max_depth=32, max_features='auto', max_leaf_nodes=None,
            min_impurity_decrease=0.0, min_impurity_split=None,
            min_samples_leaf=1, min_samples_split=2,
            min_weight_fraction_leaf=0.0, n_estimators=10, n_jobs=None,
            oob_score=False, random_state=None, verbose=0,
            warm_start=False)
y_pred = r.predict_proba(X_test_one_hot)
confusion_matrix(y_test, y_pred[:, 1] > 0.5)
# OUTPUT
#array([[386, 158],
#       [213, 331]], dtype=int64)