import os

path = os.getcwd()

print(path)

from flask import Flask
from flask import request
import json

    
import pandas as pd
from learning_spaces.kst.iita import iita_exclude_transitive
from types import SimpleNamespace


def create_app(test_config=None):
    # create and configure the app
    app = Flask(__name__, instance_relative_config=True)
    app.run(host='0.0.0.0', port=80)
    app.config.from_mapping(
        SECRET_KEY='dev',
        DATABASE=os.path.join(app.instance_path, 'flaskr.sqlite'),
    )

    if test_config is None:
        # load the instance config, if it exists, when not testing
        app.config.from_pyfile('config.py', silent=True)
    else:
        # load the test config if passed in
        app.config.from_mapping(test_config)

    # ensure the instance folder exists
    try:
        os.makedirs(app.instance_path)
    except OSError:
        pass

    # a simple page that says hello
    @app.route('/hello')
    def hello():
        return 'Hello, World!'


    
    @app.route('/iita')
    def iita():
        username = request.args.get('json')
        #matrix = json.load(username)
        x = json.loads(username, object_hook=lambda d: SimpleNamespace(**d))
        print(x)
        z = {}
        for y in range(len(x)):
            z["attempt" + str(y)] = x[y]
        
        print(z)
        data_frame = pd.DataFrame(z)
        print("ok")
        response = iita_exclude_transitive(data_frame, v=1)
        print(response)
        return str(response) #+ str(username)
    
    return app