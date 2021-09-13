import hashlib
from datetime import datetime
from werkzeug.exceptions import HTTPException
from datetime import timezone

lastID = 0
chan_id = 10000000000 # assumption is, channels won't go above 10 billion

class ValueError(HTTPException):
    code = 400
    message = 'No message specified'

class Member:

    def __init__(self, email, password, firstName, lastName, handle):

        global lastID
        self.id         = lastID + 1
        lastID          += 1

        self.email      = email
        self.password   = hashlib.sha256(str(password).encode()).hexdigest()
        self.firstName  = firstName
        self.lastName   = lastName
        self.handle     = handle
        self.image      = None

        if len(owners_dictionaries) == 0:
            self.permission = 1
        else:
            self.permission = 3

    @property
    def email(self):
        return self.email

class Channel:

    def __init__(self, owner, name, isPublic):        #owner is dict type members
        global chan_id

        self.ch_id      = chan_id + 1
        chan_id         += 1

        self.owners     = [owner]                    # list of dictionaries, such that specs filled meaning Dict
        self.members    = []                    # list of dictionaries, such that specs filled meaning Dict
        self.name       = name
        self.isPublic   = isPublic
        self.messages   = []                   # list of messages,  such that specs filled meaning Dict
        self.standUpFinish = None           # changed this variable to reflect the time the standup will finish




# note that the passwords below will not actually be stored in plain text, when the User object is
# object is initialized with the given password, it is hashed and then stored in the list

Users_classes = []
Channels_classes = []

channels_dictionaries = []
owners_dictionaries = []
admins_dictionaries = []
members_dictionaries = []


ResetCodes = [] # this is an empty dictionary because we wait till someone calls auth password request
                # contains user's email and code which will be deleted after user resets password


def getMembers():
    global members_dictionaries
    return members_dictionaries

def setMembers(newMembers):
    global members_dictionaries
    members_dictionaries = newMembers

def getAdmins():
    global admins_dictionaries
    return admins_dictionaries

def setAdmins(newAdmins):
    global admins_dictionaries
    admins_dictionaries = newAdmins

def getOwners():
    global owners_dictionaries
    return owners_dictionaries

def setOwners(newOwners):
    global owners_dictionaries
    owners_dictionaries = newOwners

def getChannels():
    global channels_dictionaries
    return channels_dictionaries

def setChannels(newChannels):
    global channels_dictionaries
    channels_dictionaries = newChannels

def getUserDetails(u_id):
    global Users_classes
    if u_id == None:
        raise ValueError("Invalid token or u_id")

    for user in Users_classes:
        if int(user.id) == int(u_id):
            return user

    return None


def setUserDetails(newUserDetails):
    global Users_classes

    for i in range(0, len(Users_classes)):
        if Users_classes[i].id == newUserDetails.id:
            Users_classes[i] = newUserDetails

def getChannelDetails(ch_id):
    global Channels_classes

    for channel in Channels_classes:
        if channel.ch_id == int(ch_id):
            return channel

    raise ValueError("Invalid channel id")


def setChannelDetails(newChannelDetails):
    global Channels_classes

    for i in range(0, len(Channels_classes)):
        if Channels_classes[i].ch_id == newChannelDetails.ch_id:
            Channels_classes[i] = newChannelDetails

def getUserClasses():
    global Users_classes
    return Users_classes

def setUserClasses(newUserClasses):
    global Users_classes
    Users_classes = newUserClasses

def getChannelClasses():
    global Channels_classes
    return Channels_classes

def setChannelClasses(newChannelClasses):
    global Channels_classes
    Channels_classes = newChannelClasses

def getResetCodes():        # list dict
    global ResetCodes
    return ResetCodes

def setResetCodes(code):    # list dict
    global ResetCodes
    ResetCodes = code

def resetData():

    global members_dictionaries, admins_dictionaries, owners_dictionaries, Users_classes, Channels_classes, ResetCodes, lastID, chan_id

    members_dictionaries = []
    admins_dictionaries = []
    ResetCodes = []
    owners_dictionaries = []
    Users_classes = []
    Channels_classes = []
    lastID = 0
    chan_id = 10000000000

def setTestData():
    global members_dictionaries, admins_dictionaries, owners_dictionaries, Users_classes, Channels_classes, channels_dictionaries, lastID

    Users_classes = [
        Member("eilia.key@hotmail.com", "ILOVErats4312", "Eilia", "Keyhanee", 'eiliakeyhanee'),
        Member("Matt.Galinski@gmail.com", "password", "Matt", "Galinski", "mattgalinski"),
        Member("Nichlas.Dingle@yahoo.com", "IdontLikeRats123", "Nicholas", "Dingle", "nicholasdingle"),
        Member("Brett.Jeffers@outlook.com", "IkindaLikeRats321", "Brett", "Jeffers", "brettjeffers")
    ]

    Users_classes[1].permission = 2
    owners_dictionaries = [{'u_id' : 1, 'name_first' : "Eilia", 'name_last' : "Keyhanee"}]
    admins_dictionaries = [{'u_id' : 2, 'name_first' : "Matt", 'name_last' : "Galinski"}]
    members_dictionaries = [{'u_id' : 3, 'name_first' : "Nicholas", 'name_last' : "Dingle"}, {'u_id' : 4, 'name_first' : "Brett", 'name_last' : "Jeffers"}]
    first_channel = Channel(owners_dictionaries[0], "firstChannel", True)
    first_channel.owners.append(admins_dictionaries[0])#eilia, matt, owners
    first_channel.members.append(members_dictionaries[0])# Nicholas is memebr

    msg_id = int(str(first_channel.ch_id) + str(len(first_channel.messages) + 1))
    now = datetime.utcnow().replace(tzinfo=timezone.utc).timestamp()
    first_channel.messages.append({ 'message_id' : msg_id,  'u_id': (admins_dictionaries[0])['u_id'], 'message': "Hello World!", 'time_created': now, 'reacts': [],    'is_pinned': True })

    Channels_classes = [first_channel]  #eilia, matt, owners

    Channels_classes.append(Channel(admins_dictionaries[0], "secondChannelPrivate", False))
    channels_dictionaries = [{'channel_id' : 10000000001,'name': "firstChannel" },{'channel_id' : 10000000002,'name': "secondChannelPrivate" }]
# for the moment we keep this here for the development, so that one can copy past the relevant fileds into the code

'''
messages    = []
dict_message =  { 'message_id' : int,  'u_id': int,     'message': "",  'time_created': datetime(,,,,),
                    'reacts': [{}],    'is_pinned': 0 }

reacts = []
dict_reacts =   {'react_id' : 0, 'u_ids':[], 'is_this_user_reacted': 0 }

channels    = []
dict_channel =  {'channel_id' : 0,'name': "" }

members = []
dict_members =  {'u_id' : 0, 'name_first':"", 'name_last': "" }
'''
