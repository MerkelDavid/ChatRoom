using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Log
    {
        private Queue<Message> messageLog;

        public Log()
        {

        }

        public void AddToLog(Client messageClient, string currentMessage)
        {

            messageLog.Enqueue(new Message(messageClient,currentMessage));
        }

        public void printLog()
        {
            Queue<Message> tempLog = messageLog;
            for(int i = 0; i < messageLog.Count; i++)
            {
                Message currentMessage =  tempLog.Dequeue();
                Console.WriteLine(currentMessage.Body);
            }
        }
    }
}
