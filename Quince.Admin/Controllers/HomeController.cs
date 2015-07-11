using Quince.Admin.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quince.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var pageModel = PagesManager.GetHomePageModel();
            return View(pageModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}