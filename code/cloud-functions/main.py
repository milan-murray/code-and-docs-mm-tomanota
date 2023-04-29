import sys
sys.path.append('./packages')

import nltk
import regex
from nltk.corpus import wordnet
from flask import jsonify, request, make_response, Flask
from flask_cors import CORS

app = Flask(__name__)
CORS(app)

nltk.download('wordnet')
nltk.download('omw-1.4')

def synonymsHttp(request):
    word = request.args.get('word')
    language = request.args.get('language')
    if not word:
        return jsonify({'error': 'No parameter for "word"'}), 400
    if not language:
        language = 'english'

    synsets = wordnet.synsets(word, lang=language)

    synonyms = []
    for syn in synsets:
        for lemma in syn.lemmas(lang=language):
            synonyms.append(lemma.name())
    
    response = make_response(jsonify({'synonyms': synonyms}))
    response.headers['Access-Control-Allow-Origin'] = '*'
    response.headers['Access-Control-Allow-Methods'] = 'GET'

    return response
