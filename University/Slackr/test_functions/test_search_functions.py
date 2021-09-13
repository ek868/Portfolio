import pytest
from Error import AccessError

from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import *
from admin_functions import *

def test_search():
    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    u_id1 = user1['u_id']
    token1 = user1['token']
    channel1 = channels_create(token1, "channel1", True)
    message_send(token1, channel1, "Hello World!")
    assert search(token1, "Hello") == {"Hello World!"}
    assert search(token1, "bye") == {""}
