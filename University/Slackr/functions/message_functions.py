from data import *
from tokens import *
from Error import AccessError
from global_scope_functions import is_OwnerAdmin_Member
from datetime import date, time, datetime, timezone

def message_send(token, channel_id, message):

    if len(message) > 1000:
        raise ValueError("message is too long")

    authorised  = getUserDetails(getIdFromToken(token))
    channel     = getChannelDetails(channel_id)

    all_participants = channel.owners + channel.members

    if not any(participant['u_id'] == authorised.id for participant in all_participants):
        raise AccessError("Authorised user not in channel")
    #! IMPORTANT
    #! the msgid is generated so that it takes the channel id (10 billion) and appends the msgid (nth consecutive msg
    #! in the channel) and appends both. The rational is that assum that the number of channels won't exceeds 10 bill
    #! therefore, later on we can split string of number from 0 to 11 to extract channel id.
    msg_id = int(str(channel.ch_id) + str(len(channel.messages) + 1))
    u_m_dict = { 'message_id' : msg_id,  'u_id': authorised.id, 'message':message,'time_created': datetime.utcnow().replace(tzinfo=timezone.utc).timestamp(),
                 'reacts': [],    'is_pinned': 0 }
    channel.messages.insert(0,u_m_dict) #insert the newest message at the beggining, it facilitates channel_messages()

    setChannelDetails(channel)
    return {'message_id': msg_id}


def message_unpin(token, message_id):
    #! see comment in message_send()
    ch_id = int(str(message_id)[0:11])
    channel = getChannelDetails(ch_id)

    user_id = getIdFromToken(token)
    user = getUserDetails(user_id)

    if (not any(owner['u_id'] == user.id for owner in channel.owners)) and user.permission != 1 and user.permission != 2:
        raise ValueError("Permission denied")

    all_participants = channel.owners + channel.members
    if not any(participant['u_id'] == user.id for participant in all_participants):
        raise AccessError("User is not in the channel")

    for i in range(len(channel.messages)):
        if int(channel.messages[i]['message_id']) == int(message_id):
            if channel.messages[i]['is_pinned'] == 0:
                raise ValueError ("Messaged already unpinned")
            else:
                channel.messages[i]['is_pinned'] = 0
                setChannelDetails(channel)
                return

    raise ValueError("Invalid mesage_id")
    

def message_remove(token, message_id):
    chan_id = int(str(message_id)[0:11])
    channel = getChannelDetails(chan_id)
    authorised = getUserDetails(getIdFromToken(token))

    # assumption, we assume that the person who calls this function is either the owner of a message, or, it is a owner
    # of the channel without being an global OwAd, or, the person is a global OwAd who may, or may not be in the channel.
    # Essentially, the access error will be rised if the auth user is a global member without being in the channel.
    # It means that a global OwAd can remove a messge without being inside the channel.
    # Our first assumption was that a global OwAd wouldn't be able to remove/edit a message if he/she wasn't inside the
    # channel. Finally, we decided to follow the requirement's logic

    #assume that the owner and admin of slackr can remove the content even thoguh not in channel
    auth_permission = is_OwnerAdmin_Member(authorised.id)

    for message in channel.messages:
        if int(message['message_id']) == int(message_id):
            if message['u_id'] != authorised.id and (not any(owner['u_id'] == authorised.id for owner in channel.owners)) and auth_permission != 1:
                raise AccessError("Auth user can't not del the message because of its permission")
            channel.messages.remove(message)
            setChannelDetails(channel)
            return

    raise ValueError("message no longer exists")

    # comment out the first if statement bellow in case the assumption changes

    # else if auth_permission == 1 and not any(owner['u_id'] == authorised.id for owner in channel.owners):
    #             raise AccessError("Authorised user is global owner, not chan member any more")

    # if auth user is a global member, and not an owner.
    # We already checked if auth user is owner of the message in first if statement above
    # if auth_permission == 0 and not any(owner['u_id'] == authorised.id for owner in channel.owners):

def message_edit(token, message_id, message):

    chan_id = int(str(message_id)[0:11])
    channel = getChannelDetails(chan_id)
    authorised = getUserDetails(getIdFromToken(token))

    # assumption, we assume that the person who calls this function is either the owner of a message, or, it is a owner
    # of the channel without being an gloval OwAd, or, the person is a global OwAd who may, or may not be in the channel.
    # Essentially, the access error will be rised if the auth user is a global member without being in the channel.
    # It means that a global OwAd can edit a messge without being inside the channel.
    # Our first assumption was that a global OwAd wouldn't be able to edit a message if he/she wasn't inside the
    # channel. Finally, we decided to follow the requirement's logic

    auth_permission = is_OwnerAdmin_Member(authorised.id)
    message_id = int(message_id)
    for message in channel.messages:
        if message['message_id'] == message_id:
            if message['u_id'] != authorised.id and (not any(owner['u_id'] == authorised.id for owner in channel.owners)) and auth_permission != 1:
                raise AccessError("Auth user can't not edit  the message because of its permission")
            else:
                message['message'] = message
                setChannelDetails(channel)
                return

