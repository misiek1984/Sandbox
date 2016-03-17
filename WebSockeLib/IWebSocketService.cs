using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace WebSockeLib
{
    [ServiceContract(CallbackContract = typeof(IWebSocketCallback))]
    public interface IWebSocketService
    {
        // This operation would deadlock because the reply cannot be received until 
        // the current Message completes processing.If you want to allow out-of-order message 
        // processing, specify ConcurrencyMode of Reentrant or Multiple on ServiceBehaviorAttribute.
        [OperationContract(IsOneWay = true)]
        Task Hello(string name);
    }

    [ServiceContract(CallbackContract = typeof(IWebSocketCallbackForJS))]
    public interface IWebSocketServiceForJS
    {
        // This operation would deadlock because the reply cannot be received until 
        // the current Message completes processing.If you want to allow out-of-order message 
        // processing, specify ConcurrencyMode of Reentrant or Multiple on ServiceBehaviorAttribute.
        [OperationContract(IsOneWay = true, Action = "*")]
        Task Hello(Message m);
    }
}
