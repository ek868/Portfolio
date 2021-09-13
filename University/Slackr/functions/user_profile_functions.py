import global_scope_functions
from tokens import getIdFromToken
from data import getUserDetails, setUserDetails, getUserClasses, setUserClasses, getMembers, getAdmins, getOwners
import re

def user_profile(token, u_id):
    user_id = int(getIdFromToken(token))
    user = getUserDetails(user_id)
    classes = getUserClasses()
    if not any(user.id == u_id for user in classes):
        raise ValueError("U_id doesn't exists")

    # the function doesn't raise a value error directly because getUserDetails() will do it when an invalid u_id
    # is fead to the function
    return { 'email': user.email, 'name_first':user.firstName, 'name_last':user.lastName, 'handle_str':user.handle }

def user_profile_setname(token, name_first, name_last):

    if len(name_first) < 1 or len(name_first) > 50:
        raise ValueError("name_last lenght is incorrect")

    if len(name_last) < 1 or len(name_last) > 50:
        raise ValueError("name_last lenght is incorrect")

    user    = getUserDetails(getIdFromToken(token))
    user.firstName = name_first
    user.lastName = name_last
    setUserDetails(user)

def user_profile_setemail(token, email):
    if not email_valid(email):
        raise ValueError("Invalid email address")

    user = getUserDetails(getIdFromToken(token))
    members = getUserClasses()

    for member in members:
        if member.email == email:
            raise ValueError("Email is already in use")

    user.email = email
    setUserDetails(user)

def user_profile_sethandle(token, handle_str):
    if len(str(handle_str)) < 3 or len(str(handle_str)) > 20:
        raise ValueError("Unacceptable handle length")

    user = getUserDetails(getIdFromToken(token))
    members = getUserClasses()

    for member in members:
        if member.handle == str(handle_str):
            raise ValueError("handle is already in use")

    user.handle = handle_str
    setUserDetails(user)

def users_all(token):

    if getIdFromToken(token) == None:
        raise ValueError("Invalid Token")

    members = getMembers()
    admins= getAdmins()
    owners = getOwners()

    return {'users': members + admins + owners}

def email_valid(email):
    if (re.search('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$', email)):
        return True
    else:
        return False