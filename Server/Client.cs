using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Client
    {
        public NetworkStream stream;
        public TcpClient client;
        public string UserName;
        public Client(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
            //UserName = "User"+generateID();
            UserName = "User " + CreateUserName(stream);//new code : change user name
        }

        //new method : get user name
        public string CreateUserName(NetworkStream stream)
        {
            string askForName = "Please enter a user name.";
            byte[] message = Encoding.ASCII.GetBytes(askForName);
            stream.Write(message, 0, message.Count());

            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);

            return recievedMessageString;
        }

        public string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public void Send(string Message)
        {

            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());

        }
        public async Task<string> Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
            Console.WriteLine(recievedMessageString);
            return recievedMessageString;
        }
    }
}
