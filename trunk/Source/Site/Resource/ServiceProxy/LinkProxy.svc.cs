using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace UrlMe.cc.Resource.ServiceProxy
{
    [ServiceContract(Namespace = "UrlMe.cc.Resource.ServiceProxy.LinkProxy")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LinkProxy
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string HelloWorld(string s)
        {
            return "Hello world";
        }
    }
}
