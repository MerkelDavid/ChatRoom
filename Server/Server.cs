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
        Queue<string> Messages;


        public Server()
        {
            Messages = new Queue<string>();
            Clients = new Dictionary<int, Client>();
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);
            server.Start();
        }
        public void  Run()
        {
            AcceptClients();
        }

        public async void ManageMessages(Client currentClient)
        {
            while (true)
            {
                string actualMessage = "";
                try
                {
                        Task<string> message = currentClient.Recieve();
                        await message;
                        actualMessage = currentClient.UserName + ": " + message.Result;
                        Respond(actualMessage,currentClient);
                }
                catch(Exception e)
                {
                    //user 'logs off'
                    actualMessage = currentClient.UserName + " has logged off";
                    Respond(actualMessage,currentClient);
                }

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
                Client Tempclient = new Client(stream, clientSocket);
                Clients.Add(Clients.Count, Tempclient);
                Respond(Tempclient.UserName + "has connected",Tempclient);
                Thread newThread = new Thread(()=>ManageMessages(Tempclient));
                newThread.Start();
            }

        }
        private void Respond(string body,Client currentClient)
        {
            Object thislock = new Object();
            Messages.Enqueue(body);
            lock(thislock){
                foreach (KeyValuePair<int, Client> item in Clients)
                {
                    if (currentClient != item.Value)
                    item.Value.Send(body);
                }
            }
        }


    }
}
