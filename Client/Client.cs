using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        public void Send()
        {
            string messageString = UI.GetInput();
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            stream.Write(message, 0, message.Count());
            Send();
        }
        public void Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            if (recievedMessage.Length > 0)
            {
                UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
                Recieve();
            }
            else
            {
                Recieve();
            }
        }

        public Task chat()
        {
                Parallel.Invoke(
                    () => Send(),
                    () => Recieve()
                );

            return null;
        }
    }
}
