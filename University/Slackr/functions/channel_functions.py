from data import *
from tokens import *
from datetime import date, time, datetime, timezone
from Error import AccessError
from global_scope_functions import is_OwnerAdmin_Member
from werkzeug.exceptions import HTTPException

# this is line of code is a test to check if commits work, also included in channel_function and
# test_channel_function

# who ever reads this function, you have to understand that we have a list of channels based on dictionaries
# and a list of channel classes. You might think that this is crazy, but we found that having a list of
# channels with few slected infos  in a form of dictionary is restrictive for the development.
# Henceforth, there exists this list of classes for 'channels', 'messages', 'reacts' and 'members' so that we respect
#  the project specs but also, it makes the data manipulation easier for us. That was your 'why the heck section'.


def channel_create(token, name, is_public):
    # Why & How: We check the name lenght, token validity to rise errors, followed by getting list of channel classes
    # and dictionaries which are appended with a new respective element. Return dict with chan id
    if len(name) > 20:
        raise ValueError("channel name is too long")

    channelsClasses = getChannelClasses()
    channelsDicts = getChannels()

    uid = getIdFromToken(token)
    if uid == None:
        raise ValueError("Token is invalid")

    owner = getUserDetails(uid)
    newChannel = Channel({'u_id' : owner.id, 'name_first': owner.firstName, 'name_last':  owner.lastName }, name, is_public)

    channelsClasses.append(newChannel)
    channelsDicts.append({'channel_id' : newChannel.ch_id, 'name' : name})

    setChannelClasses(channelsClasses)
    setChannels(channelsDicts)


    return {'channel_id' : newChannel.ch_id}

def channel_invite(token, channel_id, u_id):
    # We check if channel and token are valid, check if auth user in channel else error. Append user to right category
    channel     = getChannelDetails(channel_id) #return details or None
    channels    = getChannelClasses()
    authorised  = getUserDetails(getIdFromToken(token))
    target      = getUserDetails(u_id) # return details or rise value error


    all_participants = channel.owners + channel.members
    if not any(participant['u_id'] == authorised.id for participant in all_participants):
        raise AccessError("Authorised user not in channel")

    authed_perm = is_OwnerAdmin_Member(target.id)
    if authed_perm == 1:
        channel.owners.append({'u_id' : target.id,'name_first': target.firstName,'name_last': target.lastName })
        setChannelDetails(channel)
    else :
        channel.members.append({'u_id' : target.id,'name_first': target.firstName,'name_last': target.lastName })
        setChannelDetails(channel)


def channel_join(token, channel_id):
    # check details, rise error if needed. Doesn't assume that golbal AdOw can join private channel
    channel     = getChannelDetails(channel_id)
    user        = getUserDetails( getIdFromToken(token))
    authed_perm = is_OwnerAdmin_Member(user.id)

    all_participants = channel.members + channel.owners
    for member in all_participants:
        if int(user.id) == member['u_id']:
            raise ValueError("User is already in the channel")

    if (not channel.isPublic) and authed_perm == 0:
        raise AccessError("This channel is private")

    if authed_perm == 1:
        channel.owners.append({'u_id' : user.id,'name_first': user.firstName,'name_last': user.lastName })
        setChannelDetails(channel)
    else:
        channel.members.append({'u_id' : user.id,'name_first': user.firstName,'name_last': user.lastName })
        setChannelDetails(channel)



def channel_list(token):
    # GetUserDEtails rise error if token is invalid, append channel if user in participants
    user = getUserDetails(getIdFromToken(token))
    channels = getChannelClasses()
    lista = []
    all_participants = []
    for chan in channels:
        owners = chan.owners
        memebrs = chan.members
        all_participants = owners + memebrs

        if  any(participant['u_id'] == user.id for participant in all_participants):
            lista.append({'channel_id' : chan.ch_id,'name': chan.name })

    return {'channels':lista}




def channel_listall(token):
    return {'channels': getChannels()} # getChannels returns list of channels


