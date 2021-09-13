import string, random
from Error import AccessError

# ------------------------------------------------------------------------------
#                           header comments hello
# ------------------------------------------------------------------------------

def test_admin_userpermission_change():
'''
    u_id1, token1 = auth_register("good@email.com", "abcd", "god ", "mod")
    u_id2, token2 = auth_register("bad@email.com", "abcd", "chan ", "user")
'''

    chan_id = channels_create(token1, "chan1", True) # make fake id if needd
    channel_invite(token1, u_id2, chan_id)

    u_id_wrong = "998877"
    wrong_permission = 9

    with pytest.raises(ValueError, match =r".*"):
        admin_userpermission_change(token1, u_id_wrong, 3)

    with pytest.raises(ValueError, match =r".*"):
        admin_userpermission_change(token1, u_id1, wrong_permission)

    with pytest.raises(AccessError, match =r".*"):
        admin_userpermission_change(token2, u_id2, 2)
# ------------------------------------------------------------------------------
