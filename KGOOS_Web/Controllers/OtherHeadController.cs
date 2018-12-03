using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KGOOS_Web.Controllers
{
    public class OtherHeadController : Controller
    {
        //
        // GET: /OtherHead/

        public ActionResult NewMessage()
        {
            return View();
        }
        public ActionResult Education()
        {
            return View();
        }

        public ActionResult Logistics()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }        
    }
}
