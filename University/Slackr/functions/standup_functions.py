from data import *
from tokens import *
from message_functions import message_send
import time
from datetime import datetime, date, timedelta, timezone
import threading

queue = []

def standup_start(token, channel_id):
    global queue
    user = getUserDetails(getIdFromToken(token))
    channel = getChannelDetails(channel_id)

    if channel.standUpFinish != None:
        raise ValueError("A standup is already occuring in the channel")
    allParticipants = channel.owners + channel.members
    if not any(member['u_id'] == user.id for member in allParticipants):
        raise AccessError("Authorised user is not a member of the channel")
    channel.standUpFinish = (datetime.utcnow() + timedelta(minutes=15)).replace(tzinfo=timezone.utc).timestamp()
    threading._start_new_thread(queue_up_baby, (token, channel_id))
    return {'time_finish' : channel.standUpFinish}


def standup_send(token, channel_id, message):
    global queue

    user = getUserDetails(getIdFromToken(token))
    channel = getChannelDetails(channel_id)

    if len(message) > 1000:
        raise ValueError("message too long")
    if channel.standUpFinish == None:
        raise ValueError("channel is not in a standup")
    allParticipants = channel.owners + channel.members
    if not any(member['u_id'] == user.id for member in allParticipants):
        raise AccessError("Authorised user is not a member of the channel")

    queue.append(message)

def standup_active(token, channel_id):
    channel = getChannelDetails(channel_id)
    if channel.standUpFinish == None:
        return {"is_active" : False, "time_finish" : channel.standUpFinish}
    else:
        return {"is_active" : True, "time_finish" : channel.standUpFinish}
    
def queue_up_baby(token, channel_id):
    
    time.sleep(900)
    summary = ""

    for message in queue:
        summary += message + "\n"
    
    message_send(token, channel_id, summary)