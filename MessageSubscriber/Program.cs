using System;
using System.Messaging;
using System.Threading;
using System.Threading.Tasks;

using MessageLib;

namespace MessageSubscriber
{
    class Program
    {
        private static string _multicastingQueueName;

        static void Main(string[] args)
        {
            Console.WriteLine("I'm a subscriber");
            Configuration.PrintConfiguration();

            Configure();
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
            if (Configuration.UseMulticasting)
                MessageQueue.Delete(_multicastingQueueName);
            else
                MessageQueue.Delete(Configuration.ReceiveQueue);

            if (Configuration.ClearSystemQueuesOnSubscriber)
                Utils.CleanSystemQueues();
        }

        private static void Configure()
        {
            if (Configuration.UseMulticasting)
            {
                _multicastingQueueName = Configuration.ReceiveQueue + DateTime.Now.Ticks.ToString();
                MessageQueue.Create(_multicastingQueueName, Configuration.IsReceiveQueueTransactional);
            }
            else
            {
                if (!MessageQueue.Exists(Configuration.ReceiveQueue))
                    MessageQueue.Create(Configuration.ReceiveQueue, Configuration.IsReceiveQueueTransactional);
            }
        }

        private async static Task Start(CancellationToken token)
        {
            await Task.Run(() =>
                {
                    var queueName = Configuration.UseMulticasting ? _multicastingQueueName : Configuration.ReceiveQueue;
                    using (var q = new MessageQueue(queueName))
                    {
                        if (Configuration.UseMulticasting)
                            q.MulticastAddress = Configuration.MulticastAddress;

                        //This will cause that evey received message will be placed in the journal queue
                        q.UseJournalQueue = true;

                        while (true)
                        {
                            if (token.IsCancellationRequested)
                                return;

                            try
                            {
                                var msg = q.Receive(new TimeSpan(0, 0, 0, 1));
                                msg.Formatter = new XmlMessageFormatter(new[] {typeof (string)});
                                var body = msg.Body as string;

                                if (msg.ResponseQueue != null)
                                {
                                    if(msg.ResponseQueue.Transactional)
                                        msg.ResponseQueue.Send("Response message", MessageQueueTransactionType.Single);
                                    else
                                        msg.ResponseQueue.Send("Response message");
                                }
                                   

                                Console.WriteLine(body);

                            }
                            catch (MessageQueueException ex)
                            {
                                if (ex.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
                                    throw;
                            }

                            Thread.Sleep(1250);
                        }
                    }
                }, token);
        }
    }
}
