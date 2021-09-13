from user_profile_functions.py import *
from Error import AccessError
import pytest
import unittest


from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import *
from admin_functions import *

def test_user_profile_setemail():

    #BEGIN SETUP

    admin = auth_register("admin@gmail.com", "Passw0rd123", "Mr Ad", "Min")
    user1 = auth_register("user1@gmail.com", "Passw0rd123", "Mr Us", "Er")
    user2 = auth_register("user2@gmail.com", "Passw0rd123", "Mr Us", "Er")

    #END SETUP

    user_profile_setemail(user1['token'], "googlyboo@gmail.com") #change the email
    assert user_profile(admin, user1['u_id'])['email'] == "googlyboo@gmail.com"

    with pytest.raises(ValueError): #invalid email
        user_profile_setemail(user1['token'], "billy bob at gmail dot com")
    with pytest.raises(ValueError): #email is taken
        user_profile_setemail(user2['token'], "googlyboo@gmail.com")
    with pytest.raises(ValueError): #invalid token
        user_profile_setemail(1234, "1234@gmail.com")

def test_user_profile_setname():
    #BEGIN SETUP

    resetData()
    setTestData()
    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    #END SETUP
    first = "longname" * 32
    second = "longname" * 32
    #invalid names
    with pytest.raises(ValueError):
        user_profile_setname(eilia['token'], first, "SurMore")
        user_profile_setname(eilia['token'], "OneMore", second)

def test_user_profiles_uploadphoto_success():
    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    user_profiles_uploadphoto(user1['token'], https://upload.wikimedia.org/wikipedia/en/a/a9/Example.jpg, 0,0,200,200)  # this is an example image from wikipedia

def test_user_profiles_uploadphoto_fail():
    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    user_profiles_uploadphoto(user1['token'], https://upload.wikimedia.org/awikipedia/en/a/a9/Example.jpg, 0,0,200,200)  # 403 URL
    user_profiles_uploadphoto(user1['token'], https://upload.wikimedia.org/wikipedia/en/a/a9/Example.jpg, -1,0,200,200)
    user_profiles_uploadphoto(user1['token'], https://upload.wikimedia.org/wikipedia/en/a/a9/Example.jpg, 0,-1,200,200)
    user_profiles_uploadphoto(user1['token'], https://upload.wikimedia.org/wikipedia/en/a/a9/Example.jpg, 0,0,400,200)
    user_profiles_uploadphoto(user1['token'], https://upload.wikimedia.org/wikipedia/en/a/a9/Example.jpg, 0,0,200,400)
    user_profiles_uploadphoto(user1['token'], https://upload.wikimedia.org/wikipedia/en/a/a9/Example.jpg, 200,200,0,0)

def test_user_profile():

# BEG SET UP
    user = auth_register("good@email.com", "abcd", "god ", "mod")
    u_id1_unvalid = "3423ALmonfs4   "
# END SET UP
    with pytest.raises(ValueError):
        user_profile(user['token'], u_id1_unvalid)

def test_user_profile_sethandle():

    #BEGIN SETUP
    resetData()
    setTestData()
    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    matt = auth_login("Matt.Galinski@gmail.com", "password")

    #END SETUP
    nick_name = "BigFella123"
    long_nick_name = "BigFellaQLD_won_statOfOrigin2017"
    assert user_profile_sethandle(eilia['token'], nick_name)
# long handnle
    with pytest.raises(ValueError):
        user_profile_sethandle(eilia['token'], long_nick_name)
