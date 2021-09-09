using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Input;

namespace NargesLogs
{

    class ConnectionFunctions
    {

        //Declaring variables that will be used in multiple functions.
        NetworkStream stream;

        //-----------------------------------------------------------------------------Send Commands------------------------------------------------------------------//

        public string[] SendGenericCommand(string command)
        {

            //Connects to server and sends command.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send(command);

            //Reads the information from the server.
            byte[] recievedpacket = read(65536);
            string recievedpacketdecoded = Decode(recievedpacket);

            //Checks if the server has indicated that there is no information there.
            if (recievedpacketdecoded.Remove(recievedpacketdecoded.IndexOf("~")) == "NULL")
                return new string[0];

            //Loads all of the information into an array and returns the array.
            int numofmessages = int.Parse(recievedpacketdecoded.Remove(recievedpacketdecoded.IndexOf("\0")).Substring(recievedpacketdecoded.Remove(recievedpacketdecoded.IndexOf("\0")).IndexOf("~")).Substring(1));
            string[] messages = new string[numofmessages];
            string firstmessage = recievedpacketdecoded.Remove(recievedpacketdecoded.IndexOf("\0")).Remove(recievedpacketdecoded.Remove(recievedpacketdecoded.IndexOf("\0")).IndexOf("~"));
            messages[0] = firstmessage;

            for (int i = 1; i < numofmessages; i++)
            {

                recievedpacket = read(65536);
                recievedpacketdecoded = Decode(recievedpacket);
                string message = recievedpacketdecoded.Remove(recievedpacketdecoded.IndexOf("\0")).Remove(recievedpacketdecoded.Remove(recievedpacketdecoded.IndexOf("\0")).IndexOf("~"));
                messages[i] = message;
                if (messages[i] == "NULL")
                    return null;

            }

            return messages;

        }

        public void SendPatientInfo()
        {

            //Connects to the server and sends command 25.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#25");
            WaitForPing();

            //Saves the current patient information into an array.
            string[] info = new string[15];
            info[0] = CurrentPatient.Name;
            info[1] = CurrentPatient.FamilyName;
            info[2] = CurrentPatient.Occupation;
            info[3] = CurrentPatient.Sex;
            info[4] = CurrentPatient.DoB;
            info[5] = CurrentPatient.AoTSI.ToString();
            info[6] = CurrentPatient.HomePhone;
            info[7] = CurrentPatient.MobilePhone;
            info[8] = CurrentPatient.WorkPhone;
            info[9] = CurrentPatient.EmergencyContact;
            info[10] = CurrentPatient.Medicare;
            info[11] = CurrentPatient.Healthcare;
            info[12] = CurrentPatient.ResidentialAddress;
            info[13] = CurrentPatient.PostalAddress;

            if (CurrentPatient.NewPatient == false)
                info[14] = CurrentPatient.NewPatient.ToString() + CurrentPatient.ID;

            else
                info[14] = CurrentPatient.NewPatient.ToString();

            //Sends each piece of information individually.
            for (int i = 0; i < 15; i++)
            {

                Send(info[i]);
                WaitForPing();

            }

        }

        public void SendClientImage(string path, string additionalmessage)
        {

            //Connects to the server and sends command 26.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#26");
            WaitForPing();

            //Reads the client image.
            FileStream OpenFile = File.OpenRead(path);
            byte[] file = new byte[(int)OpenFile.Length + additionalmessage.Length];
            OpenFile.Read(file, 0, (int)OpenFile.Length);

            //Adds the requested additional message to the end of the image.
            for (int i = 0; i < additionalmessage.Length; i++)
            {
                byte[] encodedmessage = Encoding.ASCII.GetBytes(additionalmessage);
                file[(int)OpenFile.Length + i] = encodedmessage[i];
            }

            OpenFile.Close();

            //Calculates image size.
            byte[] filesize = new byte[file.Length.ToString().ToCharArray().Length];
            filesize = Encoding.ASCII.GetBytes(file.Length.ToString() + "~INCOMING_FILE_SIZE");

            //Sends the image size to the server.
            stream.Write(filesize, 0, filesize.Length);
            stream.Flush();
            WaitForPing();

            //Sends the image to the server.
            stream.Write(file, 0, file.Length);
            stream.Flush();
            WaitForPing();

        }

