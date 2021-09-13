import jwt
from data import getOwners, getAdmins, getMembers
from Error import AccessError

def generateToken(id):
    return jwt.encode({"u_id" : str(id)}, "1531_Pass_Cut_Off", algorithm='HS256')

def getIdFromToken(token):
    connections = getConnections()
    if not any(token == connection for connection in connections):
        return None

    try:
        return (jwt.decode(token, "1531_Pass_Cut_Off", algorithm='HS256'))['u_id']
    except:
        return None

Connections = []

def getConnections():
    global Connections
    return Connections

def setConnections(newConnections):
    global Connections
    Connections = newConnections

def resetConnections():
    connections = getConnections()
    connections = []
    setConnections(connections)

def setTestConnections(): #it means that only one owner, admin and user is connected
    connections = getConnections()
    owner = (getOwners())[0]
    admin = (getAdmins())[0]
    member = (getMembers())[0]
    connections.append(generateToken(owner['u_id']))
    connections.append(generateToken(admin['u_id']))
    connections.append(generateToken(member['u_id']))
    setConnections(connections)
