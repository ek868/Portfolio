import string, random
from Error import AccessError
import pytest

from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import *
from admin_functions import *
# ------------------------------------------------------------------------------
#                           header comments
# ------------------------------------------------------------------------------

def test_message_send():

# BEG SET UP

    resetData()
    setTestData()
    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    eiliaToken = bytes(eilia['token'][2:len(eilia['token']) - 1], 'utf-8')
    matt = auth_login("Matt.Galinski@gmail.com", "password")
    mattToken = bytes(matt['token'][2:len(matt['token']) - 1], 'utf-8')
    moreThanThousandChars = "x" * 1004
    thousandChars = "x" * 1000
    channels = getChannelClasses()
# END

    msg_id = message_send(eiliaToken, channels[0].ch_id, thousandChars)['message_id']
    channelMessages = channel_messages(eiliaToken, channels[0].ch_id, 0)

    assert channelMessages['messages'][0]['message'] == thousandChars
    assert channelMessages['messages'][0]['message_id'] == msg_id

    with pytest.raises(ValueError):
        message_send(eiliaToken, channels[0].ch_id, moreThanThousandChars) # msg with > 1000 chr
    with pytest.raises(AccessError):
        message_send(eiliaToken, channels[1].ch_id, thousandChars) # eilia not in chan_id 1

    assert (auth_logout(mattToken))['is_success'] == True

    with pytest.raises(ValueError):
        message_send(matt['token'], channels[0].ch_id, "Matt is logged out")
# ------------------------------------------------------------------------------
def test_message_remove():
# BEG SET UP
    resetData()
    resetConnections()
    setTestData()

# END SET UP
    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    eiliaToken = bytes(eilia['token'][2:len(eilia['token']) - 1], 'utf-8')

    channels = getChannelClasses()
    channelMessages = channel_messages(eiliaToken, channels[0].ch_id, 0)

    msg_id = channelMessages['messages'][0]['message_id']
    message_remove(eiliaToken, msg_id) #eilia is an owner of slackr
    
    channelMessages = channel_messages(eiliaToken, channels[0].ch_id, 0)
    assert len(channelMessages['messages']) == 0

    with pytest.raises(ValueError):
        message_remove(eiliaToken, msg_id)
    
    msg_id = message_send(eiliaToken, channels[0].ch_id, "Hello WORLD!")['message_id']
    brett = auth_login("Brett.Jeffers@outlook.com", "IkindaLikeRats321")
    brettToken = bytes(brett['token'][2:len(brett['token']) - 1], 'utf-8')

    with pytest.raises(AccessError):
        message_remove(brettToken, msg_id) #brett is not the author of the message or an admin of any kind
    

# ------------------------------------------------------------------------------
def test_message_sendlater():       #not implemented
#? un-authorised user send a message to the channel
# BEG SET UP
    user1 = auth_register("good@email.com", "abcd312312312d", "god ", "mod")
    user2 = auth_register("bad@email.com", "abcd312312312d", "chan", "admin")

    chan_id = channel_create(user1['token'], "chan1", True) # make fake id if needed
    chan_id_wrong = "99999999999"
    string = "a" * 1005
# END SET UP

    assert message_sendlater(user1['token'], chan_id['channel_id'], "5 min", 5)
# wrond chan id
    with pytest.raises(ValueError):
        message_sendlater(user1['token'], chan_id_wrong, "5 min", 5)
# message is longer than 1000 chars
    with pytest.raises(ValueError):
        message_sendlater(user1['token'], chan_id['channel_id'], string, 5)  #
# negative time
    with pytest.raises(ValueError):
        message_sendlater(user1['token'], chan_id_wrong, "5 min", -5)
# the un-authorised user:
    with pytest.raises(ValueError):
        message_sendlater(user2['token'], chan_id['channel_id'], "5 min", 5)

# ------------------------------------------------------------------------------
def test_message_edit():

# BEG SET UP
    user1 = auth_register("good@ds.com", "abcd312312312d", "god ", "mod")
    user2 = auth_register("another@fd.com", "abcd312312312d", "chan ", "admin")
    user3 = auth_register("another@df.com", "abcd312312312d", "chan ", "user")
    user4 = auth_register("yo@qw.com", "abcd312312312d", "out", "sider")

    chan_id = channel_create(user1['token'], "chan1", True) # make fake id if needed

    channel_invite(user1['token'], user2['u_id'], chan_id['channel_id'])
    channel_invite(user1['token'], user3['u_id'], chan_id['channel_id'])

    admin_userpermission_change(user1['token'], user2['u_id'], 2)

    message_send(user1['token'],chan_id['channel_id'],"hello world!   CREATOR")
    message_send(user3['token'],chan_id['channel_id'],"i'm just a user")

    lista = channel_messages(user1['token'], chan_id['channel_id'], 0)

    m_id1 = lista[0]['message_id']
    m_id2 = lista[1]['message_id']

    new_text = "test new string"
# END SET UP

    assert message_edit(user1['token'], m_id1, new_text)

    with pytest.raises(ValueError):
        message_edit(user1['token'], m_id1, 1)
# un-authorised user modification requet
    with pytest.raises(ValueError, match = r".* "):
        message_edit(user3['token'], m_id1, new_text)
# message wasn't sent by the right person
    with pytest.raises(ValueError):
        message_edit(user2['token'], m_id2, new_text)
# request from oustide of channel, ILLEGAL !
    with pytest.raises(ValueError):
        message_edit(user4['token'], m_id1, new_text)

