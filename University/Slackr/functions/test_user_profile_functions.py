from data import *
from Error import AccessError
import pytest
import unittest


from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import user_profile
from admin_functions import *


def test_user_profile():
    #BEGIN SETUP
    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    connected = getConnections()
    #END SETUP
    user_id = int(getIdFromToken(connected[0]))
    user_info = user_profile(connected[0], user_id)

    assert user_info['email'] == "eilia.key@hotmail.com"
    with pytest.raises(ValueError): #
        user_profile(connected[0], 53432)

