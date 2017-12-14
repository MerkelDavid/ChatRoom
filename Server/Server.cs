using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static Client client;
        Dictionary<int,Client> Clients;
        TcpListener server;
        public Server()
        {
            Clients = new Dictionary<int, Client>();
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void  Run()
        {
            AcceptClients();
        }

        public async void ManageMessages()
        {
            while (true)
            {
                Task<string> message = client.Recieve();
                await message;
                Respond(message.Result);
            }
        }

        private void AcceptClients()
        {
            while (true)
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = clientSocket.GetStream();
                client = new Client(stream, clientSocket);
                Clients.Add(Clients.Count, client);
                Thread newThread = new Thread(ManageMessages);
                newThread.Start();
            }

        }
        private void Respond(string body)
        {
            Object thislock = new Object();
            lock(thislock){
                foreach (KeyValuePair<int, Client> item in Clients)
                {
                    //if (client != item.Value)
                        item.Value.Send(body);
                }
            }
        }


    }
}
