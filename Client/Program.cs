using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 9999);
            //Client client = new Client("192.168.0.102", 8001);
            /*            while (true)
                        {
                            client.Send();
                            bool isReading = true;
                            while (isReading)
                            {
                                client.Recieve();
                                Console.ReadKey();
                                isReading = false;
                            }
                        }
            */
            client.chat();
        }
    }
}