        public void RemovePatient(string ID)
        {

            //Connects to the server and sends command 27.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#27");
            WaitForPing();

            //Sends the ID of the patient that needs to be removed.
            Send(ID);
            WaitForPing();

        }

        public void AddVisit(string Date, string Reason, string PatientID)
        {

            //Connects to the server and sends command 28.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#28");
            WaitForPing();

            //Sends the Date of the new visit.
            Send(Date);
            WaitForPing();

            //Sends the reason for the new visit.
            Send(Reason);
            WaitForPing();

            //Sends the ID of the patient with the new visit.
            Send(PatientID);
            WaitForPing();

            //Reads the allocated ID for the visit and sets it as the current visit ID.
            byte[] VisitIDPacket = read(65536);
            CurrentVisit.ID = int.Parse(Encoding.ASCII.GetString(VisitIDPacket).Remove(Encoding.ASCII.GetString(VisitIDPacket).IndexOf("\0")));

        }

        public void UpdateVisitInfo(string ID, string date, string reason)
        {

            //Connects to the server and sends command 34.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#34");
            WaitForPing();

            //Sends the ID of the visit that is to be updated.
            Send(ID);
            WaitForPing();

            //Sends the date of the visit.
            Send(date);
            WaitForPing();

            //Sends the reason of the visit.
            Send(reason);
            WaitForPing();

        }

        public void SendVisitImages(string filepath, string Patient, string VisitID, int ImageNum, Window window)
        {

            //Sets the cursor to 'wait' for UI feedback.
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.SetCursor(Cursors.Wait);
            });

            //Connects to the server and sends command 29.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#29");
            WaitForPing();

            //Sends the Patient ID.
            Send(Patient);
            WaitForPing();

            //Sends the visit ID.
            Send(VisitID);
            WaitForPing();

            //Sends the Image number (used to identify specific images in a visit).
            Send(ImageNum.ToString());
            WaitForPing();

            //Reads the image.
            FileStream filestream = new FileStream(filepath, FileMode.Open);
            byte[] file = new byte[filestream.Length];
            filestream.Read(file, 0, file.Length);
            filestream.Close();

            //Sends the Image size.
            byte[] imagesize = Encoding.ASCII.GetBytes(file.Length.ToString());
            stream.Write(imagesize, 0, imagesize.Length);
            stream.Flush();
            WaitForPing();

