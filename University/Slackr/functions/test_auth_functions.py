import pytest
import jwt

from data import *
from tokens import resetConnections
from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import *
from admin_functions import *
# ------------------------------------------------------------------------------
def test_auth_register_success():
    resetData()
    resetConnections()
    #user = auth_register("user@unsw.edu.au", "password", "generic", "person")
    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    u_id1 = user1['u_id']
    token1 = bytes(user1['token'][2:len(user1['token']) - 1], 'utf-8')

    assert u_id1 == 1
    assert int(jwt.decode(token1, "1531_Pass_Cut_Off", algorithm='HS256')['u_id']) == u_id1

    connected = getConnections()
    assert any(token1 == connection for connection in connected) == True
    user = user_profile(token1, u_id1)
    assert user['handle_str'] == "onesur"

    user2 = auth_register("user2@unsw.edu.au", "AfairlysTr0ngpwd", "VeryLongNamePerson", "HasLongNameYes")
    token2 = bytes(user2['token'][2:len(user2['token']) - 1], 'utf-8')
    user = user_profile(token2, user2['u_id'])
    assert user['handle_str'] == "verylongnamepersonha"
    assert user2['u_id'] == 2

    user3 = auth_register("user3@unsw.edu.au", "AfairlysTr0ngpwd", "VeryLongNamePerson", "HasLongNameYes")
    token3 = bytes(user3['token'][2:len(user3['token']) - 1], 'utf-8')
    user = user_profile(token3, user3['u_id'])
    assert user['handle_str'] == "verylongnamepersonh1"
    assert user3['u_id'] == 3

def test_auth_register_fail():
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order

    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    with pytest.raises(ValueError):
        user2 = auth_register("user1@unsw.edu.au", "AnothersTr0ngpwd", "Two", "Sur")    # email conflict
    with pytest.raises(ValueError):
        user3 = auth_register("notAvalidEmail", "AnothersTr0ngpwd", "Two", "Sur")   # invalid email
    with pytest.raises(ValueError):
        user4 = auth_register("user2@unsw.edu.au", "", "Two", "Sur")    # invalid password
    with pytest.raises(ValueError):
        user5 = auth_register("user2@unsw.edu.au", "AnothersTr0ngpwd", "Thisisastringmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchlongerthanfifty", "Sur")   # invalid first name
    with pytest.raises(ValueError):
        user6 = auth_register("user2@unsw.edu.au", "AnothersTr0ngpwd", "Two", "Thisisastringmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchmuchlongerthanfifty")   # invalid surname
# ------------------------------------------------------------------------------
def test_logout():
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    connected = getConnections()

    token = connected[0]
    valid = auth_logout(token)
    assert not any(token == connection for connection in connected)
    assert valid['is_success'] == True

    with pytest.raises(ValueError):
        channel_create(token, "firstChannel", True)
    assert (auth_logout(1234))['is_success'] == False
    assert (auth_logout(token))['is_success'] == False
    
# ------------------------------------------------------------------------------
def test_login_success():
    resetData()
    setTestData()
    resetConnections()
    
    user1 = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    u_id1 = user1['u_id']
    assert u_id1 == 1
    token1 = bytes(user1['token'][2:len(user1['token']) - 1], 'utf-8')
    assert int(jwt.decode(token1, "1531_Pass_Cut_Off", algorithm='HS256')['u_id']) == u_id1
    

def test_login_fail():
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    
    auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    with pytest.raises(ValueError):
        auth_login("user2@unsw.edu.au", "AnothersTr0ngpwd") # not registered
    with pytest.raises(ValueError):
        auth_login("notavalidemail", "AnothersTr0ngpwd")    # invalid email
    with pytest.raises(ValueError):
        auth_login("user1@unsw.edu.au", "AnothersTr0ngpwd") # incorrect pwd
# ------------------------------------------------------------------------------

def test_auth_passwordreset_request():
    # BEG SET UP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    # END

    auth_passwordreset_request("Matt.Galinski@gmail.com")
    coded = getResetCodes()
    assert len(coded[0]['code']) >= 0 #asserts there is indeed a code, and it's larger than 0

    with pytest.raises(ValueError):
        auth_passwordreset_request("not.registered@gmail.com") #email not registered
    with pytest.raises(ValueError):
        auth_passwordreset_request("invalidemail") #invalid email address

# ------------------------------------------------------------------------------
def test_auth_passwordreset_reset():
    # BEG SET UP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    # END

    auth_passwordreset_request("Brett.Jeffers@outlook.com")

    codes = getResetCodes()
    auth_passwordreset_reset(codes[0]['code'], "newPWD1valid")
    brett = auth_login("Brett.Jeffers@outlook.com", "newPWD1valid")         # check if it works
    assert brett['u_id'] == 4
    assert (auth_logout(bytes(brett['token'][2:len(brett['token']) - 1], 'utf-8')))['is_success'] == True
    resetConnections()
    auth_passwordreset_request("Brett.Jeffers@outlook.com")
    with pytest.raises(ValueError):
        auth_passwordreset_reset("notvalidcode", "nerrrereww")   #wrong reset cod
    with pytest.raises(ValueError):
        auth_passwordreset_reset(codes[0]['code'], "12") # wrong password

