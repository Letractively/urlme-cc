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
    public class LinkController : Controller
    {
        [Route("")]
        public JsonResult Get()
        {
            var links = Link.Get(User.Identity.Name.UserId());
            
            return this.Json(links, JsonRequestBehavior.AllowGet);
        }

        [Route("{linkId}")]
        [HttpDelete]
        public JsonResult Delete(int linkId)
        {
            var response = new SuccessResponse { Success = false };
            
            if (!Request.IsAuthenticated) {
               throw new HttpException(401, "Unauthorized access");
            }

            if (Link.Delete(linkId))
            {
                response.Success = true;
            }

            return this.Json(response, JsonRequestBehavior.AllowGet);
        }
	}
}