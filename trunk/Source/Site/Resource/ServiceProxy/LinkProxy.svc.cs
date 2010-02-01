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

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string DeleteLink(int linkId)
        {
            int success = Library.Data.LinkData.DeleteLink(linkId);
            if (success == 0)
                return "Success:" + linkId.ToString();
            return "Failure";
        }

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string DeleteLinks(string linkIds)
        {
            int success = Library.Data.LinkData.DeleteLinks(linkIds);
            if (success == 0)
                return "Success:" + linkIds;
            return "Failure";
        }
    }
}
