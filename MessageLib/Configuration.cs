using System;

namespace MessageLib
{
    public static class Configuration
    {
        /// <summary>
        /// Cannot be set to true with UseMulticasting
        /// </summary>
        public const bool IsReceiveQueueTransactional = false;
        public const string ReceiveQueue = @".\private$\ReceiveQueue";

        /// <summary>
        /// Cannot be set to true with IsReceiveQueueTransactional
        /// </summary>
        public const bool UseMulticasting = true;
        public const string MulticastAddress = "234.1.1.1:8001";

        public const bool ClearSystemQueuesOnSubscriber = false;
        public const bool ClearSystemQueuesOnPublisher = true;

        public static void PrintConfiguration()
        {
            Console.WriteLine("IsReceiveQueueTransactional = " + IsReceiveQueueTransactional);
            Console.WriteLine("ReceiveQueue = " + ReceiveQueue);
            Console.WriteLine("UseMulticasting = " + UseMulticasting);
            Console.WriteLine("MulticastAddress = " + MulticastAddress);
            Console.WriteLine("ClearSystemQueuesOnSubscriber = " + ClearSystemQueuesOnSubscriber);
            Console.WriteLine("ClearSystemQueuesOnPublisher = " + ClearSystemQueuesOnPublisher);
        }
    }
}
