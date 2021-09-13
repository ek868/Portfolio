import hashlib
import string
import random
import re

from data import *
from tokens import getConnections, setConnections, generateToken, getIdFromToken
from werkzeug.exceptions import HTTPException

#emailf section:
import smtplib
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
# ------------------------------------------------------------------------------

def auth_register(email, password, name_first, name_last):

    #raising the specified exceptions
    if not email_valid(email):
        raise ValueError("Invalid email address")
    if not email_unique(email):
        raise ValueError("Email already in use")

    valid_password(password)
    valid_names(name_first, name_last)

    #retrieving info
    usersClasses = getUserClasses()
    connections = getConnections()

    #generating handle
    handle = generate_handle(name_first, name_last)

    #creating and adding the user and their token to the connections list
    newUser = Member(email, password, name_first, name_last, handle)
    newToken = generateToken(newUser.id)
    usersClasses.append(newUser)
    connections.append(newToken)

    #appending the appropriate dictionary
    if newUser.permission == 1:
        owners = getOwners()
        owners.append({'u_id' : newUser.id, 'name_first' : newUser.firstName, 'name_last' : newUser.lastName})
        setOwners(owners)
    else:
        members = getMembers()
        members.append({'u_id' : newUser.id, 'name_first' : newUser.firstName, 'name_last' : newUser.lastName})
        setMembers(members)

    #setting the global data
    setUserClasses(usersClasses)
    setConnections(connections)

    #returning the required information
    return {'u_id' : newUser.id, 'token' : str(newToken)}

def auth_login(email, password):

    if not email_valid(email): #checking that the email is valid
        raise ValueError("invalid email")

    #loading data
    users = getUserClasses()
    connections = getConnections()

    #looking for the user with the given email
    u_id = None
    for user in users:
        if user.email == email and user.password == hashlib.sha256(str(password).encode()).hexdigest():
            u_id = user.id
            token = generateToken(u_id)

    #Connecting user and returning info if possible, otherwise give an error.
    if u_id == None:
        raise ValueError("incorrect email or password")
    else:
        connections.append(token)
        setConnections(connections)
        return {'u_id' : u_id, 'token' : str(token)}

def auth_logout(token):
    #if the token is invalid then return false
    if getIdFromToken(token) == None:
        return {'is_success' : False}
    connections = getConnections()
    for connection in connections:
        if connection == token:

            connections.remove(connection)
            setConnections(connections)
            return {'is_success' : True}  #if the connection was found then remove the connection and return true

    return {'is_success' : False} #otherwise return false

def auth_passwordreset_request(email):
    # gets list of User classes
    users = getUserClasses()

    if not email_valid(email):
        raise ValueError("Invalid email")

    #checks if email appears in the list of User.email
    if not any(user.email == email for user in users):
        raise ValueError("Email not found") # email doesn't exist in our database

    code = generate_rand_code()
    # appends dictionary with email and code to list of rest codes
    codes = getResetCodes()
    codes.append({'email': email, 'code':code})
    setResetCodes(codes)

    # send an email to user who requested pass workd reset
    mail_content = 'You recently sent a request to reset your Slackr password.'\
    'please, use this code on the Slackr website to reset your account: \n\n'\
    + code + '\n \n Thanks for using Slackr, \n Sincerly,\n The Dev Team.\n'
    sender_address = 'dev.quantastic@gmail.com'
    sender_pass = 'Misiek21+'
    receiver_address = email
    message = MIMEMultipart()
    message['From'] = sender_address
    message['To'] = receiver_address
    message['Subject'] = 'Slackr Password resset code'
    message.attach(MIMEText(mail_content, 'plain'))
    session = smtplib.SMTP('smtp.gmail.com', 587)
    session.starttls()
    session.login(sender_address, sender_pass)
    text = message.as_string()
    session.sendmail(sender_address, receiver_address, text)
    session.quit()


def auth_passwordreset_reset(reset_code, new_password):

    valid_password(new_password)
    # gets list of classes and reset coodes to comapre the
    users = getUserClasses()
    codes = getResetCodes()

    # if everything goes well, we encode user's password into his user details
    valid = False
    for idx, code in enumerate(codes):
        if codes[idx]['code'] == reset_code:
            email = codes[idx]['email']
            for user in users:
                if user.email == email:
                    user.password = hashlib.sha256(str(new_password).encode()).hexdigest()
            valid = True
    # if flag is valid, then delete the reset code from list, else, generate value error
    if valid == True:
        for idx, code in enumerate(codes):
            if code['email'] == email:
                del codes[idx]
        setResetCodes(codes)
        setUserClasses(users)
    else:
        raise ValueError("reset_code")

def generate_handle(name_first, name_last):

    #cat the two names together
    handle = str(name_first).lower() + str(name_last).lower()
    if len(handle) > 20:
        handle = handle[0:20]


    #if the handle already exists then keep adding a number two it until it no longer exists
    if not check_handle(handle):

        handleSuff = 1
        if len(handle) >= 20:
            handle = handle[0:20 - len(str(handleSuff))]
        handle += str(handleSuff)
        while (not check_handle(handle)):
            handleSuff += 1
            if len(handle) >= 20:
                handle = handle[0:20 - len(str(handleSuff))]
            handle += str(handleSuff)

    return handle

#go through the current users and check if the given handle already exists
def check_handle(handle):

    users = getUserClasses()

    for user in users:
        if user.handle == handle:
            return False

    return True

#check if an email is valid
def email_valid(email):
    if (re.search('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$', email)):
        return True
    else:
        return False

#check if an email is unique
def email_unique(email):
    users = getUserClasses()
    for user in users:
        if user.getEmail() == email:
            return False

    return True

def valid_password(password):
    if len(password) < 6:
        raise ValueError("Password too short")

def valid_names(name_first, name_last):
    if len(name_first) < 1 or len(name_first) > 50:
        raise ValueError("Unacceptable first name length")
    if len(name_last) < 1 or len(name_last) > 50:
        raise ValueError("Unacceptable last name length")

def generate_rand_code(): # used for auth register
    text = string.hexdigits
    return ''.join(random.choice(text) for i in range(7))
