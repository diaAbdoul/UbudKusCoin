using System;
using System.Collections.Generic;
using System.Net;
using UbudKusCoin.Helpers;
using UbudKusCoin.Models;

namespace UbudKusCoin.Services.P2P
{
    public class P2PService
    {
        private P2PServer _server { set; get; }
        private IPAddress _address { set; get; }
        private int _port { set; get; }


        public readonly IDictionary<string, P2PClient> KnownPeers = new Dictionary<string, P2PClient>();

        public P2PService(IPAddress address, int port)
        {
            _address = address;
            _port = port;
            _server = new P2PServer(address, port);
        }


        public void Start()
        {
            // Start the server
            Console.WriteLine("P2P starting ...");
            _server.Start();
            Console.WriteLine("P2P running!");

            ListenEvent();

            if (_port != 3000)
            {
                Connect("127.0.0.1", 3000);
            }

        }

        private void ListenEvent()
        {
            //Console.WriteLine("... this is listerner ...");
            ServiceManager.ChainService.EventBlockCreated += Evt_EventBlockCreated;
            // Evt.TransactionCreated += Evt_TransactionCreated;
        }

        void Evt_EventBlockCreated(object sender, Block block)
        {
            //if (sender == null)
            //{
            //    Console.WriteLine("No New Block Added.");
            //    return;
            //}
            Console.WriteLine("putu kusuma create block.");
            Utils.PrintBlock(block);
            // BroadCast("A New Block from: " + _port);
        }

        void Evt_TransactionCreated(object sender, EventArgs e)
        {
            //if (sender == null)
            //{
            //    Console.WriteLine("Transaction created");
            //    return;
            //}
            Console.WriteLine("A New Transaction Created.");
        }

        public void Stop()
        {
            // Stop the server
            Console.WriteLine("P2P stopping...");
            _server.Stop();
            Console.WriteLine("P2P stopped...");
        }

        public void Connect(string destAddress, int destPort)
        {

            // start client, start client
            //if (destPort != 3000)
            //{

                if (!KnownPeers.ContainsKey(destAddress+":"+destPort))
                {

                    // Create a new TCP chat client
                    var client = new P2PClient(destAddress, destPort);

                    // Connect the client
                    Console.Write("Client connecting...");
                    client.ConnectAsync();
                    // Send the entered text to the chat server
                    client.SendAsync("hello my  " + destPort);
                    Console.WriteLine("Done!");

                    KnownPeers.Add(destAddress + ":" + destPort, client);

                }

                //end of
            //}
        }


        public  void SendData(string url, string data)
        {
            foreach (var peer in KnownPeers)
            {
                if (peer.Key == url)
                {
                    peer.Value.SendAsync(data);
                }
            }
        }

        //handleer OnBlockCreated
        //do broadcast


        public void BroadCast(string data)
        {
            //Console.WriteLine(".. broad cast .." + data);
            foreach (var peer in KnownPeers)
            {
                peer.Value.SendAsync(data);
            }
        }


        public IList<string> GetPeers()
        {
            IList<string> allpeers = new List<string>();
            foreach (var peer in KnownPeers)
            {
                allpeers.Add(peer.Key);
            }
            return allpeers;
        }


    }
}
