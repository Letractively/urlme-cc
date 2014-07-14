using Newtonsoft.Json.Linq;
using ianhd.core.Extensions;

namespace seeitornot.model
{
    public partial class Theater
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string mapUrl { get; set; }
        
        public Theater() { }
    }
}
