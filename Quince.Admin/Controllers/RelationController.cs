using Quince.Admin.Core.Managers;
using Quince.Admin.Core.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quince.Admin.Controllers
{
    public class RelationController : Controller
    {
        // GET: Relation
        public ActionResult Browse(long id, int? page)
        {
            var pageModel = PagesManager.GetRelationBrowsePageModel(id, (page ?? 1) - 1);
            return View(pageModel);
        }
    }
}