As a group we used serveral techniques listed bellow:

-Python linting
-White box testing
-Black boc testing
-Pytest testing
-Manual testing of the integration between frontend and backend

All the tests were writen, and modiffied if needed after the implementation of backend functions. Tests were run on
each function that has been implemented.
A debugging phase was needed in most of the cases to make sure that the tests are well implemented or the functions
do not generate bugs.

Linting allowed us to quickly pick up the funcition details if it was defined in a different file.

Ideas of python coverage were rised among team members.

------------------------------------------------------------------------------------------------------------------------
Below you can find all the user acceptance criteria:
------------------------------------------------------------------------------------------------------------------------

Website interface: As a new slackr user, I want to be able to sign up with my unsw details (email, password, name)
so that I have an account I can return to later on:
- New user, open window with student/staff email, new password, first and last name window
- checks if email is valid and password is longer than 6 caracters
- the user given names must be less than 50 and longer than 0
- store user's information
- log is user so he/she can experience the website

Website interface:As a slackr user, i want to sign in quickly with my email and password so that i don’t lose time
on filling extra information :
- Store and list user's password, and email
- In a new window, the user should be able to write his email, then password
- Upen connections, the user can use Slackr with all the saved details and options for the previous use
- Upon failure, ask the use to write connections one more time


Website interface:As a slackr user, i want to reset my password by only filling an email window, and receive
an activation code, so that i can use my account when I lose the password:
- Store and list user's password, and email
- Ask user's email and checks if its valid
- Generate random code and send it to user's emai
- Once received, user writes the code in a window which checks code validity
- If allowed, user can change the password with specific requirements (check client's req)

Channel: As a user, I would like my list of channels to display a channel name so that I can easily identify
the channels of which I am a part.
- Keep track of all the channels created on the server
- A user should be able to join a public channel regardless of his global permissions
- User's permissions are transfered to channel's permissions
- A user can't access private channel with 'member' permission
- A user can access private channel with global user permission 'Admin' or 'Owner'


Channel: As a user, I want to be promoted by the owner or admin of the channel, so that I can give a hand in the
 moderation of the channel:
- Check if user is already an owner of the channel
- User shouldn't become owner of the channel if he/she is normal user and isn't in the channel
- once appointed as new owner of channel, user should be able to promote other users to owner
- If user has a 'member' authorarisation, and is promoted to owner of the channel, then, this promotion doesn't translate
to different channels


Channel: As a user, I would like to be able to see a list of the channels of which I am a part, so that I can keep track
of my channels and access them quickly when I want to send a message.
- Store user's details
- When asked for, list all channels the user is part of
- The user can click on a channel in newly generated list (after fron tend implementation)
- After clicking on channel, the user can send, invite, and leave channel, all in all, do all the actions specified n

Channel: As a user, I would like to be able to create channels so that I can facilitate communication among employees.
- User can click on a window, in menu bar to create channel
- Asks for name, and public / private channel status
- Sets channel's details to user's specification
- user joins the channel as owner regardless of his her global user permission
- User should be able to add, remove, promote memebers and send messages,

Channel: As an administrator, I would like to be able to decide whether a channel is public or private so that I
have control over who has access to information and communication channels in the workplace:
- As per above assurance, user should be able to make channel private or public

Channel: As the owner of the channel, i want to remove admins or owners of the channel, so that they won’t have
access to the project when they transfer onto another project:
- Owner of channel can make other channel users, owners of the channel
- If another user is made owner, he she can moderate the channel, and access all the functions of a channel owner
meaning (pin, remove, edit etc..)

Channel: As a user, I would like the ability to see who is in the channels of which I am a part so that I can know
who will possibly see the messages I send
- Store all the members of a channel
- One should be able to access list of all participants in the channel

Channel: As a channel user, i don’t want to have user’s pops in the chat, so that it doesn’t waste space on my
screen and disturbs me on my work:
- Make it impossible for other users to spam the channel's chat so that it doesn't pollute the channel's feed

Channel: As a slackr user, i want to be able to access basic channel details so that one can know who is in the channel.
- Store the member's details
- Print the selected user details
- Make sure that the selected user is a valid user


Channel: As a user, I would like the ability to see who is in charge of each channel so that I can know who to
message if I have problems with or questions about the channel.
- Store a list of owner of the channel
- List should be updated whenever owner leaves, is kicked out, or someone is promoted to owner of the channel
- Only memebrs of the channel should be able to access this information

Channel: As a user, I would like the ability to leave a channel so that I can remove myself from channels
that are no longer relevant to me or that I am no longer interested in.
- Keep track of user's memberships
- Once user decide to leave channel, he she is removed of the list of members or owners of this channel
- If this is a public channel, then user can join back the channel
- If channel is private, then someone has to add the user back into channel

Website: As an administrator, I would like to be able to change who is and is not in charge of a channel,
so that I can ensure the various channels of communication in the workplace are properly Regulated.
- Keep track of all Global Owner, admins, and memebrs of the server
- Admin is able to promote member to admin
- Owner is able to promote memebr or admin to owner
- Lower permissions can't affect the upper permisions (member can't demote owner)
- Check if targer user is already an admin or owner

