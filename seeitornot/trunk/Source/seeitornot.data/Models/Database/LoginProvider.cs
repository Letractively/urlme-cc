using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeitornot.data.Models.Database
{
    public sealed partial class LoginProvider : BaseModel
    {
        public int LoginProviderId { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthEndpoint { get; set; }
        public string Scope { get; set;  }
        public string AccessTokenEndpoint { get; set; }
        public string AccessTokenEndpointMethod { get; set; }
        public string UserInfoEndpoint { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
