from data import *
from tokens import *
from Error import AccessError

def find_channel(channel_id):
    channels = getChannelClasses()
    for channel in channels:
        if channel_id == channel.ch_id:
            return channel

    raise ValueError("Channel id doesn't exist")


def is_OwnerAdmin_Member(user_id):

    Owners = getOwners()
    Admins = getAdmins()
    Members = getMembers()

    if  any(user['u_id'] == user_id for user in Owners):
        return 1
    if  any(user['u_id'] == user_id for user in Admins):
        return 1
    if  any(user['u_id'] == user_id for user in Members):
        return 0