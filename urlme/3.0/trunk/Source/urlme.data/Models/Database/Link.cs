using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urlme.data.Models
{
    public sealed partial class Link : BaseModel
    {
        public int LinkId { get; set; }
        public int UserId { get; set; }
        public string Path { get; set; }
        public string DestinationUrl { get; set; }
        public int HitCount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
