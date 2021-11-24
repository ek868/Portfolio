using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace NargesLogs_Server
{

    class ConnectionFunctions
    {

        //The following code is used to simulate an 'enter' key press when the program is not in focus.
        //The code is from: https://stackoverflow.com/questions/9016087/net-call-to-send-enter-keystroke-into-the-current-process-which-is-a-console
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        const int VK_RETURN = 0x0D;
        const int WM_KEYDOWN = 0x100;
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public static void ClearCurrentConsoleLine()
        {

            //Clears the current console line.
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);

        }

        public static void Terminate()
        {

            //Encrypts the database and closes the program.
            Program.EncryptDataBase(Global_Information.password, 3214);
            Environment.Exit(0);

        }

        //Connection objects used by multiple functions.
        NetworkStream stream;
        TcpListener MainServer = new TcpListener(3214);

        public void Launch()
        {
           
            //Starts the listener.
            MainServer.Start();
            Console.WriteLine(DateTime.Now + ": Server Launched.");

            //Uses second thread to constantly handle inputs while executing the program.
            Thread InputThread = new Thread(ActiveInput);
            InputThread.Start();

            Console.Write("Command: ");
            bool first = true;

            //Infinite loop to catch new users whenever they connect.
            for (;;)

                //Loop checks over and over if a client is attempting to join.
                if (MainServer.Pending())
                {

                    ClearCurrentConsoleLine();

                    //Simulates enter to continue the program.
                    if (first)
                    {
                        //The following code is from: https://stackoverflow.com/questions/9016087/net-call-to-send-enter-keystroke-into-the-current-process-which-is-a-console
                        var hWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                        PostMessage(hWnd, WM_KEYDOWN, VK_RETURN, 0);
                        //---------------------------------------------------------------------------------------------------------------------------------------------------------
                        first = false;
                    }

                    //Uses new thread to handle the client while continuing listening for new clients.
                    Console.WriteLine(DateTime.Now + ": Connecting to a client...");
                    Thread HandleThread = new Thread(HandleClient);
                    HandleThread.Start();
                    Thread.Sleep(100);

                }
            
        }

        public void ActiveInput()
        {

            //Infinite loop to listen for new commands.
            while (true)
            {

                //Reads command.
                string input = Console.ReadLine();

                //Checks command and executes the appropriate command.
                if (input == "terminate")
                {

                    //Encrpyts the database and closes the application.
                    Program.EncryptDataBase(Global_Information.password, 3214);
                    Environment.Exit(0);

                }

                else if (input == "safeclose")
                {
                    //Waits for all processes to be complete before terminating, to avoid creating errors with half finished commands.
                    while (true)
                    {

                        if(processing == false)
                        {

                            Program.EncryptDataBase(Global_Information.password, 3214);
                            Environment.Exit(0);

                        }

                    }

                }

                else if(input == "help")
                {

                    //Lists all of the available commands.
                    Console.WriteLine(" ");
                    Console.WriteLine("terminate - terminates the server.");
                    Console.WriteLine("safeclose - waits for all processes to complete before terminating the server.");
                    Console.WriteLine(" ");
                    Console.Write("Command: ");

                }

            }

        }

        //Objects and variables used in many functions.
        DatabaseFunctions DbFunctions = new DatabaseFunctions();
        public static bool processing = false;

        public void HandleClient()
        {

            //Accepts the connection and saves to local TCP client.
            TcpClient Server = new TcpClient();
            Server = MainServer.AcceptTcpClient();
            Console.WriteLine(DateTime.Now + ": Connected to client.");

            //Loop listens for new client commands.
            while (true)
            {
                
                //Connects client stream with local networkstream.
                stream = Server.GetStream();
                Byte[] Packet = new Byte[65536];

                Console.Write("Command: ");

                //Waits on the 'stream.Read()' line until a command is recieved, while waiting, processing is false and the server is able to be terminated. Otherwise, the safeexit command will wait.
                processing = false;
                stream.Read(Packet, 0, Packet.Length);
                processing = true;

                ClearCurrentConsoleLine();
                Console.WriteLine(" ");

                //Simulates enter.
                //The following code is from: https://stackoverflow.com/questions/9016087/net-call-to-send-enter-keystroke-into-the-current-process-which-is-a-console
                var hWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                PostMessage(hWnd, WM_KEYDOWN, VK_RETURN, 0);
                //---------------------------------------------------------------------------------------------------------------------------------------------------------

                //This code checks the tcp connection.
                //The following code is from: https://stackoverflow.com/questions/1387459/how-to-check-if-tcpclient-connection-is-closed/19706302
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections().Where(x => x.LocalEndPoint.Equals(Server.Client.LocalEndPoint) && x.RemoteEndPoint.Equals(Server.Client.RemoteEndPoint)).ToArray();

                if (tcpConnections != null && tcpConnections.Length > 0)
                {
                    TcpState stateOfConnection = tcpConnections.First().State;
                    if (stateOfConnection != TcpState.Established)
                    {
                        Console.WriteLine(DateTime.Now + ": Client Disconnected.");
                        Console.Write("Command: ");
                        break;
                    }

                }
                //----------------------------------------------------------------------------------------------------------------------------------------------------------
                Console.WriteLine(DateTime.Now + ": Command recieved.");

                //Filters command string and checks what the command is, guiding it to the correct function.
                string recieved = Encoding.ASCII.GetString(Packet);
                string command = recieved.Remove(recieved.IndexOf("\0"));

                if (command == "3214~COMMAND#1")
                    ExecuteGenericCommand(1, "RetrieveUsernames", null);

                else if (command == "3214~COMMAND#2")
                    ExecuteGenericCommand(2, "RetrievePasswords", null);

                else if (command == "3214~COMMAND#3")
                    ExecuteGenericCommand(3, "RetrievePatientDetails", "ID");

                else if (command == "3214~COMMAND#4")
                    ExecuteGenericCommand(4, "RetrievePatientDetails", "Name");

                else if (command == "3214~COMMAND#5")
                    ExecuteGenericCommand(5, "RetrievePatientDetails", "Family_Name");

                else if (command == "3214~COMMAND#6")
                    ExecuteGenericCommand(6, "RetrievePatientDetails", "Occupation");

                else if (command == "3214~COMMAND#7")
                    ExecuteGenericCommand(7, "RetrievePatientDetails", "Sex");

                else if (command == "3214~COMMAND#8")
                    ExecuteGenericCommand(8, "RetrievePatientDetails", "DoB");

                else if (command == "3214~COMMAND#9")
                    ExecuteGenericCommand(9, "RetrievePatientDetails", "Aboriginal_or_TSI");

                else if (command == "3214~COMMAND#10")
                    ExecuteGenericCommand(10, "RetrievePatientDetails", "Home_Phone");

                else if (command == "3214~COMMAND#11")
                    ExecuteGenericCommand(11, "RetrievePatientDetails", "Mobile_Number");

                else if (command == "3214~COMMAND#12")
                    ExecuteGenericCommand(12, "RetrievePatientDetails", "Work_Phone");

                else if (command == "3214~COMMAND#13")
                    ExecuteGenericCommand(13, "RetrievePatientDetails", "Emergency_Contact");

                else if (command == "3214~COMMAND#14")
                    ExecuteGenericCommand(14, "RetrievePatientDetails", "Medicare_Number");

                else if (command == "3214~COMMAND#15")
                    ExecuteGenericCommand(15, "RetrievePatientDetails", "Healthcare_Number");

                else if (command == "3214~COMMAND#16")
                    ExecuteGenericCommand(16, "RetrievePatientDetails", "Residential_Address");

                else if (command == "3214~COMMAND#17")
                    ExecuteGenericCommand(17, "RetrievePatientDetails", "Postal_Address");

                else if (command == "3214~COMMAND#18")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #18...");
                    Command_SendClientImages();
                    Console.WriteLine(DateTime.Now + ": Command #18 Complete.");

                }

                else if (command == "3214~COMMAND#19")
                    ExecuteGenericCommand(19, "RetrieveVisitInfo", "Patient_ID");

                else if (command == "3214~COMMAND#20")
                    ExecuteGenericCommand(20, "RetrieveVisitInfo", "Date");

                else if (command == "3214~COMMAND#21")
                    ExecuteGenericCommand(21, "RetrieveVisitInfo", "Reason");

                else if (command == "3214~COMMAND#22")
                {

                    Ping();

                    Console.WriteLine(DateTime.Now + ": Executing Command #22...");
                    Command_SendVisitNotes();
                    Console.WriteLine(DateTime.Now + ": Command #22 Complete.");

                }

                else if (command == "3214~COMMAND#23")
                    ExecuteGenericCommand(23, "RetrieveVisitInfo", "ID");

                else if (command == "3214~COMMAND#24")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #24...");

                    Ping();
                    Command_SendVisitImages();

                    Console.WriteLine(DateTime.Now + ": Command #24 Complete.");

                }

                else if (command == "3214~COMMAND#25")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #25...");

                    Ping();
                    Command_RecievePatientInfo();

                    Console.WriteLine(DateTime.Now + ": Command #25 Complete.");

                }

                else if (command == "3214~COMMAND#26")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #26...");

                    Ping();
                    Command_RecieveClientImage();

                    Console.WriteLine(DateTime.Now + ": Command #26 Complete.");

                }

                else if (command == "3214~COMMAND#27")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #27...");

                    Ping();
                    Command_RemovePatient();

                    Console.WriteLine(DateTime.Now + ": Command #27 Complete.");

                }

                else if (command == "3214~COMMAND#28")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #28...");

                    Ping();
                    Command_AddVisit();
                    
                    Console.WriteLine(DateTime.Now + ": Command #28 Complete.");

                }

                else if (command == "3214~COMMAND#29")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #29...");

                    Ping();
                    Command_RecieveVisitImages();
                    
                    Console.WriteLine(DateTime.Now + ": Command #29 Complete.");

                }

                else if (command == "3214~COMMAND#30")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #30...");

                    Ping();
                    Command_RecieveVisitNotes();
                    
                    Console.WriteLine(DateTime.Now + ": Command #30 Complete.");

                }

                else if (command == "3214~COMMAND#31")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #31...");

                    Ping();
                    Command_RemoveVisit();

                    Console.WriteLine(DateTime.Now + ": Command #31 Complete.");

                }

                else if (command == "3214~COMMAND#32")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #32...");

                    Ping();
                    Command_RemoveImage();
                    
                    Console.WriteLine(DateTime.Now + ": Command #32 Complete.");

                }

                else if (command == "3214~COMMAND#33")
                {

                    Console.WriteLine(DateTime.Now + ": Client disconnected.");
                    Console.Write("Command: ");
                    break;

                }

                else if (command == "3214~COMMAND#34")
                {

                    Console.WriteLine(DateTime.Now + ": Executing Command #34...");

                    Ping();
                    Command_UpdateVisitInfo();

                    Console.WriteLine(DateTime.Now + ": Command #34 Complete.");

                }

            }

            //Closes the connection and the thread.
            stream.Close();
            Thread.CurrentThread.Abort();

        }

        public void ExecuteGenericCommand(int cmdnumber, string method, string parameter)
        {

            //This is a highly dynamic function that can handle a variety of generic commands.
            //Conversion is required for later.
            object[] parameters = { parameter };

            //Calls a dynamic method based on function parameter using 'MethodInfo' class.
            DatabaseFunctions DbFunctions = new DatabaseFunctions();
            Console.WriteLine(DateTime.Now + ": Executing Command #" + cmdnumber.ToString() + "...");
            MethodInfo Method = DbFunctions.GetType().GetMethod(method);

            //Information goes to this list.
            List<string> information = new List<string>();

            //If parameters are required, they are specified to the custom method.
            //The method is invoked from the class 'DatabaseFunctions' through the DatabaseFunctions object known as 'DbFunctions' and a cast is used to specify the return type.
            if (parameter != null)
                information = (List<string>)Method.Invoke(DbFunctions, parameters);

            //If no parameters are required, then null is sent instead.
            else
                information = (List<string>)Method.Invoke(DbFunctions, null);

            //If no information was returned, then 'NULL' is sent to the client to indicate this.
            if (information.Count == 0)
                Send("NULL", 1);

            //Otherwise, a loop is used to send all of the information individually.
            for (int i = 0; i < information.Count; i++)
            {

                Send(information[i], information.Count);
                WaitForPing();

            }

            Console.WriteLine(DateTime.Now + ": Command #" + cmdnumber.ToString() + " Complete.");

        }

        public void Command_SendClientImages()
        {

            //Images and IDs retrieved from database.
            List<string> patientImagePath = DbFunctions.RetrievePatientDetails("Image_Path");
            List<string> correspondingIDs = DbFunctions.RetrievePatientDetails("ID");

            Console.WriteLine(DateTime.Now + ": Sending the number of images...");

            //Sends the number of images.
            int numberofimages = patientImagePath.Count;
            byte[] numpacket = Encoding.ASCII.GetBytes(numberofimages.ToString());
            stream.Write(numpacket, 0, numpacket.Length);
            stream.Flush();

            Console.WriteLine(DateTime.Now + ": Number of images sent.");
            WaitForPing();

            //Goes through each imagepath.
            for (int i = 0; i < patientImagePath.Count; i++)
            {

                //Sends the Patient ID of the Patient image that is about to be sent.
                Console.WriteLine(DateTime.Now + ": Sending ID #" + i + "...");
                Send(correspondingIDs[i], numberofimages);
                Console.WriteLine(DateTime.Now + ": ID #" + i + " sent.");
                WaitForPing();

                //If an image path exists, then it is searched for and sent to the client.
                if (patientImagePath[i] != "")
                {

                    Console.WriteLine(DateTime.Now + ": Sending Image #" + i + "...");

                    try
                    {

                        SendFile(patientImagePath[i], "");
                        Console.WriteLine(DateTime.Now + ": Image #" + i + " sent.");

                    }

                    //If the search fails, then nothing is sent to the client and the error is put in the console terminal.
                    catch
                    {

                        Send("NULL", 1);
                        Console.WriteLine(DateTime.Now + ": Image #" + i + " was not found.");

                    }
                    
                }

                //Otherwise, 'NULL' is sent to the client to indicate this.
                else
                {

                    Send("NULL", 4);
                    WaitForPing();

                }

            }

        }

        public void Command_SendVisitNotes()
        {
            
            //Retrieves the note paths from the database.
            List<string> unfilterednotepaths = DbFunctions.RetrieveVisitInfo("Notes");

            Console.WriteLine(DateTime.Now + ": Sending number of notes...");

            //Sends the number of notes to the client.
            int numberofnotes = unfilterednotepaths.Count;
            byte[] numpacket = Encoding.ASCII.GetBytes(numberofnotes.ToString());
            stream.Write(numpacket, 0, numpacket.Length);
            stream.Flush();

            Console.WriteLine(DateTime.Now + ": Number of notes sent.");
            WaitForPing();

            //Goes through each note path.
            for (int i = 0; i < unfilterednotepaths.Count; i++)
            {

                Console.WriteLine(DateTime.Now + ": Sending note file #" + i + "...");

                //Sends the file and the owner of the file to the client.
                try
                {

                    string owner = "~~OWNER" + unfilterednotepaths[i].Substring(unfilterednotepaths[i].LastIndexOf(@"\"));
                    SendFile(unfilterednotepaths[i], owner);

                }

                //If this fails, then an error is thrown and the user is informed.
                catch
                {

                    byte[] nullmessage = Encoding.ASCII.GetBytes("NULL");
                    stream.Write(nullmessage, 0, nullmessage.Length);
                    stream.Flush();
                    Console.WriteLine(DateTime.Now + ": No note files were sent.");
                    return;

                }

                Console.WriteLine(DateTime.Now + ": Note file #" + i + " Sent.");

            }

        }

        public void Command_SendVisitImages()
        {

            //Reads the owner ID of the visit.
            Console.WriteLine(DateTime.Now + ": Sending patient ID...");
            byte[] patientIDpacket = Read(65536);
            string patientIDdecoded = Encoding.ASCII.GetString(patientIDpacket);
            string patientID = patientIDdecoded.Remove(patientIDdecoded.IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Patient ID sent");

            //Retrieves image paths and owner IDs.
            List<string> UnfilteredImages = DbFunctions.RetrieveVisitInfo("Image_Paths");
            List<string> PatientIDs = DbFunctions.RetrieveVisitInfo("Patient_ID");

            //Saves relevant visits to this list.
            List<int> RelevantVisits = new List<int>();

            //Goes through each to see if the corresponding owner ID is the correct owner ID, adding all visits that belong to the patient to the list above.
            for (int i = 0; i < UnfilteredImages.Count; i++)
            {

                if (PatientIDs[i] == patientID)
                    RelevantVisits.Add(i);

            }

            //Sends the number of images to the client.
            Console.WriteLine(DateTime.Now + ": Sending number of images...");
            byte[] numofimages = Encoding.ASCII.GetBytes(RelevantVisits.Count.ToString());
            stream.Write(numofimages, 0, numofimages.Length);
            stream.Flush();


            Console.WriteLine(DateTime.Now + ": Number of images sent.");
            WaitForPing();

            //Retrieves visit IDs.
            List<string> VisitIDs = DbFunctions.RetrieveVisitInfo("ID");

            //Goes through each relevant visit.
            for (int i = 0; i < RelevantVisits.Count; i++)
            {

                //Reads the required visit ID.
                Console.WriteLine(DateTime.Now + ": Sending visit ID for visit #" + i + "...");
                byte[] VisitIDpacket = Encoding.ASCII.GetBytes(VisitIDs[RelevantVisits[i]]);
                stream.Write(VisitIDpacket, 0, VisitIDpacket.Length);
                stream.Flush();

                Console.WriteLine(DateTime.Now + ": Visit ID sent.");
                WaitForPing();

                //Saves images for that visit to this list.
                List<string> Images = new List<string>();

                //Adds the images for the visit with the required visit ID to the list above.
                while (UnfilteredImages[RelevantVisits[i]].ToCharArray().Count() > 0)
                {

                    string sub = UnfilteredImages[RelevantVisits[i]].Remove(UnfilteredImages[RelevantVisits[i]].IndexOf("~~NEXT_IMAGE_PATH"));
                    Images.Add(sub);
                    UnfilteredImages[RelevantVisits[i]] = UnfilteredImages[RelevantVisits[i]].Substring((sub.Count() + 17));

                }

                //Sends the number of images for that visit to the client.
                Console.WriteLine(DateTime.Now + ": Sending number of images for visit #" + i + "...");
                int numberofimages = Images.Count;
                byte[] numpacket = Encoding.ASCII.GetBytes(numberofimages.ToString());
                stream.Write(numpacket, 0, numpacket.Length);
                stream.Flush();

                Console.WriteLine(DateTime.Now + ": Number of images sent.");
                WaitForPing();

                //Sends each image to the client.
                for (int x = 0; x < Images.Count; x++)
                {
                    string u = Images[x].Remove(Images[x].IndexOf("."));

                    FileStream OpenFile = File.OpenRead(Directory.GetCurrentDirectory() + Images[x]);
                    byte[] Image = new byte[(int)OpenFile.Length];
                    OpenFile.Read(Image, 0, (int)OpenFile.Length);
                    OpenFile.Close();

                    Console.WriteLine(DateTime.Now + ": Sending image size of image #" + x + " for visit #" + i + "...");
                    byte[] ImageSize = new byte[Image.Length.ToString().ToCharArray().Length];
                    ImageSize = Encoding.ASCII.GetBytes(Image.Length.ToString() + "~INCOMING_IMAGE_SIZE");
                    stream.Write(ImageSize, 0, ImageSize.Length);
                    stream.Flush();
                    Console.WriteLine(DateTime.Now + ": Image size sent.");

                    WaitForPing();

                    Console.WriteLine(DateTime.Now + ": Sending owner information of image #" + x + " for visit #" + i + "...");
                    byte[] OwnerPacket = new byte[10];
                    OwnerPacket = Encoding.ASCII.GetBytes(u);
                    stream.Write(OwnerPacket, 0, OwnerPacket.Length);
                    stream.Flush();
                    Console.WriteLine(DateTime.Now + ": Owner information sent.");
                    WaitForPing();

                    Console.WriteLine(DateTime.Now + ": Sending image #" + x + " for visit #" + i + "...");
                    stream.Write(Image, 0, Image.Length);
                    stream.Flush();
                    Console.WriteLine(DateTime.Now + ": Image sent.");
                    WaitForPing();
                }

            }

        }

        public void Command_RecievePatientInfo()
        {

            //Puts info into this list.
            List<string> patientinfo = new List<string>(15);
            Console.WriteLine(DateTime.Now + ": Recieving patient information...");

            //Reads the recieved info in a loop and adds them to the list above.
            for (int i = 0; i < 15; i++)
            {

                patientinfo.Add(Encoding.ASCII.GetString(Read(65536)));

            }

            //Updates the database using the entries of the list above.
            Console.WriteLine(DateTime.Now + ": Patient information recieved.");
            DbFunctions.UpdatePatient(patientinfo[0], patientinfo[1], patientinfo[2], patientinfo[3], patientinfo[4], patientinfo[5], patientinfo[6], patientinfo[7], patientinfo[8], patientinfo[9], patientinfo[10], patientinfo[11], patientinfo[12], patientinfo[13], patientinfo[14]);

        }

        public void Command_RecieveClientImage()
        {

            //Reads the image size.
            Console.WriteLine(DateTime.Now + ": Recieving image size...");
            byte[] imagesizepacket = Read(65536);
            string imagesizedecoded = Encoding.ASCII.GetString(imagesizepacket);
            int imagesize = int.Parse(imagesizedecoded.Remove(imagesizedecoded.IndexOf("~INCOMING_FILE_SIZE")));
            Console.WriteLine(DateTime.Now + ": Image size recieved.");

            //Reads the name of the client.
            Console.WriteLine(DateTime.Now + ": Recieving image...");
            byte[] recievedpacket = Read(imagesize);
            string namefinder = Encoding.ASCII.GetString(recievedpacket);
            string name = namefinder.Substring(namefinder.IndexOf("~CLIENTNAME")).Substring(11);
            byte[] image = recievedpacket.Take(recievedpacket.Length - (11 + name.Length)).ToArray();
            Console.WriteLine(DateTime.Now + ": Image recieved.");

            //Saves images to the Client Images folder.
            Console.WriteLine(DateTime.Now + ": Saving image...");

            if (Directory.Exists(Directory.GetCurrentDirectory() + @"..\..\..\..\..\" + name) == false)
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Images\" + name);

            using (FileStream filestream = new FileStream(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Images\" + name + @"\img_1.jpg", FileMode.Create))
            {

                filestream.Write(image, 0, image.Length);
                filestream.Close();

            }

            Console.WriteLine(DateTime.Now + ": Image saved.");

            //Updates the client images entry in the database.
            DbFunctions.UpdateClientImage(@"..\..\..\..\..\Client Images\" + name + @"\img_1.jpg", name.Remove(name.IndexOf(" ")), name.Substring(name.IndexOf(" ")).Substring(1));

        }

        public void Command_RemovePatient()
        {

            //Reads requested patient ID.
            Console.WriteLine(DateTime.Now + ": Recieving patient ID...");
            byte[] recievedpacket = Read(65536);
            string id = Encoding.ASCII.GetString(recievedpacket).Remove(Encoding.ASCII.GetString(recievedpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Patient ID recieved.");

            //Retrieve Patient IDs, Names, and Family Names.
            List<string> PatientIDs = DbFunctions.RetrievePatientDetails("ID");
            List<string> Names = DbFunctions.RetrievePatientDetails("Name");
            List<string> Family_Names = DbFunctions.RetrievePatientDetails("Family_Name");

            //Finds the entry with the correct ID and saves their name.
            string name = "";

            for (int i = 0; i < PatientIDs.Count; i++)
            {
                if (PatientIDs[i] == id)
                {
                    name = Names[i] + " " + Family_Names[i];
                }
            }

            //Attempts to delete all of their saved images from the image folders.
            try
            {

                Console.WriteLine(DateTime.Now + ": Deleting images...");
                Directory.Delete(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Images\" + name, true);
                Directory.Delete(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + name, true);
                Console.WriteLine(DateTime.Now + ": Images deleted.");

            }

            //If this process fails throws an error.
            catch
            {

                Console.WriteLine(DateTime.Now + ": Failed to delete images.");

            }

            //Deletes all patient note files from note file folder.
            Console.WriteLine(DateTime.Now + ": Deleting notes...");

            foreach (string filepath in Directory.EnumerateFiles(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Notes"))
            {

                string filename = filepath.Substring(filepath.LastIndexOf(@"\"));

                if (filename.Remove(filename.IndexOf("_")).Substring(1) == "U" + id)
                    File.Delete(filepath);

            }

            Console.WriteLine(DateTime.Now + ": Notes deleted.");

            //Removes the patient from the database.
            DbFunctions.RemovePatient(id);

        }

        public void Command_AddVisit()
        {
            
            //Saves new visit info into this list.
            string[] visitinfo = new string[3];
            Console.WriteLine(DateTime.Now + ": Recieving new visit information...");

            //Uses loop to read and filter new visit info as well as save them to the list above.
            for (int i = 0; i < 3; i++)
            {

                byte[] recievedpacket = Read(65536);
                visitinfo[i] = Encoding.ASCII.GetString(recievedpacket).Remove(Encoding.ASCII.GetString(recievedpacket).IndexOf("\0"));

            }

            Console.WriteLine(DateTime.Now + ": New visit information recieved.");

            //Creates the new entry in the database using collected info and saves the new ID to a string, which is then sent back to the client.
            Console.WriteLine(DateTime.Now + ": Sending generated ID of new visit...");
            string id = DbFunctions.CreateVisit(visitinfo[0], visitinfo[1], visitinfo[2]);
            stream.Write(Encoding.ASCII.GetBytes(id), 0, id.Length);
            stream.Flush();

            Console.WriteLine(DateTime.Now + ": Generated ID sent.");
            WaitForPing();

        }

        public void Command_RecieveVisitImages()
        {

            //Reads patient name.
            Console.WriteLine(DateTime.Now + ": Recieving patient name...");
            byte[] Patientpacket = Read(65536);
            string patient = Encoding.ASCII.GetString(Patientpacket).Remove(Encoding.ASCII.GetString(Patientpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Patient name recieved.");

            //Reads visit ID.
            Console.WriteLine(DateTime.Now + ": Recieving visit ID...");
            byte[] VisitIDpacket = Read(65536);
            string visitid = Encoding.ASCII.GetString(VisitIDpacket).Remove(Encoding.ASCII.GetString(VisitIDpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Visit ID recieved.");

            //Reads image number.
            Console.WriteLine(DateTime.Now + ": Recieving image number...");
            byte[] ImageNumpacket = Read(65536);
            string imagenum = Encoding.ASCII.GetString(ImageNumpacket).Remove(Encoding.ASCII.GetString(ImageNumpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Image number recieved.");

            //Reads image size.
            Console.WriteLine(DateTime.Now + ": Recieving image size...");
            byte[] imagesizepacket = Read(65536);
            int imagesize = int.Parse(Encoding.ASCII.GetString(imagesizepacket).Remove(Encoding.ASCII.GetString(imagesizepacket).IndexOf("\0")));
            Console.WriteLine(DateTime.Now + ": Image size recieved.");

            //Reads image.
            Console.WriteLine(DateTime.Now + ": Recieving image...");
            byte[] img = Read(imagesize);
            Console.WriteLine(DateTime.Now + ": Image Recieved.");

            //Saves image to the client visit images folder.
            Console.WriteLine(DateTime.Now + ": Saving image...");

            if (Directory.Exists(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + patient) == false)
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + patient);

            using (FileStream filestream = new FileStream(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + patient + @"\" + visitid + imagenum + ".jpg", FileMode.Create))
            {

                filestream.Write(img, 0, img.Length);
                filestream.Dispose();
                filestream.Close();

            }

            Console.WriteLine(DateTime.Now + ": Image saved.");

            //Updates database with new visit image.
            DbFunctions.UpdateVisitImages(@"..\..\..\..\..\Client Visit Images\" + patient + @"\" + visitid + imagenum + ".jpg", visitid);

        }

        public void Command_RecieveVisitNotes()
        {

            //Reads patient ID.
            Console.WriteLine(DateTime.Now + ": Recieving patient ID...");
            byte[] PatientIDpacket = Read(65536);
            string patientID = Encoding.ASCII.GetString(PatientIDpacket).Remove(Encoding.ASCII.GetString(PatientIDpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Patient ID recieved.");

            //Reads visit ID.
            Console.WriteLine(DateTime.Now + ": Recieving visit ID...");
            byte[] VisitIDpacket = Read(65536);
            string visitID = Encoding.ASCII.GetString(VisitIDpacket).Remove(Encoding.ASCII.GetString(VisitIDpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Visit ID recieved.");

            //Reads file size.
            Console.WriteLine(DateTime.Now + ": Recieving file size...");
            byte[] filesizepacket = Read(65536);
            string filesize = Encoding.ASCII.GetString(filesizepacket).Remove(Encoding.ASCII.GetString(filesizepacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": File size recieved.");

            //Reads file.
            Console.WriteLine(DateTime.Now + ": Recieving note file...");
            byte[] filepacket = Read(int.Parse(filesize));
            Console.WriteLine(DateTime.Now + ": Note file sent.");

            //Saves file to visit notes folder.
            Console.WriteLine(DateTime.Now + ": Saving note file...");

            if (Directory.Exists(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Notes") == false)
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Notes");

            using (FileStream filestream = new FileStream(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Notes\U" + patientID + "_V" + visitID + ".txt", FileMode.Create))
            {

                filestream.Write(filepacket, 0, filepacket.Length);
                filestream.Close();

            }

            Console.WriteLine(DateTime.Now + ": Note file saved.");

            //Updates database with new visit notes file.
            DbFunctions.UpdateVisitNotes(@"..\..\..\..\..\Client Notes\U" + patientID + "_V" + visitID + ".txt", visitID);

        }

        public void Command_RemoveVisit()
        {

            //Reads visit ID.
            Console.WriteLine(DateTime.Now + ": Recieving visit ID...");
            byte[] idpacket = Read(65536);
            string id = Encoding.ASCII.GetString(idpacket).Remove(Encoding.ASCII.GetString(idpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Visit ID recieved.");

            //Retrieves VisitIDs, Visit owner IDs, Patient IDs, Patient names, and Patient family names.
            List<string> VisitIDs = DbFunctions.RetrieveVisitInfo("ID");
            List<string> PatientIDs_visits = DbFunctions.RetrieveVisitInfo("Patient_ID");
            List<string> PatientIDs = DbFunctions.RetrievePatientDetails("ID");
            List<string> Names = DbFunctions.RetrievePatientDetails("Name");
            List<string> Family_Names = DbFunctions.RetrievePatientDetails("Family_Name");

            //Finds the correct visit.
            int relevantID = 0;

            for (int i = 0; i < VisitIDs.Count; i++)
            {

                if (VisitIDs[i] == id)
                {

                    relevantID = int.Parse(PatientIDs_visits[i]);

                }

            }

            //Finds correct patient name.
            string name = "";

            for (int i = 0; i < PatientIDs.Count; i++)
            {

                if (int.Parse(PatientIDs[i]) == relevantID)
                {

                    name = Names[i] + " " + Family_Names[i];

                }

            }

            //Finds the file names of the visit images.
            List<string> filenames = new List<string>();

            foreach (string filepath in Directory.EnumerateFiles(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + name))
            {

                string filename = filepath.Substring(filepath.LastIndexOf(@"\") + 1);

                if (filename.Remove(filename.IndexOf(".") - 1) == id)
                    filenames.Add(filename);

            }

            //Deletes the files with the names listed above.
            Console.WriteLine(DateTime.Now + ": Deleting visit images...");

            foreach (string filename in filenames)
                File.Delete(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + name + @"\" + filename);

            Console.WriteLine(DateTime.Now + ": Visit images deleted.");

            //Finds the note files of the current visit.
            List<string> filenames_notes = new List<string>();

            foreach (string filepath in Directory.EnumerateFiles(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Notes"))
            {

                string filename = filepath.Substring(filepath.LastIndexOf(@"\") + 1);

                if (filename.Substring(filename.IndexOf("_") + 2).Remove(filename.Substring(filename.IndexOf("_") + 2).IndexOf(".")) == id)
                    filenames_notes.Add(filename);

            }

            //Deletes the files with the names listed above.
            Console.WriteLine(DateTime.Now + ": Deleting visit notes...");

            foreach (string filename in filenames_notes)
                File.Delete(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Notes\" + filename);

            Console.WriteLine(DateTime.Now + ": Visit notes deleted.");

            //Removes the visit from the database.
            DbFunctions.RemoveVisit(id);

        }

        public void Command_RemoveImage()
        {

            //Reads the image ID.
            Console.WriteLine(DateTime.Now + ": Recieving image ID...");
            byte[] idpacket = Read(65536);
            string imageid = Encoding.ASCII.GetString(idpacket).Remove(Encoding.ASCII.GetString(idpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Image ID recieved.");

            //Reads the patient ID.
            Console.WriteLine(DateTime.Now + ": Recieving patient ID...");
            byte[] pacientidpacket = Read(65536);
            string pacientid = Encoding.ASCII.GetString(pacientidpacket).Remove(Encoding.ASCII.GetString(pacientidpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Patient ID recieved.");

            //Reads the patient name.
            Console.WriteLine(DateTime.Now + ": Recieving patient name...");
            byte[] namepacket = Read(65536);
            string name = Encoding.ASCII.GetString(namepacket).Remove(Encoding.ASCII.GetString(namepacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Patient name recieved.");

            //Reads the visit ID.
            Console.WriteLine(DateTime.Now + ": Recieving visit ID...");
            byte[] visitIDpacket = Read(65536);
            string visitID = Encoding.ASCII.GetString(visitIDpacket).Remove(Encoding.ASCII.GetString(visitIDpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Visit ID recieved.");

            //Retrieves the visit IDs and image paths.
            List<string> IDs = DbFunctions.RetrieveVisitInfo("ID");
            List<string> Image_Paths = DbFunctions.RetrieveVisitInfo("Image_Paths");
            string ImagePath = null;
            int relevantentry = 0;

            //Finds the correct visit entry and saves its image path.
            for (int i = 0; i < IDs.Count; i++)
                if (IDs[i] == visitID)
                    relevantentry = i;

            ImagePath = Image_Paths[relevantentry];

            //Isolates the desired image path within the imagepaths entry.
            string DispImagePath = ImagePath;
            int index = 0;

            for (int i = 1; i < int.Parse(imageid.Remove(imageid.IndexOf("."))); i++)
            {
                index += DispImagePath.IndexOf("NEXT_IMAGE_PATH") + 15;
                DispImagePath = DispImagePath.Substring(DispImagePath.IndexOf("NEXT_IMAGE_PATH") + 15);
            }

            string NewImagePath1 = ImagePath.Remove(index);
            ImagePath = ImagePath.Substring(ImagePath.IndexOf("~~U" + pacientid + "V" + visitID));
            string NewImagePath2 = "";

            //Deletes the file in this desired image path.
            Console.WriteLine(DateTime.Now + ": Deleting image...");
            File.Delete(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + name + @"\" + visitID + imageid);
            Console.WriteLine(DateTime.Now + ": Image deleted.");

            //Renames all of the files to account for one file's deletion.
            Console.WriteLine(DateTime.Now + ": Renaming files...");

            //Goes through each file within the folder.
            for (int i = 0; i < Directory.GetFiles(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + name).Count(); i++)
            {

                //Reads the image ID of the file.
                string[] filepaths = Directory.GetFiles(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + name);
                string filename = filepaths[i].Substring(filepaths[i].LastIndexOf(@"\") + 1);
                int fileid = int.Parse(filename.Substring(filename.IndexOf(".") - 1).Remove(1));

                //If the image ID was above the one that was removed, it is decreased by one to account for the file's removal.
                if (fileid > int.Parse(imageid.Remove(imageid.IndexOf("."))))
                {

                    fileid--;
                    char[] newfilenamechar = filename.ToCharArray();
                    newfilenamechar[(filename.IndexOf(".") - 1)] = fileid.ToString().ToCharArray()[0];
                    filename = "";

                    foreach (char x in newfilenamechar)
                    {
                        filename += x;
                    }

                    //The file is the rewritten with the new name.
                    using (FileStream readstream = new FileStream(filepaths[i], FileMode.Open))
                    {

                        byte[] file = new byte[readstream.Length];
                        readstream.Read(file, 0, file.Length);
                        readstream.Close();
                        File.Delete(filepaths[i]);

                        using (FileStream writestream = new FileStream(Directory.GetCurrentDirectory() + @"..\..\..\..\..\Client Visit Images\" + name + @"\" + filename, FileMode.Create))
                        {

                            writestream.Write(file, 0, file.Length);

                        }

                    }

                    //The new file path is then added onto the imagepath field of the visit.
                    NewImagePath2 += "~~U" + pacientid + "V" + fileid + @"..\..\..\..\..\Client Visit Images\" + name + @"\" + filename + "~~NEXT_IMAGE_PATH";

                }

            }

            //The new imagepath field is reconstructed and the database is updated.
            Console.WriteLine(DateTime.Now + ": Files renamed.");
            string newimagepath = NewImagePath1 + NewImagePath2;
            DbFunctions.RemoveVisitImage(newimagepath, visitID);

        }

        public void Command_UpdateVisitInfo()
        {

            //Reads the visit ID.
            Console.WriteLine(DateTime.Now + ": Recieving visit ID...");
            byte[] idpacket = Read(65536);
            string id = Encoding.ASCII.GetString(idpacket).Remove(Encoding.ASCII.GetString(idpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Visit ID recieved.");

            //Reads the new date.
            Console.WriteLine(DateTime.Now + ": Recieving visit date...");
            byte[] datepacket = Read(65536);
            string date = Encoding.ASCII.GetString(datepacket).Remove(Encoding.ASCII.GetString(datepacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Visit date recieved.");

            //Reads the new reason.
            Console.WriteLine(DateTime.Now + ": Recieving visit reason...");
            byte[] reasonpacket = Read(65536);
            string reason = Encoding.ASCII.GetString(reasonpacket).Remove(Encoding.ASCII.GetString(reasonpacket).IndexOf("\0"));
            Console.WriteLine(DateTime.Now + ": Visit reason recieved.");

            //Updates the database using the new date and new reason.
            DbFunctions.UpdaterVisitInfo(id, date, reason);

        }

        public void SendFile(string path, string additionalmessage)
        {

            //path is used to read the desired file.
            FileStream OpenFile = File.OpenRead(Directory.GetCurrentDirectory() + path);
            byte[] file = new byte[(int)OpenFile.Length + additionalmessage.Length];
            OpenFile.Read(file, 0, (int)OpenFile.Length);

            //The additional message is added onto the packet.
            for (int i = 0; i < additionalmessage.Length; i++)
            {
                byte[] encodedmessage = Encoding.ASCII.GetBytes(additionalmessage);
                file[(int)OpenFile.Length + i] = encodedmessage[i];
            }

            //The packet size is sent to the client.
            byte[] filesize = new byte[file.Length.ToString().ToCharArray().Length];
            filesize = Encoding.ASCII.GetBytes(file.Length.ToString() + "~INCOMING_FILE_SIZE");
            stream.Write(filesize, 0, filesize.Length);
            stream.Flush();
            WaitForPing();

            //The packet is sent to the client.
            stream.Write(file, 0, file.Length);
            stream.Flush();
            WaitForPing();
            OpenFile.Close();

        }

        public void Send(string message, int numberofmessages)
        {
            
            //The message is encoded in ascii and sent to the client via a packet and a stream.
            byte[] Packet = new byte[message.Length];
            Packet = Encoding.ASCII.GetBytes((message + "~" + numberofmessages.ToString()));
            stream.Write(Packet, 0, Packet.Length);
            stream.Flush();

        }

        public byte[] Read(int size)
        {

            //Loop is used to ensure all data is read before Ping.
            byte[] packet = new byte[size];
            bool dataleft = true;
            int bytesread = 0;

            while (dataleft)
            {

                //The data is read byte by byte until there is no more data left.
                stream.Read(packet, bytesread, 1);
                bytesread++;

                //If no more data is left, waits 10 milliseconds and checks again to avoid missing bytes due to slow internet connection.
                if(stream.DataAvailable == false)
                {

                    Thread.Sleep(10);

                    if (stream.DataAvailable == false)
                        dataleft = false;

                }

            }

            Ping();
            return packet;

        }

        public void WaitForPing()
        {

            //Loop used to wait for a generic Ping message indicating to the server the data was recieved and processed and that the client is ready for more data.
            while (true)
            {

                byte[] PingReader = new byte[9];
                stream.Read(PingReader, 0, 9);
                string Ping = Encoding.ASCII.GetString(PingReader);

                if (Ping == "3214~PING")
                    break;

                else { }

            }

        }

        public void Ping()
        {

            //Sends a generic Ping message indicating to the client that the data was recieved and processed and that the server is ready for more data.
            byte[] PingPacket = new byte[9];
            PingPacket = Encoding.ASCII.GetBytes("3214~PING");
            stream.Write(PingPacket, 0, 9);
            stream.Flush();

        }

    }

}
