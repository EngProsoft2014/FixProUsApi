
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FixProUsApi.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SendNotifications()
        {
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult SendNotifications([FromBody] string message)
        {
            return View();
        }
    }
}