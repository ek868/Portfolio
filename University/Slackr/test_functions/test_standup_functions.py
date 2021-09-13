from standup_functions.py import *
from Error import AccessError
import pytest
import unittest
import datetime
import time


<<<<<<< HEAD
from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import *
from admin_functions import *
=======
import sys
sys.path.append("../functions")

from functions.auth_functions import *
from functions.channel_functions import *
from functions.message_functions import *
from functions.user_profile_functions import *
from functions.admin_functions import *
>>>>>>> 34144f7fa0b586d4eca70954ab85629dd44d6959

def test_standup():

    #BEGIN SETUP

    admin = auth_register("admin@gmail.com", "Passw0rd123", "Mr Ad", "Min")
    user = auth_register("user@gmail.com", "Passw0rd123", "Mr Us", "Er")

    channel1 = channels_create(admin['token'], "Channel1", True)
    channel2 = channels_create(admin['token'], "Channel2", True)

    channel_join(user['token'], channel1)

    #END SETUP

    assert standup_start(admin['token'], channel1) == datetime.datetime.now() + datetime.timedelta(minutes=15) #check that the correct time is returned
    with pytest.raises(AccessError):
        message_send(user['token'], channel1, "Testing123") #you cant send messages during standup

    long_message = "I promised to look after a friends cat for the week. My place has a glass atrium that goes through two levels, I have put the cat in there with enough food and water to last the week. I am looking forward to the end of the week. It is just sitting there glaring at me, it doesn't do anything else. I can tell it would like to kill me. If I knew I could get a perfect replacement cat, I would kill this one now and replace it Friday afternoon. As we sit here glaring at each other I have already worked out several ways to kill it.The simplest would be to drop heavy items on it from the upstairs bedroom though I have enough basic engineering knowledge to assume that I could build some form of spear like projectile device from parts in the downstairs shed. If the atrium was waterproof, the most entertaining would be to flood it with water. It wouldnt have to be that deep, just deeper than the cat.I dont know how long cats can swim but I doubt it would be for a whole week. If it kept the swimming up for too long I could always try dropping things on it as well. I have read that drowning is one of the most peaceful ways to die so really it would be a win win situation for me and the cat I think. "

    with pytest.raises(ValueError):
        standup_send(user['token'], channel1, long_message) #message is too long

    #The following part of the test will take 16 minutes to execute by design
    #Leave this test commented out unless this part is being tested specifically
    '''
    standup_send(user['token'], channel1, "I completed task 1")
    standup_send(admin['token'], channel1, "I completed task 55")
    standup_send(user['token'], channel1, "but Ad Min has not done task 2")
    standup_send(user['token'], channel1, "So I can't do task 3")
    standup_send(admin['token'], channel1, "Now I will do task 6")
    standup_send(user['token'], channel1, "I will do task 4 instead.")

    time.sleep(960)

    expected_message = "Mr Us Er:\n\nI completed task 1\nbut Ad Min has not done task 2\nSo I can't do task 3\nI will do task 4 instead.\n\nMr Ad Min:\n\nI completed task 55\nNow I will do task 6"
    assert channel_messages(admin['token'], channel1, 0)['messages'][0] == expected_message
    '''
    with pytest.raises(ValueError):
        standup_start(123, channel2) #invalid token
    with pytest.raises(ValueError):
        standup_start(admin['token'], 1234) #invalid channel id

    with pytest.raises(AccessError):
        standup_start(user['token'], channel2) #user is not in channel2

    with pytest.raises(ValueError):
        standup_send(123, channel1, "hElLo?!") #invalid token
    with pytest.raises(ValueError):
        standup_send(admin['token'], 41224, "hEEEEElLooo??") #invalid channel id

    with pytest.raises(AccessError):
        standup_send(user['token'], channel2, "HEIIILLLOI") #user is not in the channel

    channel_join(user['token'], channel2)

    with pytest.raises(AccessError):
        standup_send(user['token'], channel2, "HELLLOOOOO???!?!?!?!") #there is no standup in the channel
