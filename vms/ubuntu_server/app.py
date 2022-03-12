from crypt import methods
from flask import Flask

app = Flask(__name__)

@app.route("/", methods=['GET'])
def hello_world():
    return "<p>Get Hello, World!</p>"

@app.route("/", methods=['PUT'])
def hello_world_put():
    return "<p>Put Hello, World!</p>"

@app.route("/", methods=['POST'])
def hello_world_post():
    return "<p>Post Hello, World!</p>"

app.run(host='0.0.0.0', port=5000)
