using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CodeBDH.Display.DataAccess;

namespace CodeBDH.Display.Controllers
{
    public class DisplayController : Controller
    {
        // GET: Display
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcilPano()
        {
            return View();
        }
    }
}