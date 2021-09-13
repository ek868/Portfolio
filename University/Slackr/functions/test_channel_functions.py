from data import *
from Error import AccessError
import pytest
import unittest


from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import *
from admin_functions import *
# this is line of code is a test to check if commits work, also included in channel_function and
# test_channel_function

def test_channel_create():

    #BEGIN SETUP

    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    connected = getConnections()
    #END SETUP

    publicChannel = channel_create(connected[0], "ourChannel", True) #eilia creates channel
    privateChannel = channel_create(connected[1], "myChannel", False) #matt creates channel
    all_channel = getChannels()
    allc = channel_listall(connected[0])
    assert allc['channels'] == all_channel #check that the channel were created

    with pytest.raises(ValueError): #value errors for too many characters and bad token.
        channel_create(connected[0], "bad channel name with far too many characters in it", True)
    with pytest.raises(ValueError): #wrong token
        channel_create(1234, "badTokenChannel", True)

    resetData()

def test_channel_join():

    #BEGIN SETUP

    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channel = getChannels()
    connected = getConnections()
    user1 = auth_register("gali@go.com","qwertyuiop","matt","galli")
    user1token =  bytes(user1['token'][2:len(user1['token']) - 1], 'utf-8')
    connected2token = connected[2]

    channelIdPrivate = channel_create(user1token, "myChannel", False)
    channelIdPublic = channel_create(connected2token, "OurChannel", True)
    all_channel = getChannels()

    #END SETUP

    #normal case eilia create a public channel, so brett should be able to join it
    channel_join(connected[0],channelIdPublic['channel_id'])
    publicChannel = channel_details(connected2token,channelIdPublic['channel_id'])
    connected0Id =  int(getIdFromToken(connected[0]))
    if not any(chan['u_id'] == connected0Id for chan in publicChannel['owner_members']):
        raise ValueError("The presumed owener not in the channel")

    with pytest.raises(AccessError):
        channel_join(connected2token,channelIdPrivate['channel_id'])     #insufficient permissions
    with pytest.raises(ValueError):
        channel_join(connected2token, 123456)                            #bad channel id


def test_channel_leave():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    connected = getConnections()
    #END SETUP
    #! change ValueError to AccessError

    # channel_leave(connected[1]['token'], 10000000002)

    channel_leave(connected[1], 10000000002)

    with pytest.raises(AccessError):
        channel_details(connected[1],10000000002)

    with pytest.raises(ValueError):
        channel_leave(connected[0], 1000000000321321) # eilia wants to leave invalid channel id

    with pytest.raises(AccessError):
        channel_leave(connected[2], 10000000002) # global user wants to leave channel he is not part of

def test_channel_addowner():

    #BEGIN SETUP

    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order

    adminDetails = auth_register("admin@gmail.com", "Passw0rd123", "Mr Ad", "Min")
    adminToken =  bytes(adminDetails['token'][2:len(adminDetails['token']) - 1], 'utf-8')
    userDetails = auth_register("user@gmail.com", "Passw0rd123", "Mr Us", "Er")
    userid = userDetails['u_id']
    channelId = channel_create(adminToken, "MyChannel", False)
    #END SETUP

    #normal test
    # print(type(adminToken))
    channel_addowners(adminToken, channelId['channel_id'], userDetails['u_id'])
    channelDetails = channel_details(adminToken, channelId['channel_id'])
    test = channelDetails['owner_members'][0]['u_id']



    if not any(chan['u_id'] == userid for chan in channelDetails['owner_members']):
        raise ValueError("The presumed owener not in the channel")

    with pytest.raises(ValueError):
        channel_addowners(adminToken, channelId['channel_id'], userDetails['u_id']) #already an owner
    with pytest.raises(ValueError):
        channel_addowners(adminToken, 1233412, userDetails["u_id"]) #channel id does not exist.

    #attempting to add an owner when you are not an owner
    # unautherisedUser1 = auth_register("unautherisedUser1@gmail.com", "Passw0rd123", "Mr Un", "Autherised")
    # unautherisedUser2 = auth_register("unautheriseduser2@gmail.com", "Passw0rd123", "Mr Also", "Unautherised")
    # channel_invite(adminToken, channelId['channel_id'], unautherisedUser1['u_id'])
    # with pytest.raises(AccessError):
    #     channel_addowners(unautherisedUser1['token'], channelId['channel_id'], unautherisedUser2['u_id'])

def test_channel_removeowners():

    #BEGIN SETUP

    admin = auth_register("admin@ewq.com", "Passw0rd123", "Mr Ad", "Min")
    adminToken = bytes(admin['token'][2:len(admin['token']) - 1], 'utf-8')
    user1 = auth_register("user1@gmewqdsaail.com", "Passw0rd123", "Mr Us", "Er")
    user1TOken = bytes(user1['token'][2:len(user1['token']) - 1], 'utf-8')
    channelId = channel_create(adminToken, "MyChannel", True)
    channel_join(user1TOken, channelId['channel_id'])
    channel_addowners(adminToken, channelId['channel_id'], user1['u_id'])

    #END SETUP

    #normal test
    channelDetails = channel_details(adminToken, channelId['channel_id'])
    if not any(chan['u_id'] == admin['u_id'] for chan in channelDetails['owner_members']):
        raise ValueError("The presumed owener not in the channel")

    channel_removeowners(adminToken, channelId['channel_id'], user1['u_id'])

    if any(chan['u_id'] == user1['u_id'] for chan in channelDetails['owner_members']):
        raise ValueError("The presumed owener should not be in the channel")

    with pytest.raises(ValueError):
        channel_removeowners(adminToken, channelId['channel_id'], user1['u_id']) #user1 is not an owner to begin with
    with pytest.raises(ValueError):
        channel_removeowners(adminToken, 1231223, user1['u_id']) #bad channel id

    #attempting to remove an owner with an unautherised token
    channel_addowners(adminToken, channelId['channel_id'], user1['u_id'])
    user2 = auth_register("user2@gmail.com", "Passw0rd123", "Mr Us", "Er")
    user2Token = bytes(user2['token'][2:len(user2['token']) - 1], 'utf-8')
    channel_join(user2Token, channelId['channel_id'])

    with pytest.raises(AccessError):
        channel_removeowners(user2Token, channelId['channel_id'], user1['u_id'])

