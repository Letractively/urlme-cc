using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeitornot.data.Models
{
    public sealed partial class User : BaseModel
    {
        public int UserId { get; set; }
        public int LoginProviderId { get; set; }
        public string LoginProviderUserKey { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