            //Sends the image.
            stream.Write(file, 0, file.Length);
            stream.Flush();
            WaitForPing();

        }

        public void SendVisitNotes(RichTextBox rtb)
        {

            //Connects to the server and sends command 30.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#30");
            WaitForPing();

            //The following code reads richtextbox text and saves it into a string. 
            string rtfFromRtb = string.Empty;

            //Code from: https://social.msdn.microsoft.com/Forums/sqlserver/en-US/149deaab-122d-4385-aa24-e44a94cd281d/how-can-get-rtf-text-from-wpf-rich-text-box-control?forum=wpf
            using (MemoryStream ms = new MemoryStream())
            {
                TextRange range2 = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                range2.Save(ms, DataFormats.Rtf);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    rtfFromRtb = sr.ReadToEnd();
                }
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------//

            //Sends the Current Patient's ID.
            Send(CurrentPatient.ID);
            WaitForPing();

            //Sends the Current Visit's ID.
            Send(CurrentVisit.ID.ToString());
            WaitForPing();

            //Sends the length of the richtextbox text.
            Send(rtfFromRtb.Length.ToString());
            WaitForPing();

            //Sends the richtextbox text.
            Send(rtfFromRtb);
            WaitForPing();

        }

        public void RemoveVisit(string id)
        {

            //Connects to the server and sends command 31.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#31");
            WaitForPing();

            //Sends the ID of the patient that is to be removed.
            Send(id);
            WaitForPing();

        }

        public void RemoveImage(string imageid, string id, string visitID, string name)
        {

            //Connects to the server and sends command 32.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#32");
            WaitForPing();

            //Sends the ID of the image that is to be removed.
            Send(imageid);
            WaitForPing();

            //Sends the ID of the patient.
            Send(id);
            WaitForPing();

            //Sends the name of the patient.
            Send(name);
            WaitForPing();

            //Sends the ID of the visit.
            Send(visitID);
            WaitForPing();

        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------//



        //-----------------------------------------------------------------------------Request Commands---------------------------------------------------------------//

        public void RequestVisitNotes()
        {

            //Connects to the server and sends command 22.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#22");
            WaitForPing();

            //Reads the number of files that are about to be sent.
            byte[] numberoffilespacket = read(65536);

            //Cancels if no files are being sent.
            if (Decode(numberoffilespacket).Remove(Decode(numberoffilespacket).IndexOf("\0")) == "NULL")
                return;

            int numberoffiles = int.Parse(Decode(numberoffilespacket));

            //Reads each of the files and saves them to a temporary folder.
            for (int i = 0; i < numberoffiles; i++)
            {

                byte[] filesizepacket = read(65536);
                string filesizedecoded = Decode(filesizepacket);
                int filesize = int.Parse(filesizedecoded.Remove(filesizedecoded.IndexOf("~INCOMING_FILE_SIZE")));

                byte[] recievedpacket = read(filesize);

                string filestring = Decode(recievedpacket);
                string owner = filestring.Substring(filestring.IndexOf("~~OWNER")).Substring(7);

                byte[] file = new byte[recievedpacket.Length - (owner.Length + 7)];
                for (int x = 0; x < file.Length; x++)
                {
                    file[x] = recievedpacket[x];
                }

                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\tempnotefiles") == false)
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\tempnotefiles");
                File.WriteAllBytes(Directory.GetCurrentDirectory() + @"\tempnotefiles\" + owner, file);

            }

        }

        public void RequestClientImages()
        {

            //Connects to the server and sends command 18.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#18");

            //Reads the number of images.
            byte[] numberofimagespacket = read(65536);
            int numberofimages = int.Parse(Decode(numberofimagespacket));

            //Reads the images and saves them to a temporary folder.
            for (int i = 0; i < numberofimages; i++)
            {

                byte[] clientidpacket = read(65536);
                string clientid = Encoding.ASCII.GetString(clientidpacket).Remove(Encoding.ASCII.GetString(clientidpacket).IndexOf("~"));

                byte[] imagesizepacket = read(65536);
                string imagesizedecoded = Decode(imagesizepacket);

                if(imagesizedecoded.Remove(imagesizedecoded.IndexOf("~")) != "NULL")
                {

                    int imagesize = int.Parse(imagesizedecoded.Remove(imagesizedecoded.IndexOf("~INCOMING_FILE_SIZE")));

                    byte[] recievedpacket = read(imagesize);

                    if (Directory.Exists(Directory.GetCurrentDirectory() + @"\tempclientimages") == false)
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\tempclientimages");
                    using (FileStream filestream = new FileStream(Directory.GetCurrentDirectory() + @"\tempclientimages\img_" + clientid + ".jpg", FileMode.Create))
                    {
                        filestream.Write(recievedpacket, 0, recievedpacket.Length);
                        filestream.Close();
                    }
                    File.WriteAllBytes(Directory.GetCurrentDirectory() + @"\tempclientimages\img_" + clientid + ".jpg", recievedpacket);

                }

            }

        }

        public void RequestVisitImages(string name, int ID)
        {

            //Connects to the server and sends command 24.
            stream = Custom_Classes.Tcp.client.GetStream();
            Send("3214~COMMAND#24");
            WaitForPing();

            //Sends the ID of the patient whos images are needed.
            Send(CurrentPatient.ID);
            WaitForPing();

            //Reads the number of visits the patient has.
            byte[] bNumberOfVisits = read(65536);
            string sNumberOfVisits = Decode(bNumberOfVisits);
            string NumberOfVisits = sNumberOfVisits.Remove(sNumberOfVisits.IndexOf("\0"));

            //Goes through each visit, reads their information and prepares temporary folders.
            for (int l = 0; l < int.Parse(NumberOfVisits); l++)
            {

                byte[] VisitIDpacket = read(65536);
                string VisitID = Encoding.ASCII.GetString(VisitIDpacket).Remove(Encoding.ASCII.GetString(VisitIDpacket).IndexOf("\0"));

                byte[] firstrecievedpacket = read(65536);
                int numberofimages = int.Parse(Decode(firstrecievedpacket));


                if (Directory.Exists(Directory.GetCurrentDirectory() + @"\tempvisitimages\" + name + @"\Visit" + VisitID) == false)
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\tempvisitimages\" + name + @"\Visit" + VisitID);

                //Goes through the images for that visit and saves them to the previously created temporary folder.
                for (int i = 0; i < numberofimages; i++)
                {

                    byte[] imagesizepacket = read(65536);
                    string imagesizedecoded = Decode(imagesizepacket);
                    int imagesize = int.Parse(imagesizedecoded.Remove(imagesizedecoded.IndexOf("~INCOMING_IMAGE_SIZE")));

                    byte[] ownerpacket = read(10);
                    string ownerpacketdecoded = Decode(ownerpacket);
                    string owner = ownerpacketdecoded.Substring(2).Remove(ownerpacketdecoded.Substring(2).IndexOf("V"));

                    byte[] recievedpacket = read(imagesize);

                    if (owner == "U" + ID.ToString())
                    {

                        GC.Collect();
                        GC.WaitForPendingFinalizers();

                        using(FileStream filestream = new FileStream(Directory.GetCurrentDirectory() + @"\tempvisitimages\" + name + @"\Visit" + VisitID + @"\img_" + i.ToString() + ".jpg", FileMode.Create))
                        {

                            filestream.Write(recievedpacket, 0, recievedpacket.Length);
                            filestream.Close();

                        }

                    }

                }

            }

        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------//



        //----------------------------------------------------------------------------Fundamentals-------------------------------------------------------------------//

        public byte[] read(int size)
        {

            //Declares some variables that will be used later in the function.
            byte[] recievedpacket = new byte[size];
            bool dataleft = true;
            int bytesread = 0;
            bool error = false;

            //Loops to try to read the stream until there is no more data left.
            while (dataleft)
            {

                try
                {
                    //Reads one byte of the stream.
                    stream.Read(recievedpacket, bytesread, 1);
                    bytesread++;

                    //checks if there is more dataleft.
                    if (stream.DataAvailable == false)
                    {

                        //If there is not, it waits for 10 milliseconds and tries again to avoid slow internet connections missing packets.
                        Thread.Sleep(10);
                        if (stream.DataAvailable == false)
                            dataleft = false;

                    }

                }

                catch
                {

                    error = true;

                }

            }

            //Informs the user if something went wrong.
            if (error)
                MessageBox.Show("Some data was not recieved.", "Data Missing", MessageBoxButton.OK, MessageBoxImage.Error);

            Ping();
            return recievedpacket;

        }

        public void Send(string message)
        {

            //Encodes the message into ascii and puts into a byte packet.
            byte[] packet = new byte[message.Length];
            packet = Encoding.ASCII.GetBytes(message);

            //Writes into the server stream unless unable to, in which case an error is thrown indicating the connection has been lost and the application closes.
            try
            {

                stream.Write(packet, 0, packet.Length);

            }

            catch
            {

                MessageBox.Show("Connection to server was lost. Click OK to close the application.", "Connection lost", MessageBoxButton.OK, MessageBoxImage.Error);
                Disconnected();
                return;

            }

            //Sends the written data.
            stream.Flush();

        }

        public string Decode(byte[] message)
        {

            //Decodes a byte array into a string using ascii and returns the string.
            string decodedmessage = Encoding.ASCII.GetString(message);
            return decodedmessage;

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------//



        //----------------------------------------------------------------------------Ping System--------------------------------------------------------------------//

        public void Ping()
        {

            //Sends a generic ping message to indicate to the server that the client has recieved and processed information, and is ready to recieve more information.
            byte[] PingPacket = new byte[9];
            PingPacket = Encoding.ASCII.GetBytes("3214~PING");
            stream.Write(PingPacket, 0, 9);
            stream.Flush();

        }

        public void WaitForPing()
        {

            //Waits until a generic ping message is recieved, indicating the server has recieved and processed information and is ready to recieve more information.
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

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------//



        //---------------------------------------------------------------------------Connection----------------------------------------------------------------------//

        public void Disconnected()
        {

            //Shuts down the application.
            Application.Current.Dispatcher.Invoke(() =>
            {

                Application.Current.Shutdown();

            });

        }

        public void CloseConnection()
        {

            try
            {
                //Attempts to disconnect from the server.
                stream = Custom_Classes.Tcp.client.GetStream();
                Send("3214~COMMAND#33");
                Custom_Classes.Tcp.Disconnect();

            }

            catch
            {

            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------//

    }

}
