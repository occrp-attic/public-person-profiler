using Quince.Admin.Core.Managers;
using Quince.Admin.Core.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quince.Admin.Controllers
{
    public class EntityController : Controller
    {
        // GET: Entity
        public ActionResult Details(long id)
        {
            var model = EntityManager.GetEntity(id);
            return View(model);
        }
        public ActionResult Browse(long id, int? page)
        {
            EntitiesBrowsePageModel pageModel = PagesManager.GetEntitiesBrowsePageModel(id, (page ?? 1) - 1);
            return View(pageModel);
        }
        [HttpPost]
        public JsonResult GetEntityAmount(int entityId, int entityType, int otherEntityType)
        {
            var result = EntityManager.GetEntityAmount(entityId, entityType, otherEntityType);
            return Json(result);
        }
        [HttpPost]
        public JsonResult GetDateAmount(int entityId, int entityType, int otherEntityType)
        {
            var result = EntityManager.GetDateAmount(entityId, entityType, otherEntityType);
            return Json(result);
        }

        public ActionResult RelationType(long id, long relationType, int? page)
        {
            var pageModel = PagesManager.GetEntityRelationsPageModel(id, relationType, (page ?? 1) - 1);
            return View(pageModel);
        }
    }
}