def channel_details(token, channel_id):

    u_id = getIdFromToken(token)
    if u_id == None:
        raise ValueError("Invalid token")
    else:
        u_id = int(u_id)

    channel = getChannelDetails(channel_id)

    owners = channel.owners
    memebrs = channel.members
    all_participants = owners + memebrs
    print
    if (not any(participant['u_id'] == u_id for participant in all_participants)) and channel.isPublic == False:
        raise AccessError("Authorised user not in channel")
    if (not any(participant['u_id'] == u_id for participant in all_participants)) and channel.isPublic == True:
        raise AccessError("Authorised user not in channel")

    return {'name':channel.name, 'owner_members': channel.owners, 'all_members': channel.members}

def channel_messages(token, channel_id, start):

    try:
        start = int(start)
    except:
        raise ValueError("Start is not an integer")

    channel = getChannelDetails(channel_id)
    user = getUserDetails(getIdFromToken(token))

    all_channel_participants = channel.members + channel.owners

    if start == -1:
        start = 0

    if len(channel.messages) == 0:
        return {'messages': [],'start':0,'end':-1 }

    if start >= len(channel.messages):
        raise ValueError("The start index is larger than the number of messages")
    if not any(participant['u_id'] == user.id for participant in  all_channel_participants ) and channel.isPublic == False:
        raise AccessError("Auth user is not a participant in channel")

    list_messages = []

    end = start
    for i in range(start, start + 50):
        if i >= len(channel.messages):
            break
        if float(channel.messages[i]['time_created']) > float(datetime.utcnow().replace(tzinfo=timezone.utc).timestamp()):
            waiting = channel.messages[i]
            channel.messages.remove(waiting)
            channel.messages.insert(0, waiting)
            continue
        list_messages.append(channel.messages[i])
        end += 1

    if end != start + 50:
        end = -1

    return {'messages':list_messages,'start':start,'end':end }


def channel_addowners(token, ch_id, u_id):
    channel = getChannelDetails(ch_id)
    target = getUserDetails(u_id)

    if any(owner['u_id'] == u_id for owner in channel.owners):
        raise ValueError("User is already owner")


    connections = getConnections()
    if token not in connections:
        raise ValueError("token not logged in")


    origin = getUserDetails(getIdFromToken(token)) # this will return a ErrorValue in any case no need to assess bellow
    auth_permission =  is_OwnerAdmin_Member(origin.id)
    if not any(owner['u_id'] == origin.id for owner in channel.owners) and auth_permission != 1:
        raise AccessError("Permission denied")

    try:
        channel.members.remove({'u_id':u_id, 'name_first':target.firstName, 'name_last':target.lastName})
    except:
        pass

    channel.owners.append({'u_id':u_id, 'name_first':target.firstName, 'name_last':target.lastName})
    setChannelDetails(channel)

def channel_removeowners(token, ch_id, u_id):
    channel = getChannelDetails(ch_id)
    target = getUserDetails(u_id)

    connections = getConnections()
    if token not in connections:
        raise ValueError("token not logged in")

    origin = getUserDetails(getIdFromToken(token)) # this will return a ErrorValue in any case no need to assess bellow
    auth_permission =  is_OwnerAdmin_Member(origin.id)
    if not any(owner['u_id'] == origin.id for owner in channel.owners) and auth_permission != 1:
        raise AccessError("Permission denied")

    for i in range(len(channel.owners)):
        if channel.owners[i]['u_id'] == u_id:
            del channel.owners[i]
            channel.members.append({'u_id' : u_id, 'name_first' : target.firstName, 'name_last' : target.lastName})
            setChannelDetails(channel)
            return
    raise ValueError("The user is already not an owner of the channel")

def channel_leave(token, ch_id):
    channel = getChannelDetails(ch_id)
    target_id = int(getIdFromToken(token))
    target = getUserDetails(target_id)

    if channel == None:
        raise ValueError("Invalid channel")

    all_participants = channel.members + channel.owners
    if not any(participant['u_id'] == target_id for participant in all_participants):
        raise AccessError("User user not in channel")
    # if any of above returns None, rise Error, else iterate from members and owners of channels to the job
    if target_id == None:
        raise ValueError("Invalid token")

    try:
        channel.members.remove({'u_id':target_id, 'name_first':target.firstName, 'name_last':target.lastName})
    except:
        pass

    try:
        channel.owners.remove({'u_id':target_id, 'name_first':target.firstName, 'name_last':target.lastName})
    except:
        pass
    setChannelDetails(channel)

