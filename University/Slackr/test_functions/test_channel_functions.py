<<<<<<< HEAD
from channel_functions import *
=======
from functions.data import *
from channel_functions.py import *
>>>>>>> aff79d321ca87b7eb70d38665c1a6412ca00dfdc
from Error import AccessError
from data import *
import pytest
import unittest

import sys
sys.path.append("functions")

from functions.auth_functions import *
from functions.channel_functions import *
from functions.message_functions import *
from functions.user_profile_functions import *
from functions.admin_functions import *
# this is line of code is a test to check if commits work, also included in channel_function and
# test_channel_function

def test_channels_create():

    #BEGIN SETUP

    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    connected = getConnections()
    #END SETUP

    publicChannel = channels_create(connected[0]['token'], "ourChannel", True) #eilia creates channel
    privateChannel = channels_create(connected[1]['token'], "myChannel", False) #matt creates channel
    all_channels = getChannels()

    assert channels_listall(connected[0]['token']) == all_channels #check that the channels were created

    with pytest.raises(ValueError): #value errors for too many characters and bad token.
        channels_create(connected[0]['token'], "bad channel name with far too many characters in it", True)
    with pytest.raises(ValueError): #wrong token
        channels_create(1234, "badTokenChannel", True)

    resetData()

def test_channel_join():

    #BEGIN SETUP

    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channels = getChannels()
    connected = getConnections()

    channelIdPrivate = channels_create(connected[0]['token'], "myChannel", False)
    channelIdPublic = channels_create(connected[2]['token'], "OurChannel", True)
    all_channels = getChannels()

    #END SETUP

    #normal case eilia create a public channel, so brett should be able to join it
    assert channel_join(connected[0]['token'],channelIdPublic['channel_id'])

    with pytest.raises(AccessError):
        channel_join(connected[2]['token'],channelIdPrivate['channel_id'])     #insufficient permissions
    with pytest.raises(ValueError):
        channel_join(connected[2]['token'], 123456)                            #bad channel id

    resetData()

def test_channel_leave():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channels = getChannels()
    connected = getConnections()
    #END SETUP

    with pytest.raises(AccessError):
        channel_leave(connected[0]['token'], 13231) # eilia wants to leave invalid channel id

    with pytest.raises(ValueError):
        channel_leave(connected[2]['token'], 10000000002) # global user wants to leave channel he is not part of

def test_channel_addowner():

    #BEGIN SETUP

    resetData()
    setTestData()

    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    matt = auth_login("Matt.Galinski@gmail.com", "password")
    nichlas = auth_login("Nichlas.Dingle@yahoo.com","IdontLikeRats123")
    brett = auth_login("Brett.Jeffers@outlook.com", "IkindaLikeRats321")
    channel_public = channels_create(eilia['u_id'], "Public", True)        # A public channel by eilia
    channel_private = channels_create(nichlas['u_id'], "MyChannel", False)  # A private channel by Nichals

    '''
    adminDetails = auth_register("admin@gmail.com", "Passw0rd123", "Mr Ad", "Min")
    adminToken = adminDetails['token']
    userDetails = auth_register("user@gmail.com", "Passw0rd123", "Mr Us", "Er")
    userToken = adminDetails['token']
    channelId = channels_create(adminToken, "MyChannel", False)
    '''
    #END SETUP

    #normal test
    channel_addowner(eilia['token'],channel_public['channel_id'] , brett['u_id'])
    channelDetails = channel_details(eilia['token'], channel_public['channel_id'])
    assertIn(brett['u_id'], channelDetails['owner_members']['u_id'])

    #attempting to add an owner when you are not an owner
    '''
    unautherisedUser1 = auth_register("unautherisedUser1@gmail.com", "Passw0rd123", "Mr Un", "Autherised")
    unautherisedUser2 = auth_register("unautheriseduser2@gmail.com", "Passw0rd123", "Mr Also", "Unautherised")
    channel_invite(adminToken, channelId, unautherisedUser1['u_id'])
    with pytest.raises(AccessError):
        channel_addowner(unautherisedUser1['token'], channelId, unautherisedUser2['u_id'])
    '''



def test_channel_removeowner():

    #BEGIN SETUP

    admin = auth_register("admin@gmail.com", "Passw0rd123", "Mr Ad", "Min")
    user1 = auth_register("user1@gmail.com", "Passw0rd123", "Mr Us", "Er")
    channelId = channels_create(admin['token'], "MyChannel", True)
    channel_join(user1['token'], channelId)
    channel_addowner(admin['token'], channelId, user1['u_id'])

    #END SETUP

    #normal test
    channelDetails = channel_details(admin['Token'], channelId)
    assertIn(user1['token'], channelDetails['owner_members'])
    channel_removeowner(admin['token'], channelId, user1['u_id'])
    assertNotIn(user1['token'], channelDetails['owner_members'])

    with pytest.raises(ValueError):
        channel_removeowner(admin['token'], channelId, user1['u_id']) #user1 is not an owner to begin with
    with pytest.raises(ValueError):
        channel_removeowner(admin['token'], 1231223, user1['u_id']) #bad channel id

    #attempting to remove an owner with an unautherised token
    channel_addowner(admin['token'], channelId, user1['u_id'])
    user2 = auth_register("user2@gmail.com", "Passw0rd123", "Mr Us", "Er")
    channel_join(user2['token'], channelId)

    with pytest.raises(AccessError):
        channel_removeowner(user2['token'], channelId, user1['u_id'])

