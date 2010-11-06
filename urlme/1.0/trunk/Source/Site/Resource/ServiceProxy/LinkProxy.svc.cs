using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using UrlMe.cc.Model;
using UrlMe.cc.Model.Enums;

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
            CrudLinkResults result = Link.DeleteLink(linkId);
            if (result == CrudLinkResults.Success)
                return string.Format("{0}:{1}", CrudLinkResults.Success, linkId);
            return CrudLinkResults.Failure.ToString();
        }

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public string DeleteLinks(string linkIds)
        {
            CrudLinkResults result = Link.DeleteLinks(linkIds);
            if (result == CrudLinkResults.Success)
                return string.Format("{0}:{1}", CrudLinkResults.Success, linkIds);
            return CrudLinkResults.Failure.ToString();
        }
    }
}
