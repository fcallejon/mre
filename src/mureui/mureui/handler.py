import os
from urllib.parse import parse_qs


def getContentType(filename):
    parts = filename.split('.')
    try:
        if parts[1] == 'js' or parts[2] == 'js':
            return "application/javascript"
        elif parts[1] == 'css' or parts[2] == 'css':
            return "text/css"
        else:
            return "text/html; charset=utf-8"
    except:
        return "text/html; charset=utf-8"

def handle(event, context):

    try:
        file = event.query['f']
    except KeyError:
        file = "index.html"

    ct = getContentType(file)

    dirname = os.path.dirname(__file__)
    path = os.path.join(dirname, file)

    with(open(path, 'r')) as file:
        content = file.read()

    return {
        "statusCode": 200,
        "body": content,
        "headers": {
            "Content-Type": ct
        }
    }