def test_channel_messages():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channels = getChannels()
    connected = getConnections()

    loginDetails = auth_register("goodemail@gmail.com", "Passw0rd123", "Mr Good", "Email")
    token = loginDetails['token']
    channelId = channels_create(token, "myChannel1234", True)
    for i in range(0, 167):
        message_send(token, channelId[], f"Message #{i}")

    #END SETUP

    #checking that the correct number of messages is returned, with the correct content, in the correct order.
    #checking that the correct start and end values are returned.
    messages = channel_messages(token, channelId['message_id'], 0)

    assert messages['start'] == 0
    assert messages['end'] == 50
    assert len(messages) == 50

    for i in range(0, 50):
        assert messages['messages'][i] == f"Message #{i}"

    messages = channel_messages(token, channelId['message_id'], 50)

    assert messages['start'] == 50
    assert messages['end'] == 100
    assert len(messages) == 50

    for i in range(0, 50):
        assert messages['messages'][i] == f"Message #{ i + 50}"

    messages = channel_messages(token, channelId['message_id'], 100)

    assert messages['start'] == 100
    assert messages['end'] == 150
    assert len(messages) == 50

    for i in range(0, 50):
        assert messages['messages'][i] == f"Message #{i + 100}"

    messages = channel_messages(token, channelId['message_id'], 150)

    assert messages['start'] == 150
    assert messages['end'] == -1
    assert len(messages) == 17

    for i in range(0, 17):
        assert messages['messages'][i] == f"Message #{i + 150}"

    #checking that the correct errors are given when invalid input is used

    with pytest.raises(ValueError):
        channel_messages(token, -123, 0)

    with pytest.raises(ValueError):
        channel_messages(token, channelId['message_id'], 167)

    with pytest.raises(ValueError):
        channel_messages(token, channelId['message_id'], -2) #extra tests

    uninvitedToken = auth_register("notInvited@gmail.com", "passssssssss1231ABC", "Mr Not", "Invited")['token']
    with pytest.raises(AccessError):
        channel_messages(uninvitedToken['token'], channelId['message_id'], 0)

def test_channels_list():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channels = getChannels()
    connected = getConnections()

    #END SETUP

    assert channels_list(connected[1]['token']) == [all_channels] # matt is originally in two channels Firstchannel and second private
    channel1 = channels_create(connected[1]['token'], "Channel3", True) # matt creates a third channel
    all_channels = getChannels() # call one more time the get channels list
    assert channels_list(user['token']) == [all_channels] # check that original list includes Channel3


def test_channels_listall():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channels = getChannels()
    connected = getConnections()
    #END SETUP

    assert channels_listall(connected[1]['token']) == all_channels # check with two original channels

    channel2 = channels_create(connected[1]['token'], "Channel2", True)
    all_channels = getChannels()
    assert channels_listall(connected[1]['token']) == all_channels # channel2 is included

def test_channel_invite():
     #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channels = getChannels()
    connected = getConnections()
    #END SETUP

    assert channel_invite(connected[1]['token'], 0, 3)     # Matt (admin) invites Brett (member not in channel) to public channel

    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    user2 = auth_register("user2@unsw.edu.au", "AnothersTr0ngpwd", "Two", "Sur")

    with pytest.raises(ValueError):
        channel_invite(connected[1]['token'], 0, 8585)  # invalid user id
    with pytest.raises(ValueError):
        channel_invite(connected[1]['token'], 292, user1['u_id'])   # wrong channel id
    with pytest.raises(AccessError):
        channel_invite(user1['token'], 1, user2['u_id'])   # user1 is not part of chan_id == 1 which is private

    resetData()

def test_channel_details():
    #BEGIN SETUP
    resetData()
    setTestData()
    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    brett = auth_login("Brett.Jeffers@outlook.com", "IkindaLikeRats321")
    admins = getAdmins()
    owners = getOwners()
    members = getMembers()
    #END SETUP

    chan1_detaisl = channel_details(eilia['token'],0)

    assert chan1_detaisl['name'] == "firstChannel"
    assert chan1_detaisl['owner_members'] == admins + owners
    assert chan1_detaisl['all_members'] == members

    with pytest.raises(ValueError):
        channel1_details = channel_details(eilia['token'], 321)    # channel id invalid
    with pytest.raises(AccessError):
        channel1_details = channel_details(brett['token'], 1)    # channel id invalid
