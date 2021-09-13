"""Flask server"""
from json import dumps
from flask import Flask, request
from flask_cors import CORS
from werkzeug.exceptions import HTTPException

import sys
sys.path.append("functions")

from functions.auth_functions import *
from functions.channel_functions import *
from functions.message_functions import *
from functions.user_profile_functions import *
from functions.admin_functions import *
from functions.standup_functions import *
from functions.search_functions import *

def defaultHandler(err):
    response = err.get_response()
    response.data = dumps({
        "code": err.code,
        "name": "System Error",
        "message": err.description,
    })
    response.content_type = 'application/json'
    return response

APP = Flask(__name__)
APP.config['TRAP_HTTP_EXCEPTIONS'] = True
APP.register_error_handler(Exception, defaultHandler)
CORS(APP)

@APP.route('/echo/get', methods=['GET'])
def echo1():
    """ Description of function """
    return dumps({
        'echo' : request.args.get('echo'),
    })

@APP.route('/echo/post', methods=['POST'])
def echo2():
    """ Description of function """
    return dumps({
        'echo' : request.form.get('echo'),
    })

@APP.route('/auth/login', methods=['POST'])
def login():
    email = request.form.get('email')
    password = request.form.get('password')
    return dumps(auth_login(email, password))

@APP.route('/auth/logout', methods=['POST'])
def logout():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    return dumps(auth_logout(token))

@APP.route('/auth/register', methods=['POST'])
def register():
    email = request.form.get('email')
    password = request.form.get('password')
    name_first = request.form.get('name_first')
    name_last = request.form.get('name_last')
    return dumps(auth_register(email, password, name_first, name_last))

@APP.route('/auth/passwordreset/request', methods=['POST'])
def passwordreset_request():
    email = request.form.get('email')
    auth_passwordreset_request(email)
    return dumps({})

@APP.route('/auth/passwordreset/reset', methods=['POST'])
def passwordreset_reset():
    resetCode = request.form.get('reset_code')
    newPassword = request.form.get('new_password')
    auth_passwordreset_reset(resetCode, newPassword)
    return dumps({})

@APP.route('/channel/invite', methods=['POST'])
def invite():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    u_id = request.form.get('u_id')
    channel_invite(token, ch_id, u_id)
    return dumps({})

@APP.route('/channel/details', methods=['GET'])
def details():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.args.get('channel_id')
    return dumps(channel_details(token, ch_id))

@APP.route('/channel/messages', methods=['GET'])
def messages():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.args.get('channel_id')
    start = request.args.get('start')
    return dumps(channel_messages(token, ch_id, start))

@APP.route('/channel/leave', methods=['POST'])
def leave():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    return dumps(channel_leave(token, ch_id))

@APP.route('/channel/join', methods=['POST'])
def join():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    channel_join(token, ch_id)
    return dumps({})

@APP.route('/channel/addowner', methods=['POST'])
def addowner():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    u_id = request.form.get('u_id')
    channel_addowners(token, ch_id, u_id)
    return dumps({})

@APP.route('/channel/removeowner', methods=['POST'])
def removeowner():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    u_id = request.form.get('u_id')
    channel_removeowners(token, ch_id, u_id)
    return dumps({})

@APP.route('/channels/list', methods=['GET'])
def listChannels():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    return dumps(channel_list(token))

@APP.route('/channels/listall', methods=['GET'])
def listallChannels():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    return dumps(channel_listall(token))

@APP.route('/channels/create', methods=['POST'])
def create():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    name = request.form.get('name')
    is_public = request.form.get('is_public')
    return dumps(channel_create(token, name, is_public))

@APP.route('/message/sendlater', methods=['POST'])
def sendLater():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    message = request.form.get('message')
    time_sent = request.form.get('time_sent')
    return dumps(message_sendlater(token, ch_id, message, time_sent))

@APP.route('/message/send', methods=['POST'])
def send():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    message = request.form.get('message')
    return dumps(message_send(token, ch_id, message))

@APP.route('/message/remove', methods=['DELETE'])
def removeMessage():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    message_id = request.form.get('message_id')
    message_remove(token, message_id)
    return dumps({})

@APP.route('/message/edit', methods=['POST'])
def edit():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    message_id = request.form.get('message_id')
    message = request.form.get('message')
    message_edit(token, message_id, message)
    return dumps({})

@APP.route('/message/react', methods=['POST'])
def react():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    message_id = request.form.get('message_id')
    react_id = request.form.get('react_id')
    message_react(token, message_id, react_id)
    return dumps({})

@APP.route('/message/unreact', methods=['POST'])
def unreact():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    message_id = request.form.get('message_id')
    react_id = request.form.get('react_id')
    message_unreact(token, message_id, react_id)
    return dumps({})

@APP.route('/message/pin', methods=['POST'])
def pin():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    message_id = request.form.get('message_id')
    message_pin(token, message_id)
    return dumps({})

@APP.route('/message/unpin', methods=['POST'])
def unpin():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    message_id = request.form.get('message_id')
    message_unpin(token, message_id)
    return dumps({})

@APP.route('/user/profile', methods=['GET'])
def profile():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    u_id = request.args.get('u_id')
    return dumps(user_profile(token, u_id))

@APP.route('/user/profile/setname', methods=['PUT'])
def setname():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    name_first = request.form.get('name_first')
    name_last = request.form.get('name_last')
    user_profile_setname(token, name_first, name_last)
    return dumps({})

@APP.route('/user/profile/setemail', methods=['PUT'])
def setemail():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    email = request.form.get('email')
    user_profile_setemail(token, email)
    return dumps({})

@APP.route('/user/profile/sethandle', methods=['PUT'])
def sethandle():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    handle_str = request.form.get('handle_str')
    user_profile_sethandle(token, handle_str)
    return dumps({})

@APP.route('/user/profiles/uploadphoto', methods=['POST'])
def uploadphoto():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    img_url = request.form.get('img_url')
    x_start = request.form.get('x_start')
    y_start = request.form.get('y_start')
    x_end = request.form.get('x_end')
    y_end = request.form.get('y_end')
    user_profiles_uploadphoto(token, img_url, x_start, y_start, x_end, y_end)
    return dumps({})

@APP.route('/standup/start', methods=['POST'])
def standupStart():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    return dumps(standup_start(token, ch_id))

@APP.route('/standup/send', methods=['POST'])
def standupSend():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.form.get('channel_id')
    message = request.form.get('message')
    standup_send(token, ch_id, message)
    return dumps({})

@APP.route('/standup/active', methods=['GET'])
def standupActive():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    ch_id = request.args.get('channel_id')
    return dumps(standup_active(token, ch_id))

@APP.route('/search', methods=['GET'])
def search_wf():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    query_str = request.args.get('query_str')
    return dumps(search(token, query_str))

@APP.route('/admin/userpermission/change', methods=['POST'])
def changePermission():
    token = request.form.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    u_id = request.form.get('u_id')
    permission_id = request.form.get('permission_id')
    admin_userpermission_change(token, u_id, permission_id)
    return dumps({})

@APP.route('/users/all', methods=['GET'])
def getAllUsers():
    token = request.args.get('token')
    token = bytes(token[2:len(token) - 1], 'utf-8')
    return dumps(users_all(token))

if __name__ == '__main__':
    APP.run(port=(sys.argv[1] if len(sys.argv) > 1 else 5000), debug=True)
