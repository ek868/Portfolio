using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server_code
{
    class Program
    {

        static void Main(string[] args)
        {
            TcpListener MainServer = new TcpListener(3214);
            TcpClient Server = new TcpClient();
            NetworkStream stream;
            byte[] Packet = new byte[65536];
            string RAWclientUserName = "";

            MainServer.Start();
            Console.WriteLine("Server has started...");
            while (true)
            {
                Console.WriteLine("Server is waiting for a connection...");
                Server = MainServer.AcceptTcpClient();
                stream = Server.GetStream();
                stream.Read(Packet, 0, Packet.Length);
                RAWclientUserName = Encoding.Unicode.GetString(Packet);
                string UnameSpaces = RAWclientUserName.Substring(RAWclientUserName.IndexOf("|"));
                string ClientUserName = RAWclientUserName.Remove(RAWclientUserName.IndexOf("|"), UnameSpaces.Length);

                ClientList.List.Add(ClientUserName, Server);
                BroadCast(ClientUserName, true, ClientUserName);
                Console.WriteLine(ClientUserName + " has connected.");
                Thread nt = new Thread (() => Idle(Server, ClientUserName));
                nt.Start();
            }



        }
        
        public static void BroadCast(string msg, bool first, string username)
        {
            foreach(DictionaryEntry i in ClientList.List)
            {
                List<string> Profanity = new List<string>();
                Profanity.Add("fuck");
                Profanity.Add("Fuck");
                Profanity.Add("FUCK");
                Profanity.Add("shit");
                Profanity.Add("Shit");
                Profanity.Add("SHIT");
                Profanity.Add("cunt");
                Profanity.Add("Cunt");
                Profanity.Add("CUNT");
                Profanity.Add("bitch");
                Profanity.Add("Bitch");
                Profanity.Add("BITCH");
                Profanity.Add("faggot");
                Profanity.Add("Faggot");
                Profanity.Add("FAGGOT");
                Profanity.Add("nigger");
                Profanity.Add("Nigger");
                Profanity.Add("NIGGER");
                Profanity.Add("whore");
                Profanity.Add("Whore");
                Profanity.Add("WHORE");
                Profanity.Add("ass");
                Profanity.Add("Ass");
                Profanity.Add("ASS");

                if (first == true)
                {
                    msg = msg + " has joined the channel."; 
                }
                else
                {
                    msg = username + ": " + msg;
                }
                foreach (string word in Profanity)
                {
                    if (msg.Contains(word))
                    {
                        msg = "THIS IS A CHRISTIAN SERVER"; 
                    }
                }
                Console.WriteLine(msg);
                msg += "|";
                byte[] Packet = Encoding.Unicode.GetBytes(msg);
                NetworkStream stream;
                TcpClient broadcast = (TcpClient)i.Value;
                stream = broadcast.GetStream();
                stream.Write(Packet, 0, Packet.Length);
                stream.Flush();
            }
        }

        private static void Idle(TcpClient Client, string username)

        {
            TcpClient UserClient = Client;
            NetworkStream stream;
            byte[] Packet = new byte[65536];
            while (true)
            {
                stream = UserClient.GetStream();
                stream.Read(Packet, 0, Packet.Length);
                string message = Encoding.Unicode.GetString(Packet);
                string msgExtras = message.Substring(message.IndexOf("|"));
                string Message = message.Remove(message.IndexOf("|"), msgExtras.Length);
                BroadCast(Message, false, username);
            }

        }
    }
    public static class ClientList
    {
        public static Hashtable List = new Hashtable();
    }
}
