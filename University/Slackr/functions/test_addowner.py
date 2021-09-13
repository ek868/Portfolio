from channel_functions import channel_create, channel_addowners, channel_details
from auth_functions import auth_login
from Error import AccessError
from data import *
from tokens import *
import pytest
import unittest


def test_channel_addowners():
    resetData()
    setTestData()

    eilia = auth_login("eilia.key@hotmail.com", "ILOVErats4312")
    matt = auth_login("Matt.Galinski@gmail.com", "password")
    nichlas = auth_login("Nichlas.Dingle@yahoo.com","IdontLikeRats123")
    brett = auth_login("Brett.Jeffers@outlook.com", "IkindaLikeRats321")
    channel = getChannelDetails(10001)
    print (channel.owners)
    channel_addowners(eilia['token'], 10001, brett['u_id'])
    #channelDetails = channel_details(eilia['token'], 10001)
    #assert brett['u_id'] in channelDetails['owner_members']['u_id']
    
    print ("test")
    print (channel.owners)


test_channel_addowners()
