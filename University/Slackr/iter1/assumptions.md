Channel_messages: 

    - Assume that the function returns 'messages' in its dictionary, which is a list of up to 50 size with each element being a message. 

    - Assume that a ValueError is raised if the given start value is less than zero. 

Channel_leave: 

    - Assume that channel_leave does nothing if the user attempting to leave is not a part of the channel. 

Channel_join: 

    - A value error is raised when a bad token is passed 

Channel_details: 

    - Assume that 'owner_members' and 'all_members' are lists of tokens 

Channels_list: 

    - Assume that the function returns a list of channel IDs 

    - Assume that if the given token is not recognized then a value error is raised. 

Channels_listall: 

    - Assume that the function returns a list of channel IDs 

    - Assume that if the given token is not recognized then a value error is raised. 

Channels_create: 

    - Assume that a ValueError is raised if an invalid token is passed to the function 

    - Assume that the user that creates the channel also automatically joins the channel as owner. 

User_profile_setemail: 

    - Assume that a ValueError is returned if the given token is invalid. 

Standup_start: 

    - Assume that the standup message at the end of the standup is sent by the person who called the standup and that it consists of: 

        <user_name>: 

        Every message the user sent during the standup in chronological order 

    For every user that sent a message during the standup 

    - Assume that the function returns a datetime variable. 

Message_send: 

    - Assume that  the function raises an AccessError if it is called during a standup 

    - Assume that standup_send is called every time instead of this function during a standup 

Message_remove: 

    - any variable in auth_register if bad, then message_send fails

message_sendlater:

    - function fails if un-authorised user sends a message to the channel

message_react:

    -React id is basic 1 or 0, which will activate a thumbs up

message_unpin:

    -to pin and unpin, the website appends $$important$$ to string