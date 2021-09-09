using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NargesLogs.Custom_Classes
{

    public static class Tcp
    {

        //Declares publicly used TCP client.
        static public TcpClient client = new TcpClient();

        //Connects to the server based on saved connection settings.
        static public void Connect()
        {

            client = new TcpClient();
            client.Connect(ConnectionInfo.IPAddress, ConnectionInfo.Port);

        }

        //Disconnects from the server.
        static public void Disconnect()
        {

            client.Close();

        }

    }

}
