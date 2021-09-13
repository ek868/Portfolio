import pytest
import string, random
from Error import AccessError


from auth_functions import *
from channel_functions import *
from message_functions import *
from user_profile_functions import *
from admin_functions import *
# ------------------------------------------------------------------------------
#                           header comments hello
# ------------------------------------------------------------------------------

def test_admin_userpermission_change():

    resetData()
    resetConnections()
    setTestData()
    setTestConnections() # owner, admin and a user are connected, in that order
    connected = getConnections()
    user1 = auth_register("user1@unsw.edu.au", "AfairlysTr0ngpwd", "One", "Sur")
    u_id1 = user1['u_id']
    token1 = user1['token']
    assert str(u_id1).isdigit()
    assert type(token1) is str

    with pytest.raises(ValueError):
        admin_userpermission_change(connected[0]['token'], 56789, 2) #global owner wront u_id

    with pytest.raises(ValueError):
        admin_userpermission_change(connected[0]['token'], u_id1, 321312) #global owner wront perm

    with pytest.raises(AccessError):
        admin_userpermission_change(token1, 3, 1) #newly registered user, wants to promote another user
# ------------------------------------------------------------------------------
