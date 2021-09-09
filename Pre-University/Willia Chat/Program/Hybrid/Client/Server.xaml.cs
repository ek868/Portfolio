
using System.Text;
using System.Windows;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace Client
{
    /// <summary>
    /// Interaction logic for Server.xaml
    /// </summary>
    public partial class Server : Window
    {
        public string Seperator = "|";
        public Server()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TcpListener MainServer = new TcpListener(3214);
            TcpClient Server = new TcpClient();
            NetworkStream stream;
            byte[] Packet = new byte[65536];
            string _RAWclientUserName = "";

            MainServer.Start();
            OutPut("Server has started...");
            Thread WaitThread = new Thread(Wait);
        }

        public void Wait()
        {
            TcpListener MainServer = new TcpListener(3214);
            TcpClient Server = new TcpClient();
            NetworkStream stream;
            byte[] Packet = new byte[65536];
            string _RAWclientUserName = "";

            MainServer.Start();
            OutPut("Server has started...");
            while (true)
            {
                OutPut("Server is waitting for a connection...");
                Server = MainServer.AcceptTcpClient();
                stream = Server.GetStream();
                stream.Read(Packet, 0, Packet.Length);
                _RAWclientUserName = Encoding.Unicode.GetString(Packet);
                string UnameSpaces = _RAWclientUserName.Substring(_RAWclientUserName.IndexOf(Seperator));
                string _ClientUserName = _RAWclientUserName.Remove(_RAWclientUserName.IndexOf(Seperator), UnameSpaces.Length);

                ClientList.List.Add(_ClientUserName, Server);
                BroadCast(_ClientUserName, true, _ClientUserName);
                OutPut(_ClientUserName + " has connected.");
                Thread IdleThread = new Thread(() => Idle(Server, _ClientUserName));
                IdleThread.Start();
            }
        }

        public void BroadCast(string msg, bool first, string UserName)
        {
            foreach (DictionaryEntry i in ClientList.List)
            {
                if(first == true)
                {
                    msg = msg + " has joined.";
                }
                else
                {
                    msg = UserName + ": " + msg;
                }
                OutPut(msg);
                msg += Seperator;
                byte[] Packet = Encoding.Unicode.GetBytes(msg);
                NetworkStream stream;
                TcpClient _BroadCast = (TcpClient)i.Value;
                stream = _BroadCast.GetStream();
                stream.Write(Packet, 0, Packet.Length);
                stream.Flush();
            }
        }

        public void Idle (TcpClient Client,string UserName)
        {
            TcpClient UserClient = Client;
            NetworkStream stream;
            byte[] Packet = new byte[65536];
            while (true)
            {
                stream = UserClient.GetStream();
                stream.Read(Packet, 0, Packet.Length);
                string message = Encoding.Unicode.GetString(Packet);
                string msgExtras = message.Substring(message.IndexOf(Seperator));
                string _msg = message.Remove(message.IndexOf(Seperator), msgExtras.Length);
                BroadCast(_msg, false, UserName);
            }
        }
        
        public void OutPut(string msg)
        {
            Dispatcher.Invoke(() =>
            {
                txbDisplay.Text += msg + "\n";
            });
        }
    }
    public static class ClientList
    {
        public static Hashtable List = new Hashtable();
    }
}
