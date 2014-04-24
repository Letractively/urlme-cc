using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using urlme.data.Models;
using urlme.site.Models.Response;

namespace urlme.site.Controllers
{
    [RoutePrefix("links")]
    [Authorize]
    public class LinkController : Controller
    {
        [Route("")]
        public JsonResult Get()
        {
            var links = Link.GetByUserId(User.Identity.Name.UserId());
            
            return this.Json(links, JsonRequestBehavior.AllowGet);
        }

        [Route("")]
        [HttpPost]
        public JsonResult Post(Link source)
        {
            source.UserId = User.Identity.Name.UserId();
            var result = urlme.data.Services.SiteService.LinkSave(source);
            return this.Json(result, JsonRequestBehavior.DenyGet);
        }

        [Route("overwrite")]
        [HttpPost]
        public JsonResult Overwrite(Link source)
        {
            source.UserId = User.Identity.Name.UserId();
            var result = urlme.data.Services.SiteService.LinkOverwrite(source);
            return this.Json(result, JsonRequestBehavior.DenyGet);
        }

        [Route("{linkId}")]
        [HttpDelete]
        public JsonResult Delete(int linkId)
        {
            var response = new SuccessResponse { Success = false };

            if (Link.Delete(linkId, User.Identity.Name.UserId()))
            {
                response.Success = true;
            }

            return this.Json(response, JsonRequestBehavior.AllowGet);
        }
	}
}