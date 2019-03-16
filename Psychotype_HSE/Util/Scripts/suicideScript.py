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
import socket

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

model = load_model(r'suicide_model2.h5', custom_objects={'precision': precision, 'recall': recall, 'f1': f1})

with open(r'tokenizer.pickle', 'rb') as handle:
	tokenizer = pickle.load(handle)

# Высота матрицы (максимальное количество слов в посте)
SENTENCE_LENGTH = 250
# Размер словаря
NUM = 100000


WAITLIST_SIZE = 200
SOCKET_NUM = 1111

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((socket.gethostname(), SOCKET_NUM))

s.listen(WAITLIST_SIZE)

def countResponce(clientsocket):
    request = clientsocket.recv(1024).decode("utf-32")
    response = ""

    request = request.split("::")

    if (request[0] == "~exit~"):
        raise Exception("Program interrupted")

    size = int(request[0])
    request = request[1]

    #print(size)
    #print(request)
    #print(len(request))
    if (len(request) != size):
        request += clientsocket.recv(4*(size - len(request))).decode("utf-32")

    lines = request.split("\n")

    for text in lines: 
        text = preprocess_text(text)
        text_fin = get_sequences(tokenizer, [text])
        predicted_test = np.round(model.predict(text_fin))
        response += str((predicted_test[0][0] + 1) % 2) +"\n"

    clientsocket.send(bytes(response,"utf-8"))
    clientsocket.close()


while True:
	clientsocket, adress = s.accept()
    
	try:
		countResponce(clientsocket)
	except (Exception):
		print("exit")
		break
    
	print(f"{adress} conneted")