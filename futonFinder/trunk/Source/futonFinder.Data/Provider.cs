namespace futonFinder.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using futonFinder.Core.Extensions;
    using futonFinder.Data;

    public partial class Provider
    {
        public Provider(string providerSlug)
        {
            var oAuthProvider = null; // Data.Repositories.Current.Repository.GetOAuth(providerSlug);
            this.ProviderId = oAuthProvider.ProviderId;
            this.CodeEndpoint = oAuthProvider.CodeEndpoint;
            this.TokenEndpoint = oAuthProvider.TokenEndpoint;
            this.UserEndPoint = oAuthProvider.UserEndpoint;
            this.CodeLinkFormat = oAuthProvider.CodeLinkFormat;
            this.TokenLinkFormat = oAuthProvider.TokenLinkFormat;
            this.RedirectUri = oAuthProvider.RedirectUri.Trim();
            this.ClientId = oAuthProvider.ClientId;
            this.ClientSecret = oAuthProvider.ClientSecret;
            this.Scope = oAuthProvider.Scope;
            this.ReturnUrl = null;
            this.Code = null;
            this.Token = null;
        }

        /// <summary>
        /// Get the link to oAuths's authorization page for this application.
        /// </summary>
        /// <returns>The url with a valid request token, or a null string.</returns>
        public string AuthorizationLinkGet()
        {
            var authorizationLink = string.Format(CodeLinkFormat, CodeEndpoint, ClientId, RedirectUri, Scope);
            return authorizationLink;
        }

        /// <summary>
        /// Exchange the "code" for an access token.
        /// </summary>
        /// <param name="authToken">The oauth_token or "code" is supplied by Facebook's authorization page following the callback.</param>
        public void AccessTokenGet(string authToken)
        {
            //this.Token = authToken;
            string accessTokenUrl = string.Format(TokenLinkFormat, TokenEndpoint, ClientId, RedirectUri, ClientSecret, authToken);

            string response = WebRequest(Enumerations.Method.Get, accessTokenUrl, String.Empty);

            if (response.Length > 0)
            {
                if (this.ResponseType == "application/json;charset=UTF-8")
                {
                    var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(AccessToken));
                    var stream = new MemoryStream(Encoding.UTF8.GetBytes(response));
                    var readObject = dataContractJsonSerializer.ReadObject(stream);
                    stream.Close();
                    AccessToken accessToken = (AccessToken)readObject;
                    if (accessToken != null)
                    {
                        this.Token = accessToken.access_token;
                    }


                }
                else
                {
                    //Store the returned access_token
                    NameValueCollection qs = HttpUtility.ParseQueryString(response);

                    if (qs["access_token"] != null)
                    {
                        this.Token = qs["access_token"];
                    }
                }
            }
        }

        /// <summary>
        /// Web Request Wrapper
        /// </summary>
        /// <param name="method">Http Method</param>
        /// <param name="url">Full url to the web resource</param>
        /// <param name="postData">Data to post in querystring format</param>
        /// <returns>The web server response.</returns>
        public string WebRequest(.Method method, string url, string postData)
        {
            HttpWebRequest webRequest = null;
            StreamWriter requestWriter = null;
            string responseData = "";

            webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = method.ToString();
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.UserAgent = "[Your user agent]";
            webRequest.Timeout = 20000;

            if (method == Enumerations.Method.Post)
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //POST the data.
                requestWriter = new StreamWriter(webRequest.GetRequestStream());
                try
                {
                    requestWriter.Write(postData);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    requestWriter.Close();
                    requestWriter = null;
                }
            }

            responseData = WebResponseGet(webRequest);
            webRequest = null;
            return responseData;
        }

        /// <summary>
        /// Process the web response.
        /// </summary>
        /// <param name="webRequest">The request object.</param>
        /// <returns>The response data.</returns>
        public string WebResponseGet(HttpWebRequest webRequest)
        {
            StreamReader responseReader = null;
            string responseData = "";

            try
            {
                var response = webRequest.GetResponse();
                this.ResponseType = response.ContentType;
                responseReader = new StreamReader(response.GetResponseStream());
                responseData = responseReader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                webRequest.GetResponse().GetResponseStream().Close();
                responseReader.Close();
                responseReader = null;
            }

            return responseData;
        }

        //public OperationResult<User> ParseInformation(int providerId, string json, string app, string clientIp)
        //{
        //    DataContractJsonSerializer dataContractJsonSerializer = null;
        //    var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        //    object readObject = null;
        //    OperationResult<User> thirdPartyUserOr = null;

        //    switch (providerId)
        //    {
        //        //Facebook
        //        case (int)Enumerations.Provider.Facebook:
        //            dataContractJsonSerializer = new DataContractJsonSerializer(typeof(FacebookUser));
        //            readObject = dataContractJsonSerializer.ReadObject(stream);
        //            stream.Close();

        //            var facebookUser = (FacebookUser)readObject;
        //            thirdPartyUserOr = Provider.ThirdPartyProviderLogin(Enumerations.Provider.Facebook, facebookUser.id, facebookUser.email, facebookUser.first_name, facebookUser.last_name, app, clientIp);

        //            //User was logged in, update information))
        //            if (thirdPartyUserOr.WasSuccessful)
        //            {
        //                char? gender = null;
        //                if (facebookUser.gender != null)
        //                {
        //                    gender = facebookUser.gender == "male" ? 'M' : 'F';
        //                }

        //                //List of properties to modify
        //                var userProperties = new List<Enumerations.UserProperties>
        //                    {
        //                        Enumerations.UserProperties.Email,
        //                        Enumerations.UserProperties.Gender,
        //                        Enumerations.UserProperties.FirstName,
        //                        Enumerations.UserProperties.LastName
        //                    };

        //                var userSave = User.Save(userId: thirdPartyUserOr.Item.UserId,
        //                                         emailAddress: facebookUser.email,
        //                                         genderCode: gender,
        //                                         firstName: facebookUser.first_name,
        //                                         lastName: facebookUser.last_name,
        //                                         legacy: true,
        //                                         userProperties: userProperties);
        //            }

        //            return thirdPartyUserOr;
        //            break;

        //        //MSN Live
        //        case (int)Enumerations.Provider.Live:
        //            dataContractJsonSerializer = new DataContractJsonSerializer(typeof(LiveUser));
        //            readObject = dataContractJsonSerializer.ReadObject(stream);
        //            stream.Close();

        //            var liveUser = (LiveUser)readObject;
        //            thirdPartyUserOr = Provider.ThirdPartyProviderLogin(Enumerations.Provider.Live, liveUser.id, liveUser.emails.account, liveUser.first_name, liveUser.last_name, app, clientIp);

        //            //User was logged in, update information))
        //            if (thirdPartyUserOr.WasSuccessful)
        //            {
        //                char? gender = null;
        //                if (liveUser.gender != null)
        //                {
        //                    gender = liveUser.gender == "male" ? 'M' : 'F';
        //                }

        //                var userProperties = new List<Enumerations.UserProperties>
        //                    {
        //                        Enumerations.UserProperties.Email,
        //                        Enumerations.UserProperties.Gender,
        //                        Enumerations.UserProperties.FirstName,
        //                        Enumerations.UserProperties.LastName
        //                    };
        //                var userSave = User.Save(userId: thirdPartyUserOr.Item.UserId,
        //                                         emailAddress: liveUser.emails.account,
        //                                         genderCode: gender,
        //                                         firstName: liveUser.first_name,
        //                                         lastName: liveUser.last_name,
        //                                         legacy: true,
        //                                         userProperties: userProperties);
        //            }

        //            return thirdPartyUserOr;
        //            break;
        //    }
        //    return new OperationResult<User>();
        //}
    }
}
