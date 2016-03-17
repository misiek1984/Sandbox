using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MessageLib
{
    public static class Utils
    {
        public static void CleanSystemQueues()
        {
            using (var systemJournalQueue = new MessageQueue("FormatName:Direct=os:.\\System$;JOURNAL"))
            {
                systemJournalQueue.Purge();
            }

            using (var systemDeadLetterQueue = new MessageQueue("FormatName:Direct=os:.\\System$;DEADLETTER"))
            {
                systemDeadLetterQueue.Purge();
            }

            using (var systemDeadXLetterQueue = new MessageQueue("FormatName:Direct=os:.\\System$;DEADXACT"))
            {
                systemDeadXLetterQueue.Purge();
            }
        }
    }
}
