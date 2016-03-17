using System;
using System.Globalization;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;

using MessageLib;
using Utils = MessageLib.Utils;

namespace MessagePublisher
{
    class Program
    {
        public const string ACKQueue = @".\private$\ACKQueue";

        public const string ResponseQueue = @".\private$\ResponseQueue";
        public const bool IsResponseQueueTransactional = false;

        public const string MulticastQueue = "FormatName:MULTICAST=234.1.1.1:8001";

        private static void Main(string[] args)
        {
            Console.WriteLine("I'm a publisher");
            Configuration.PrintConfiguration();

            Init();
            try
            {
                var cts = new CancellationTokenSource();

                Console.CancelKeyPress += (s, e) =>
                    {
                        e.Cancel = true;
                        cts.Cancel();
                    };

                Console.WriteLine("Press Ctrl+C to stop.");

                try
                {
                    Start(cts.Token).Wait();
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("Press any key to continue.");
            }
            finally
            {
                Clean();
            }

            Console.ReadLine();
        }

        private static void Clean()
        {
            MessageQueue.Delete(ACKQueue);
            MessageQueue.Delete(ResponseQueue);

            if (Configuration.ClearSystemQueuesOnPublisher)
                Utils.CleanSystemQueues();
        }

        private static void Init()
        {
            if (!MessageQueue.Exists(ACKQueue))
                MessageQueue.Create(ACKQueue);

            if (!MessageQueue.Exists(ResponseQueue))
                MessageQueue.Create(ResponseQueue, IsResponseQueueTransactional);

            if (!Configuration.UseMulticasting)
            {
                var i = 0;
                while (i != 5)
                {
                    if (!MessageQueue.Exists(Configuration.ReceiveQueue))
                    {
                        Thread.Sleep(1000*i);
                        ++i;
                    }
                    else
                        break;
                }
            }
        }

        private async static Task Start(CancellationToken token)
        {
            await Task.Factory.StartNew(() =>
                {
                    var queue = Configuration.UseMulticasting ? MulticastQueue : Configuration.ReceiveQueue;

                    using (var q = new MessageQueue(queue))
                    {
                        //It works if a message is sent to a remote queue. MULTICAST queue is a remote one.
                        q.DefaultPropertiesToSend.UseJournalQueue = true;
                        
                        while (true)
                        {
                            if (token.IsCancellationRequested)
                                return;

                            var msg = new Message();
                            
                            if(Configuration.UseMulticasting)
                                msg.Body = "MULTICASTING" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
                            else
                                msg.Body = DateTime.Now.ToString(CultureInfo.InvariantCulture);

                            //Message will be sent to ACKQueue if a message was delivered to the ReceiveQueue
                            //Message will be sent to ACKQueue also if it wasn't possible to deliver message
                            msg.AdministrationQueue = new MessageQueue(ACKQueue);
                            msg.AcknowledgeType = AcknowledgeTypes.FullReceive;
                            //This will cause that a message being sent will be placed in DeadLetterQueue for example if:
                            // - a target queue does not exist
                            // - a target queue is transacion but the send operation is not
                            msg.UseDeadLetterQueue = true;
                            //It works if a message is sent to a remote queue. MULTICAST queue is a remote one.
                            msg.UseJournalQueue = true;
                            //It assures that a message will be written to a disk.
                            msg.Recoverable = true;
                            //This is queueu where we want to receive a response
                            msg.ResponseQueue = new MessageQueue(ResponseQueue);

                            if (Configuration.IsReceiveQueueTransactional)
                                q.Send(msg, MessageQueueTransactionType.Single);
                            else
                                q.Send(msg);

                            Thread.Sleep(1000);
                        }
                    }
                }, token);
        }
    }
}