def test_channel_messages():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channel = getChannels()
    connected = getConnections()

    loginDetails = auth_register("goodemail@gmail.com", "Passw0rd123", "Mr Good", "Email")
    userTOken = bytes(loginDetails['token'][2:len(loginDetails['token']) - 1], 'utf-8')
    channelId = channel_create(userTOken, "myChannel1234", True)
    for i in range(0, 167):
        message_send(userTOken, channelId, f"Message #{i}")

    #END SETUP

    #checking that the correct number of messages is returned, with the correct content, in the correct order.
    #checking that the correct start and end values are returned.
    messages = channel_messages(userTOken, channelId['message_id'], 0)

    assert messages['start'] == 0
    assert messages['end'] == 50
    assert len(messages) == 50

    for i in range(0, 50):
        assert messages['messages'][i] == f"Message #{i}"

    messages = channel_messages(userTOken, channelId['message_id'], 50)

    assert messages['start'] == 50
    assert messages['end'] == 100
    assert len(messages) == 50

    for i in range(0, 50):
        assert messages['messages'][i] == f"Message #{ i + 50}"

    messages = channel_messages(userTOken, channelId['message_id'], 100)

    assert messages['start'] == 100
    assert messages['end'] == 150
    assert len(messages) == 50

    for i in range(0, 50):
        assert messages['messages'][i] == f"Message #{i + 100}"

    messages = channel_messages(userTOken, channelId['message_id'], 150)

    assert messages['start'] == 150
    assert messages['end'] == -1
    assert len(messages) == 17

    for i in range(0, 17):
        assert messages['messages'][i] == f"Message #{i + 150}"

    #checking that the correct errors are given when invalid input is used

    with pytest.raises(ValueError):
        channel_messages(userTOken, -123, 0)

    with pytest.raises(ValueError):
        channel_messages(userTOken, channelId['message_id'], 167)

    with pytest.raises(ValueError):
        channel_messages(userTOken, channelId['message_id'], -2) #extra tests

    user2 = auth_register("notInvited@gmail.com", "passssssssss1231ABC", "Mr Not", "Invited")['token']
    user2TOken = bytes(uninvitedToken['token'][2:len(uninvitedToken['token']) - 1], 'utf-8')
    with pytest.raises(AccessError):
        channel_messages(user2TOken, channelId['message_id'], 0)

def test_channel_list():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channel = getChannels()
    connected = getConnections()

    #END SETUP
    listadict = {'channels': all_channel}
    assert channel_list(connected[1]) ==  listadict # matt is originally in two channel Firstchannel and second private
    channel1 = channel_create(connected[1], "Channel3", True) # matt creates a third channel
    all_channel = getChannels() # call one more time the get channel list
    listadict = {'channels': all_channel}
    assert channel_list(connected[1]) == listadict # check that original list includes Channel3


def test_channel_listall():

    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channel = getChannels()
    connected = getConnections()
    #END SETUP

    listadict = {'channels': all_channel}
    assert channel_listall(connected[1]) == listadict # check with two original channel

    channel2 = channel_create(connected[1], "Channel4", True)
    all_channel = getChannels()
    listadict = {'channels': all_channel}
    assert channel_listall(connected[1]) == listadict # channel2 is included

def test_channel_invite():
     #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    all_channel = getChannels()
    connected = getConnections()
    #END SETUP

    channel_invite(connected[1], 10000000002, 3)     # Matt (admin) invites Brett (member not in channel) to public channel
    chan1_details = channel_details(connected[1],10000000002)
    members = getMembers()

    assert chan1_details['all_members'] == [members[0]]

    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    user1TOken = bytes(user1['token'][2:len(user1['token']) - 1], 'utf-8')
    user2 = auth_register("user2@unsw.edu.au", "AnothersTr0ngpwd", "Two", "Sur")

    with pytest.raises(ValueError):
        channel_invite(connected[1], 0, 8585)  # invalid user id
    with pytest.raises(ValueError):
        channel_invite(connected[1], 1000000005921321, user1['u_id'])   # wrong channel id
    with pytest.raises(AccessError):
        channel_invite(user1TOken, 10000000002, user2['u_id'])   # user1 is not part of chan_id == 1 which is private

    resetData()

def test_channel_details():
    #BEGIN SETUP
    resetData()
    setTestData()
    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    eiliaToken = bytes(eilia['token'][2:len(eilia['token']) - 1], 'utf-8')
    brett = auth_login("Brett.Jeffers@outlook.com", "IkindaLikeRats321")
    brettToken = bytes(brett['token'][2:len(brett['token']) - 1], 'utf-8')
    admins = getAdmins()
    owners = getOwners()
    members = getMembers()
    #END SETUP

    chan1_details = channel_details(eiliaToken,10000000001)

    assert chan1_details['name'] == "firstChannel"
    assert chan1_details['owner_members'] == owners + admins
    assert chan1_details['all_members'] == [members[0]]

    with pytest.raises(ValueError):
        channel1_details = channel_details(eiliaToken, 2)    # channel id invalid
    with pytest.raises(AccessError):
        channel1_details = channel_details(brettToken, 10000000001)    # channel id invalid
