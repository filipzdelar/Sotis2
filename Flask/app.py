import os

path = os.getcwd()

print(path)

from flask import Flask,redirect
from flask import request
import json
import numpy as np
    
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
        y = json.loads(username, object_hook=lambda d: SimpleNamespace(**d))
        x = y[0]
        testID = y[1]
        print(x)
        x = np.array(x).T.tolist()
        print(x)
        z = {}
        for y in range(len(x)):
            z["attempt" + str(y)] = x[y]
        
        print(z)
        data_frame = pd.DataFrame(z)
        print("ok")
        print(z)
        response = iita_exclude_transitive(data_frame, v=1)#.append("{ \"testID\" : "+str(y[1])+"}")
        response["testId"] = testID
        print(response)

        print(type(response))
        print((str(response).replace('(',"[").replace(')',"]").replace('array','')).replace('. ', '').replace('\'', '"'))

        #return redirect("https://localhost:5001/api/graph/iita/"+(str(response).replace('(',"[").replace(')',"]").replace('array','')).replace('. ', '').replace('\'', '"'), code=200)
        
        return redirect("https://localhost:5001/api/graph/IitaChangeDomain/"+(str(response).replace('(',"[").replace(')',"]").replace('array','')).replace('. ', '').replace('\'', '"'), code=200)
        #return str(response) #+ str(username)
    
    return app