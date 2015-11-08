using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coub.UI.Controllers
{
    public class ImageController : Controller
    {
        //
        // GET: /Image/
        [HttpGet]
        public ActionResult GetBase64Image()
        {
            byte[] data =new byte[1];
            return Json(new {base64image = Convert.ToBase64String(data)}, JsonRequestBehavior.AllowGet);
        }

    }
}
