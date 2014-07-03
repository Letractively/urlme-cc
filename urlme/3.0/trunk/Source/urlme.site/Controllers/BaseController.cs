using System.Web.Mvc;

namespace urlme.site.Controllers
{
    public class BaseController : Controller
    {
        public new urlme.data.Models.User User { get { return urlme.data.Models.User.Current; } }
    }
}