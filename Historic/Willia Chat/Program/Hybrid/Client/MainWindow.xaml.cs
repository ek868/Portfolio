using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net.Sockets;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace Client
{
    public partial class MainWindow : Window
    {
        //Creates the items that will be used later
        TcpClient UserClient = new TcpClient();
        NetworkStream stream;
        string seperator = "|";

        public MainWindow()
        {
            InitializeComponent();
        }

        bool ExtraCharManager = true;

        private void BtnExtra_Click(object sender, RoutedEventArgs e)
        {
            if (ExtraCharManager == true)
            {

                Extras.ExtraGlobal = TxtInput.Text.ToString();
                Window1 ExtraWindow = new Window1();
                ExtraWindow.ShowDialog();
                TxtInput.Text += ExtraWindow.trial;
                ExtraCharManager = true;

            }
            else return;

        }


        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnSend.IsEnabled = true;
            Extras.ExtraGlobal = TxtInput.Text.ToString();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {

            //Connects the stream to the server stream, Sends message down the stream.
            stream = UserClient.GetStream();
            string msg = TxtInput.Text;
            SendMessage(msg);
            BtnSend.IsEnabled = true;
            TxtInput.Clear();
            BtnSend.IsEnabled = false;

        }

        private void Window_Loaded(object sender, EventArgs e)
        {

            //Connects the client using the global variables we filled in earlier.
            UserClient.Connect(Information._IPAdress, 3214);
            //Connects the stream to the server stream, encodes the user name, writes it into the stream and sends it down.
            stream = UserClient.GetStream();
            byte[] Packet = Encoding.Unicode.GetBytes(Information._UserName + seperator);
            stream.Write(Packet, 0, Packet.Length);
            stream.Flush();

            //Begins a new thread that waits for a message.
            Thread WaitThread = new Thread(WaitForMessage);
            WaitThread.Start();
            BtnSend.IsEnabled = false;
            Extras trial = new Extras();

        }

        public void WaitForMessage()
        {

            bool IsConnected = true;

            //continues the following loop until the connection is lost.
            while (IsConnected == true)
            {

                //Connects the stream to the server stream and waits there until a message is recieved.
                NetworkStream RecieveStream = UserClient.GetStream();
                byte[] Packet = new byte[65536];
                //when a message is sent down it places it into the Packet and decodes it.

                //If the processes under the "try" are not complete, it moves to catch.
                try
                {

                    //Decodes the recieved message and removes the spaces.
                    RecieveStream.Read(Packet, 0, Packet.Length);
                    string IncomingMsg = Encoding.Unicode.GetString(Packet);
                    string msgExtras = IncomingMsg.Substring(IncomingMsg.IndexOf(seperator));
                    string _msg = IncomingMsg.Remove(IncomingMsg.IndexOf(seperator), msgExtras.Length);

                    //The program directly uses "has joined the channel" messages to add clients to the list.

                    // Checks if the message is a "disconnected" message. Which would let you know if another user has disconnected.                   
                    if (_msg.Contains(" has disconnected."))
                    {

                         //Retrieves the username and removes it from the list.
                         string TempUnameRemove = _msg.Substring(_msg.IndexOf(" "));
                         string UnameRemove = _msg.Remove(_msg.IndexOf(" "), TempUnameRemove.Length);
                         RemoveFromList(UnameRemove);

                    }

                    //checks if the message recieved is a "has joined channel" message.
                    if (_msg.Contains(" has joined the channel."))
                    {

                        //Retreives the Username from the message.
                        string TempUnameForList = _msg.Substring(_msg.IndexOf(" "));
                        string UnameForList = _msg.Remove(_msg.IndexOf(" "), TempUnameForList.Length);

                        //Checks if the username is already added.
                        if (LsbConnected.Items.Contains(UnameForList))
                        {

                        }

                        else
                        {

                            //Checks if the message was replicated by the user.
                            if (UnameForList.Contains(":"))
                            {

                            }

                            else
                             //Adds the name to the list.
                            {
                                AddToList(UnameForList);
                            }
                            
                            AddToDisplay(_msg + "\n");
                            
                        }
                    }

                 else
                 {
                        
                    AddToDisplay(_msg + "\n");

                 }
                }
                
                catch
                {
                    //If the process before fails to complete, this checks if it's because the connection is lost.
                    if (UserClient.Connected == true)
                    {
                    }
                    else if (UserClient.Connected == false)
                    {

                        // if the connection is lost it ends the loop and lets the user know.
                        IsConnected = false;
                        AddToDisplay("Disconnected from server.");

                    }
                }


            }
        }
        delegate void PeremeterizedMethodInvoke5(string msg);

        public void AddToDisplay(string msg)
        {

            //checks if the current thread can use the ui element that is not under it's control.
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new PeremeterizedMethodInvoke5(AddToDisplay), msg);
                return;
            }

            //add to the display with the current time written in smaller font next to it.
            Run time = new Run();
            time.FontSize = 8;
            time.Text += "<" + DateTime.Now + "> ";
            P1.Inlines.Add(time);

            Run message = new Run();
            message.FontSize = 14;
            message.Text = msg;
            P1.Inlines.Add(message);
        }

        public void AddToList(string username)
        {

            //Checks if the thread has permissions to edit the client list.
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new PeremeterizedMethodInvoke5(AddToList), username);
                return;
            }

            //checks if the list already has the username.
            if (LsbConnected.Items.Contains(username))
            {
            }

            else
            {
                //if not, it adds it.
                LsbConnected.Items.Add(username);
            }

        }

        public void RemoveFromList(string username)
        {

            //Checks if the thread is allowed to edit the client list.
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new PeremeterizedMethodInvoke5(RemoveFromList), username);
                return;
            }
            //if so it removes the username.
            LsbConnected.Items.Remove(username);

        }

        public void SendMessage(string msg)
        {

            //Encodes the message and writes it into the stream, then flushes.
            byte[] Packet = Encoding.Unicode.GetBytes(msg + seperator);
            stream.Write(Packet, 0, Packet.Length);
            stream.Flush();

        }

        private void BlackB_Checked(Object sender, EventArgs e)
        {

            this.Background = Brushes.Black;

        }
        private void BlackB_Unchecked(Object sender, EventArgs e)
        {

            this.Background = Brushes.Gainsboro;

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {

            //When disconnect is clicked, it informs the server, and the server lets everyone else know when it recieves that wierd message.
            SendMessage("//DISCONNECT//3214!!@@##$$%%^^&&**");
            //Closes the window.
            this.Close();
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {

                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtxtDisplay.Document.ContentStart, rtxtDisplay.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

            //Does the same thing as clicking disconnect
            SendMessage("//DISCONNECT//3214!!@@##$$%%^^&&**");
            this.Close();

        }
    }
}
