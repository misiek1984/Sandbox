using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WebSockeLib
{
    [ServiceContract]
    public interface IWebSocketCallback
    {
        // Without IsOneWay set to true the following error will occure:
        // This operation would deadlock because the reply cannot be received until 
        // the current Message completes processing.If you want to allow out-of-order message 
        // processing, specify ConcurrencyMode of Reentrant or Multiple on ServiceBehaviorAttribute.
        [OperationContract(IsOneWay = true)]
        Task Send(int i);
    }

    [ServiceContract]
    public interface IWebSocketCallbackForJS
    {
        // Without IsOneWay set to true the following error will occure:
        // This operation would deadlock because the reply cannot be received until 
        // the current Message completes processing.If you want to allow out-of-order message 
        // processing, specify ConcurrencyMode of Reentrant or Multiple on ServiceBehaviorAttribute.
        [OperationContract(IsOneWay = true, Action ="*")]
        Task Send(Message m);
    }
}
