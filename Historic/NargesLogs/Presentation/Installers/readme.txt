FOR CLIENT:

-The client needs to run as administrator to function properly.

-The client .exe will be located at: "\NargesLogs\bin"

-The username and password that should be used by the markers is:
		Username: Admin
		Password: Password

FOR SERVER:

-The server must be run as administrator to work.

-The server password is: Eilia2018

-WARNING: entering the wrong password into the server will destroy the database.
	In the event of this happening, please restore one of the automatic backups (details later in this document)
	If no valid backups exist, the server must be reinstalled.

-WARNING: DO NOT CLOSE THE SERVER WITHOUT TERMINATING IT.
	Doing this will leave the database decrypted, the server can only open encrypted databases, so the server will be unable to access the database for the next launch
	If this happens, you may extract any important information from the decrypted database and restore one of the automatic backups (details later in this document)
	To terminate the server properly, type "terminate" into the console. Or, if one or more clients are connected, enter "safeexit".

-Note: one common mistake made by users launching a server, is the accidental clicking of the terminal. This will leave a white spot in the terminal and as with all console applications, will hault the process of the application.
	this will cause any connected clients to freeze as they wait for feedback and eventually stop responding.
	If this happens, please press Ctrl + C on the console to resume the process.

-To restore an automatic backup:
	1) If available, delete the decrypted database (after extracting all valuable data)
	2) Copy paste the desired automatic backup into the bin folder.
	3) Rename the backup to "EncryptedDatabase".

-To host a server that is accessible by devices other than the host, you must port forward (the default port is 3214, but a different port may be used and specified in settings window of client)
	A video port forwarding tutorial is included within the application folder of the server after it is installed.