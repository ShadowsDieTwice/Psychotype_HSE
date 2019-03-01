import io
from keras import backend as K
from keras.models import load_model
import re
import pickle
from keras.preprocessing.text import Tokenizer
from keras.preprocessing.sequence import pad_sequences
import numpy as np
import sys
import os
from time import sleep

def precision(y_true, y_pred):
    """Precision metric.

    Only computes a batch-wise average of precision.

    Computes the precision, a metric for multi-label classification of
    how many selected items are relevant."""
    
    true_positives = K.sum(K.round(K.clip(y_true * y_pred, 0, 1)))
    predicted_positives = K.sum(K.round(K.clip(y_pred, 0, 1)))
    precision = true_positives / (predicted_positives + K.epsilon())
    return precision


def recall(y_true, y_pred):
    """Recall metric.

    Only computes a batch-wise average of recall.

    Computes the recall, a metric for multi-label classification of
    how many relevant items are selected."""
    
    true_positives = K.sum(K.round(K.clip(y_true * y_pred, 0, 1)))
    possible_positives = K.sum(K.round(K.clip(y_true, 0, 1)))
    recall = true_positives / (possible_positives + K.epsilon())
    return recall


def f1(y_true, y_pred):
    precision1 = precision(y_true, y_pred)
    recall1 = recall(y_true, y_pred)
    return 2 * ((precision1 * recall1) / (precision1 + recall1 + K.epsilon()))

def preprocess_text(text):
    text = text.lower().replace("ё", "е")
    text = re.sub('((www\.[^\s]+)|(https?://[^\s]+))', 'URL', text)
    text = re.sub('@[^\s]+', ' ', text)
    text = re.sub('[^a-zA-Zа-яА-Я1-9]+', ' ', text)
    text = re.sub(' +', ' ', text)
    return text.strip()

def get_sequences(tokenizer, x):
    sequences = tokenizer.texts_to_sequences(x)
    return pad_sequences(sequences, maxlen=SENTENCE_LENGTH)

model = load_model(r'C:\Users\1\Source\Repos\myrachins\Psychotype_HSE_v2\Psychotype_HSE\Util\Scripts\suicide_model2.h5', custom_objects={'precision': precision, 'recall': recall, 'f1': f1})

with open(r'C:\Users\1\Source\Repos\myrachins\Psychotype_HSE_v2\Psychotype_HSE\Util\Scripts\tokenizer.pickle', 'rb') as handle:
    tokenizer = pickle.load(handle)

# Высота матрицы (максимальное количество слов в посте)
SENTENCE_LENGTH = 250
# Размер словаря
NUM = 100000

"""fileShutdown = str(sys.argv[3])"""

#filePathCSV = str(sys.argv[1])
workingDir = str(sys.argv[1])
#filePathStart = str(sys.argv[2])

while True :
    if (len(os.listdir(workingDir)) != 0):
        for file in os.listdir(workingDir):
            if file.endswith(".csv"):
                try:
                    id = os.path.splitext(os.path.basename(file))[0]
                    
                    filePathStart = workingDir + id + '.csv'
                    filePathRes = workingDir + id + '.txt'
                    
                    fileFrom = open(filePathStart, mode="r", encoding="utf-8", errors="ignore")
                    fileTo = open(filePathRes, 'a')

                    lines = fileFrom.readlines()
        
                    for text in lines: 
                        text = preprocess_text(text)
                        text_fin = get_sequences(tokenizer, [text])
                        predicted_test = np.round(model.predict(text_fin))
        
                        fileTo.write(str((predicted_test[0][0] + 1) % 2))
                        fileTo.write("\n")
                    fileTo.close()
                    fileFrom.close()
                    os.remove(filePathStart)
                except:
                    sleep(0.5)
                #os.rename(filePathCSV, filePathCSV + ".temp")
        
"""
    if os.path.isfile(filePathStart) :
        fileFrom = open(filePathStart, mode="r", encoding="utf-8", errors="ignore")
        fileTo = open(filePathCSV, 'a')

        lines = fileFrom.readlines()
        
        for text in lines : 
            text = preprocess_text(text)
            text_fin = get_sequences(tokenizer, [text])
            predicted_test = np.round(model.predict(text_fin))
        
            fileTo.write(str((predicted_test[0][0] + 1) % 2))
            fileTo.write("\n")
        fileTo.close()
        fileFrom.close()
        os.remove(filePathStart)
        os.rename(filePathCSV, filePathCSV + ".temp")
        
if os.path.isfile(filePathStart) :
	file.write("hehehe")
	fileFrom = io.open(filePathStart, mode="r", encoding="utf-8")
	fileTo = open(filePathCSV, 'a')
        
	lines = fileFrom.readlines()

	fileTo.write(str(1.0))
	fileTo.write(str(1.0))
	fileTo.write(str(0.0))
	file.write("")
	fileTo.close()
	fileFrom.close()
	os.remove(filePathStart)
	os.rename(filePathCSV, filePathCSV + ".temp")"""