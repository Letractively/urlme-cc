using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ianhd.core.Data;

namespace urlme.data.Models
{
    public sealed partial class User : BaseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int? FacebookUserId { get; set; }
        public bool AdminInd { get; set; }
        public DateTime CreateDate { get; set; }

        [IgnoreField]
        public bool IsAuthenticated
        {
            get { return this.UserId != 0; }
        }
    }
}