Messages: As a logged in user, I want to send messages in the channels i’m part of, so that we I can communicate
with my team:
- Participant in a channel should be able to send message no longer than 1000 characters long
- Messages are stored so taht all participants are able to read the message.
- outsiders aren't able to read messages

Messages: As a user of a channel, I want that admins can remove my messages upon request, so that they can
delete obsolete content:
- Global admin / owner of server and channel owner can remove the user's message
- Author of the message can remove the message
- The messages must disapear from the list of messages without impacting the list

Messages: As the author of a message, I want to be able to edit my messages, so that i won’t need to rewrite it
and pollute the chat feed
- Global admin / owner of server and channel owner can remove the user's message
- Author of the message can eddit the message
- Outsiders with member permission can't eddit the message
- Once eddited, the message appears with the right eddit in the channel feed.

Messages: As a user in the channel, I want to react to my peer’s messages with a thumbs up or an icon, so i can express
my approval with without writing a message:
- Given a valid member in the channel, he she can react to a message
- All channel members can see the user's reaction to the message
- One can un react the message


Messages: AS a user of a channel, I want to unreact the messages which i reacted to in the past, so that I can remove a
 reaction when i change my mind.
- Given a valid member in the channel, he she can unreact to a message
- All channel members can see the user's unreaction to the message
- One can un unreact the message


Messages: As a user, I want to pin the messages which i find to be important:
- caller of this functionality needs to be a global Owner or Admin, or a channel owner
- given a list of messages in a channel, user chooses a message to pin

Messages: As a user, I want to unpin the messages which i previously pinned, so that it won’t be displayed any more.
- given that the message is pinned, Owner of the channel can unpin the message
- Change is seen by all the memebrs in the channel

Messages: As a chanel user, I want to send my message with a delay, so that other members will read it at a set time.
- member of channel can send a message with delay
- Keep track of the delay in the backend
- Once delay ended, the message appears in the channel's feed so that all the memebrs can see it
- Outsiders can't do this action

Channel: As an Channel owner or memebr, I would like the ability to add people to a channel so that I can make sure all of
the relevant employees for a particular channel always see the content of that channel:
- Anyone can invide people to the channel
- Condition applies whethere person who invites another user is owner or simple memeber of the channel
- The guest is invited straight away to the channel

Messages: As a user, I would like to be able to see the messages I've sent so that I can see any mistakes I've made
and have a record of what I've sent to others at my workplace
- keep a track of meesages in the channel
- Member can access all the messages, eddited
- Deleted messages don't appear

User profile: As a user, I want to edit my nickname which will be seen by everyone, so that I can add important
information about my position in the group project (leader, dev, sales):
- User displays a nickname on server and channels
- Original nickname is made of appended first and last names
- User can modify the nick name, make sure to be less than 50 characters long
- The nick name is updated in all the server

User profile: As a user, I want to edit my credentials, such as the email, name, and personal picture, so
that my account keeps up with the updates of my personal information.
- given that details are saved in the server, owner of the account can change the details of his accoutn
- Once updated, the details are restored in the server with the new information
- If user changes password or email, that change should be reflected during login or password reset
- User has to connected with new credentials


Stand_up: As a user, I would like the ability to initiate 15min standups so that I can see an organized summary of
here my teammates are in regards to the work we are doing.
- During 15 min, messages from the given channel are stored in a list
- Everyone can access the messages after the end of stand up
- Information in the list can't be edited, transformed
-

Search: As a user, I want to have a message search option, so that the website can bring up all the messages which
contain the keywords.
- Searches through all the channels the user is part of
- There is no life update on the search, the user has to click on the search magnifying glass to start search
- User is provided with list of messages with contain the exact string.
- User is given an Error message if nothing matches the search


