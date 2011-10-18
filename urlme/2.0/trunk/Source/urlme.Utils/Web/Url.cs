
namespace urlme.Utils.Web
{
    using System;
    using System.Collections.Specialized;
    using System.Text.RegularExpressions;

    /// <summary>
    /// this class is to allow us to manipulate a url in a control and insert one change to a querystring param and maintain everything else
    /// </summary>
    /// <remarks>source: http://damieng.com/blog/2006/07/07/url_parsing_and_manipulation_in_net</remarks>
    public class Url
    {
        private const string SchemeDecodeRegex = @"([^:]+):";
        private const string MailtoDecodeRegex = @"(mailto:)(([^@]+)@(.+))";
        private const string UrlDecodeRegex = @"([^:]+)://(([^:@]+)(:([^@]+))?@)?([^:/?#]+)(:([d]+))?([^?#]+)?(\?([^#]+))?(#(.*))?";
        private Url baseUrl;
        private string scheme;
        private long port;
        private bool useDefaultPort;
        private string hostName;
        private string user;
        private string password;
        private string path;
        private NameValueCollection query;
        private string fragment;
        private bool relative;

        public Url()
        {
            Reset();
        }

        public Url(string url)
        {
            Reset();
            this.FullUrl = url;
        }

        public Url(Url copyUrl)
        {
            Reset();
            CopyFrom(copyUrl);
        }

        public string Scheme
        {
            get { return scheme; }
            set { scheme = value.Trim(); }
        }

        public long Port
        {
            get { return port; }
            set
            {
                port = value;
                useDefaultPort = false;
            }
        }

        public bool UseDefaultPort
        {
            get { return useDefaultPort; }
            set { useDefaultPort = value; }
        }

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public NameValueCollection Query
        {
            get { return query; }
            set { query = value; }
        }

        public string Fragment
        {
            get { return fragment; }
            set { fragment = value; }
        }

        public string FullUrl
        {
            get
            {
                if (Scheme.Equals("mailto"))
                    return string.Format("{0}:{1}@{2}", Scheme, User, HostName);
                string newUrl = string.Empty;
                if (!Relative)
                {
                    newUrl += Scheme + "://";
                    if (User.Length > 0)
                    {
                        newUrl += User;
                        if (Password.Length > 0)
                            newUrl += ":" + Password;
                        newUrl += "@";
                    }
                    newUrl += HostName;
                    if (!UseDefaultPort)
                        newUrl += ":" + Port;
                }
                newUrl += Path;
                if (QueryString.Length > 0)
                    newUrl += "?" + QueryString;
                if (Fragment.Length > 0)
                    newUrl += "#" + Fragment;
                return newUrl;
            }
            set
            {
                Reset();
                Match m = new Regex(SchemeDecodeRegex).Match(value);
                if (m.Success)
                    if (m.Groups[1].Captures[0].Value.ToLower().Equals("mailto"))
                        DecodeMailTo(value);
                    else
                        this.DecodeUrl(value);
            }
        }

        public bool Relative
        {
            get { return relative; }
            set { relative = value; }
        }

        public string QueryString
        {
            get
            {
                string newQueryString = string.Empty;
                for (int queryIdx = 0; queryIdx < Query.Count; queryIdx++)
                {
                    newQueryString += (queryIdx == 0 ? "" : "&") + Query.Keys[queryIdx];
                    if (Query[queryIdx].Length > 0)
                        newQueryString += "=" + Query[queryIdx];
                }
                return newQueryString;
            }
            set
            {
                Query.Clear();
                AppendQueryString(value);
            }
        }

        public Url BaseUrl
        {
            get { return baseUrl; }
            set { baseUrl = value; }
        }

        public void AppendQueryString(string newQueryString)
        {
            string[] pairs = newQueryString.Split('&');
            for (int pairIdx = 0; pairIdx < pairs.Length; pairIdx++)
            {
                string pair = pairs[pairIdx];
                int keyPos = pair.IndexOf('=');
                if (keyPos > 0)
                {
                    string key = pair.Substring(0, keyPos);
                    string value = pair.Substring(keyPos + 1);
                    query[key] = value;
                }
                else
                    query[pair] = string.Empty;
            }
        }

        public void Reset()
        {
            Scheme = string.Empty;
            Port = 0;
            UseDefaultPort = true;
            HostName = string.Empty;
            User = string.Empty;
            Password = string.Empty;
            Path = string.Empty;
            Query = new NameValueCollection();
            Fragment = string.Empty;
            Relative = false;
        }

        public void CopyFrom(Url copyUrl)
        {
            Scheme = copyUrl.Scheme;
            User = copyUrl.User;
            Password = copyUrl.Password;
            HostName = copyUrl.HostName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType()) return false;
            return (this.FullUrl == ((Url)obj).FullUrl);
        }

        public override int GetHashCode()
        {
            return this.FullUrl.GetHashCode();
        }

        public override string ToString()
        {
            return this.FullUrl;
        }

        private void DecodeUrl(string value)
        {
            Match m = new Regex(UrlDecodeRegex).Match(value);
            if (m.Success)
            {
                if (m.Groups[1].Captures.Count == 1)
                    Scheme = m.Groups[1].Captures[0].Value;
                if (m.Groups[4].Captures.Count == 1)
                    User = m.Groups[4].Captures[0].Value;
                if (m.Groups[5].Captures.Count == 1)
                    Password = m.Groups[5].Captures[0].Value;
                if (m.Groups[6].Captures.Count == 1)
                    HostName = m.Groups[6].Captures[0].Value;
                if (m.Groups[8].Captures.Count == 1)
                    Port = Int32.Parse(m.Groups[8].Captures[0].Value);
                if (m.Groups[9].Captures.Count == 1)
                    Path = m.Groups[9].Captures[0].Value;
                if (m.Groups[11].Captures.Count == 1)
                    QueryString = m.Groups[11].Captures[0].Value;
                if (m.Groups[13].Captures.Count == 1)
                    Fragment = m.Groups[13].Captures[0].Value;
            }
        }

        private void DecodeMailTo(string value)
        {
            Match m = new Regex(MailtoDecodeRegex).Match(value);
            if (m.Success)
            {
                if (m.Groups[1].Captures.Count == 1)
                    Scheme = m.Groups[1].Captures[0].Value;
                if (m.Groups[2].Captures.Count == 1)
                    User = m.Groups[2].Captures[0].Value;
                if (m.Groups[3].Captures.Count == 1)
                    HostName = m.Groups[3].Captures[0].Value;
            }
        }

        public object Clone()
        {
            Url newClone = (Url)this.MemberwiseClone();
            newClone.Query = new NameValueCollection(Query);
            return newClone;
        }

        public int CompareTo(object obj)
        {
            if (obj == this) return 0;
            if (!(obj is Url)) return -1;
            return ((Url)obj).FullUrl.CompareTo(this.FullUrl);
        }
    }
}