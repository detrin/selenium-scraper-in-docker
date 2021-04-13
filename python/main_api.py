# -*- coding: utf-8 -*-

import time
import json
import redis
from flask import Flask
from flask import jsonify

app = Flask(__name__)
cache = redis.StrictRedis(host="redis", port=int(6379),
                decode_responses=True)

def get_hit_count():
    retries = 5
    while True:
        try:
            return cache.get("items")
        except redis.exceptions.ConnectionError as exc:
            if retries == 0:
                raise exc
            retries -= 1
            time.sleep(0.5)

@app.route('/')
def items():
    items_str = get_hit_count()
    items = json.loads(items_str)
    return jsonify(items)

