using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace UbudKusCoin.Services.P2P
{

    public class ConSession : TcpSession
    {
        public ConSession(TcpServer server) : base(server)
        {
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Others node connected with ID {Id}");
            var msg = "hello friends, what do you want?";
            SendAsync(msg);
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Console.WriteLine("Incoming: " + message);

            // Multicast message to all connected sessions
            Server.Multicast(message + ".. dari multicst ..");

            // If the buffer starts with '!' the disconnect the current session
            if (message == "!")
                Disconnect();
        }
    }

    public class P2PServer : TcpServer
    {

      

        public P2PServer(IPAddress address, int port) : base(address, port)
        {
            Console.WriteLine("=== address: {0}", address);
            Console.WriteLine("=== port: {0}", port);
        }

        protected override TcpSession CreateSession()
        {
            return new ConSession(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"P2P server chaugh an error with code {error}");
        }
    }
}