def message_pin(token, message_id):

    chan_id = int(str(message_id)[0:11])
    channel = getChannelDetails(chan_id)
    authorised = getUserDetails(getIdFromToken(token))

    auth_permission = is_OwnerAdmin_Member(authorised.id)

    all_channel_participants = channel.owners + channel.members
    # in this situation we assume that auth user is local owner, we don't account for global permission because
    # of AccessError condition which account for presence of auth user in channel
    if not any(participant['u_id'] == authorised.id for participant in all_channel_participants):
        raise AccessError("Authorised user is not in the channel")

    if not any(owner['u_id'] == authorised.id for owner in channel.owners) and auth_permission != 1:
        raise ValueError("Auth user user isn't an Admin of the channel. Can't pin message ")

    for message in channel.messages:
        if int(message['message_id']) == int(message_id):
            if message['is_pinned'] == 1:
                raise ValueError("Message is already pinned")
            else:
                message['is_pinned'] = 1
                setChannelDetails(channel)
                return
    
    raise ValueError("Message does not exist")
    

def message_sendlater(token, channel_id, message, time_sent):

    if len(message) >= 1000:
        raise ValueError("message is too long")

    authorised  = getUserDetails(getIdFromToken(token))
    channel     = getChannelDetails(channel_id)

    all_participants = channel.owners + channel.members

    if not any(participant['u_id'] == authorised.id for participant in all_participants):
        raise AccessError("Authorised user not in channel")
    #! IMPORTANT
    #! the msgid is generated so that it takes the channel id (10 billion) and appends the msgid (nth consecutive msg
    #! in the channel) and appends both. The rational is that assum that the number of channels won't exceeds 10 bill
    #! therefore, later on we can split string of number from 0 to 11 to extract channel id.
    msg_id = int(str(channel.ch_id) + str(len(channel.messages) + 1))
    u_m_dict = { 'message_id' : msg_id,  'u_id': authorised.id, 'message':message,'time_created': datetime.strptime(time_sent[0:18], "%Y-%m-%dT%H:%M:%S").replace(tzinfo=timezone.utc).timestamp(),
                 'reacts': [],    'is_pinned': 0 }
    channel.messages.insert(0,u_m_dict) #insert the newest message at the beggining, it facilitates channel_messages()

    setChannelDetails(channel)
    return {'message_id': msg_id}

def message_react(token, message_id, react_id):

    react_id = int(react_id)
    if react_id != 1:
        raise ValueError("invalid react_id")

    u_id = getIdFromToken(token)
    if u_id == None:
        raise ValueError("Invalid token")

    chan_id = int(str(message_id)[0:11])
    channel = getChannelDetails(chan_id)
    all_channel_participants = channel.owners + channel.members
    if not any(member['u_id'] == int(u_id) for member in all_channel_participants):
        raise ValueError("Authorised user not in channel")

    for message in channel.messages:
        if int(message['message_id']) == int(message_id):
            
            for react in message['reacts']:
                if react['react_id'] == react_id:
                    if int(u_id) in react['u_ids']:
                        raise ValueError("Message react already exists")
                    else:
                        react['u_ids'].append(int(u_id))
                        setChannelDetails(channel)
                        return
            
            message['reacts'].append({'react_id' : 1, 'u_ids' : [int(u_id)], 'is_this_user_reacted' : True})
            setChannelDetails(channel)
            return
    
    raise ValueError("Invalid channel id")
                    
def message_unreact(token, message_id, react_id):

    react_id = int(react_id)
    if react_id != 1:
        raise ValueError("invalid react_id")

    u_id = getIdFromToken(token)
    if u_id == None:
        raise ValueError("Invalid token")

    chan_id = int(str(message_id)[0:11])
    channel = getChannelDetails(chan_id)
    all_channel_participants = channel.owners + channel.members
    if not any(member['u_id'] == int(u_id) for member in all_channel_participants):
        raise ValueError("Authorised user not in channel")

    for message in channel.messages:
        if int(message['message_id']) == int(message_id):
            
            for react in message['reacts']:
                if react['react_id'] == react_id:
                    if not (int(u_id) in react['u_ids']):
                        raise ValueError("The Authorised user has not reacted to this message")
                    else:
                        react['u_ids'].remove(int(u_id))
                        setChannelDetails(channel)
                        return
            
            raise ValueError("The Authorised user has not reacted to this message")
    
    raise ValueError("Invalid channel id")