# ------------------------------------------------------------------------------
def test_message_react():
#! assumptions: React id is basic 1 or 0, which will activate a thumbs up
#! authorised user: member of the channel
# BEG SET UP
    user1 = auth_register("good@bv.com", "abcd312312312d", "god ", "mod")
    user2 = auth_register("another@vfew.com", "abcd312312312d", "chan ", "user")

    chan_id = channel_create(user1['token'], "chan1", True) # make fake id if needed

    channel_invite(user1['token'], user2['u_id'], chan_id['channel_id'])

    # admin_userpermission_change(token1, u_id2, 2)

    message_send(user1['token'],chan_id['channel_id'],"hello world!   CREATOR")
    message_send(user1['token'],chan_id['channel_id'],"let's mees arround   CREATOR")
    # message_send(token3,chan_id['channel_id'],"i'm just a user")

    lista = channel_messages(user1['token'], chan_id['channel_id'], 0)

    m_id1 = lista[0]['message_id']
    m_id2 = lista[1]['message_id']
    m_id3_wrong = "999999999999"
# END SET UP
    assert message_react(user1['token'], m_id1, 1)
#user 2 hsa no rights
    with pytest.raises(ValueError):
        message_react(user2['token'], m_id3_wrong, 1)
# wrong reaxt request
    with pytest.raises(ValueError):
        message_react(user1['token'], m_id1, 543)
#? function bellow, 2 calls to check same id is applied twice
    with pytest.raises(ValueError):
        message_react(user2['token'], m_id1, 1)
        message_react(user2['token'], m_id1, 1)
# ------------------------------------------------------------------------------
def test_message_unreact():

# BEG SET UP
    user1 = auth_register("good@hht.com", "abcd312312312d", "god ", "mod")
    user2 = auth_register("another@bf.com", "abcd312312312d", "chan ", "user")

    chan_id = channel_create(user1['token'], "chan1", True) # make fake id if needed

    channel_invite(user1['token'], user2['token'], chan_id['channel_id'])

    # admin_userpermission_change(user1['token'], u_id2, 2)

    message_send(user1['token'],chan_id['channel_id'],"hello world!   CREATOR")
    message_send(user1['token'],chan_id['channel_id'],"let's mees arround   CREATOR")
    # message_send(token3,chan_id['channel_id'],"i'm just a user")

    lista = channel_messages(user1['token'], chan_id['channel_id'], 0)

    m_id1 = lista[0]['message_id']
    m_id2 = lista[1]['message_id']
    m_id3_wrong = "999999999999"
# END SET UP

    message_react(user1['token'], m_id1, 1)
    message_react(user2['token'], m_id1, 1)
    assert message_unreact(user1['token'], m_id1, 0)
# wrong msg id
    with pytest.raises(ValueError):
        message_unreact(user2['token'], m_id3_wrong, 0)
# wrong react id
    with pytest.raises(ValueError):
        message_unreact(user1['token'], m_id1, 543)

#? function bellow, 2 calls to check same id is applied twice
    with pytest.raises(ValueError):
        message_unreact(user2['token'], m_id1, 0)
        message_unreact(user2['token'], m_id1, 0)
# ------------------------------------------------------------------------------
def test_message_unpin():
#! assumptions, to pin and unpin, the website appends $$important$$ to string
# BEG SET UP
    u_id1, token1 = auth_register("good@ytee.com", "abcd312312312d", "god ", "mod")
    u_id2, token2 = auth_register("another@bgbf.com", "abcd312312312d", "chan ", "user")
    u_id2, token3 = auth_register("yo@eret.com", "abcd312312312d", "out ", "sider")

    chan_id = channel_create(token1, "chan1", True) # make fake id if needed

    channel_invite(token1, u_id2, chan_id)

    message_send(token1,chan_id,"hello world!   CREATOR")
    message_send(token1,chan_id,"let's mees arround   CREATOR")

    lista = channel_messages(token1, chan_id, 0)

    m_id1 = lista[0]['message_id$$important$$']
    m_id2 = lista[1]['message_id']
    m_id3_wrong = "999999999999"
# END SET UP

    message_pin(token1, m_id1)

    with pytest.raises(ValueError):
        message_unpin(token1, m_id3_wrong)

    with pytest.raises(ValueError):
        message_unpin(token2, m_id1)

    with pytest.raises(ValueError):
        message_unpin(token1, m_id2)

    with pytest.raises(AccessError):
        message_unpin(token3, m_id1)
# ------------------------------------------------------------------------------
def test_message_pin():
# BEG SET UP
    user1 = auth_register("good@asdadqw.com", "abcd312312312d", "god ", "mod")
    user2 = auth_register("another@dadsa.com", "abcd312312312d", "chan ", "user")
    user3 = auth_register("yo@eqweqw.com", "abcd312312312d", "out ", "sider")

    chan_id = channel_create(user1['token'], "chan1", True) # make fake id if needed

    channel_invite(user1['token'], user2['u_id'], chan_id['channel_id'])

    message_send(user1['token'],chan_id['channel_id'],"hello world!   CREATOR")
    message_send(user1['token'],chan_id['channel_id'],"let's mees arround   CREATOR")

    lista = channel_messages(user1['token'], chan_id['channel_id'], 0)

    m_id1 = lista[0]['message_id$$important$$']
    m_id2 = lista[1]['message_id']
    m_id3_wrong = "999999999999"
# END SET UP

    with pytest.raises(ValueError):
        message_pin(user1['token'], m_id3_wrong)

    with pytest.raises(ValueError):
        message_pin(user2['token'], m_id1)

    with pytest.raises(ValueError):
        message_pin(user1['token'], m_id2)
        message_pin(user1['token'], m_id2)

    with pytest.raises(AccessError):
        message_pin(user3['token'], m_id1)
# ------------------------------------------------------------------------------
