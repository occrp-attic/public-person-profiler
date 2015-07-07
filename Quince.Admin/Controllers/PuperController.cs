using Quince.Admin.Core.Managers;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.Entity;
using Quince.Admin.Core.Models.EntityType;
using Quince.Admin.Core.Models.Relation;
using Quince.Admin.Core.Models.RelationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Quince.Admin.Controllers
{
    [Authorize]
    public class PuperController : Controller
    {
        // GET: Puper
        #region EntityTypes
        public PartialViewResult EntityTypes()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetEntityTypes(DTRequest dtRequest)
        {
            var response = EntityTypeManager.GetEntityTypes(dtRequest);
            return Json(response);
        }

        public PartialViewResult AddEntityType()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> AddEntityType(EntityTypeAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await EntityTypeManager.AddEntiTypeAsync(entityType);
            return Json(response);
        }

        public PartialViewResult EditEntityType(long id)
        {
            var entityType = EntityTypeManager.GetEntityTypeEditModel(id);
            return PartialView("AddEntityType", entityType);
        }
        [HttpPost]
        public async Task<JsonResult> EditEntityType(EntityTypeAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await EntityTypeManager.EditEntiTypeAsync(entityType);
            return Json(response);
        }

        public PartialViewResult DeleteEntityType(long id)
        {
            var entityType = EntityTypeManager.GetEntityTypeEditModel(id);
            return PartialView(entityType);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteEntityType(EntityTypeAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await EntityTypeManager.DeleteEntiTypeAsync(entityType.Id);
            return Json(response);
        } 
        #endregion

        #region Entities
        public PartialViewResult Entities()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetEntities(DTRequest dtRequest)
        {
            var response = EntityManager.GetEntities(dtRequest);
            return Json(response);
        }

        public PartialViewResult AddEntity()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> AddEntity(EntityAddEditModel entity)
        {
            Quince.Utils.Messages.Response response = await EntityManager.AddEntityAsync(entity);
            return Json(response);
        }

        public PartialViewResult EditEntity(long id)
        {
            var entityType = EntityManager.GetEntityEditModel(id);
            return PartialView("AddEntity", entityType);
        }
        [HttpPost]
        public async Task<JsonResult> EditEntity(EntityAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await EntityManager.EditEntityAsync(entityType);
            return Json(response);
        }

        public PartialViewResult DeleteEntity(long id)
        {
            var entityType = EntityManager.GetEntityTableModel(id);
            return PartialView(entityType);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteEntity(EntityTypeAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await EntityManager.DeleteEntityAsync(entityType.Id);
            return Json(response);
        } 
        #endregion

        #region RelationTypes
        public PartialViewResult RelationTypes()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetRelationTypes(DTRequest dtRequest)
        {
            var response = RelationTypeManager.GetRelationTypes(dtRequest);
            return Json(response);
        }

        public PartialViewResult AddRelationType()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> AddRelationType(RelationTypeAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await RelationTypeManager.AddRelationTypeAsync(entityType);
            return Json(response);
        }

        public PartialViewResult EditRelationType(long id)
        {
            var entityType = RelationTypeManager.GetRelationTypeEditModel(id);
            return PartialView("AddRelationType", entityType);
        }
        [HttpPost]
        public async Task<JsonResult> EditRelationType(RelationTypeAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await RelationTypeManager.EditRelationTypeAsync(entityType);
            return Json(response);
        }

        public PartialViewResult DeleteRelationType(long id)
        {
            var entityType = RelationTypeManager.GetRelationTypeEditModel(id);
            return PartialView(entityType);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRelationType(RelationTypeAddEditModel entityType)
        {
            Quince.Utils.Messages.Response response = await RelationTypeManager.DeleteRelationTypeAsync(entityType.Id);
            return Json(response);
        }
        #endregion



        #region Relations
        public PartialViewResult Relations()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetRelations(DTRequest dtRequest)
        {
            var response = RelationManager.GetRelations(dtRequest);
            return Json(response);
        }

        public PartialViewResult AddRelation()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> AddRelation(RelationAddEditModel relation)
        {
            Quince.Utils.Messages.Response response = await RelationManager.AddRelationAsync(relation);
            return Json(response);
        }

        public PartialViewResult EditRelation(long id)
        {
            var relation = RelationManager.GetRelationEditModel(id);
            return PartialView("AddRelation", relation);
        }
        [HttpPost]
        public async Task<JsonResult> EditRelation(RelationAddEditModel relation)
        {
            Quince.Utils.Messages.Response response = await RelationManager.EditRelationAsync(relation);
            return Json(response);
        }

        public PartialViewResult DeleteRelation(long id)
        {
            var relation = RelationManager.GetRelationDisplayModel(id);
            return PartialView(relation);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRelation(RelationTypeAddEditModel relation)
        {
            Quince.Utils.Messages.Response response = await RelationManager.DeleteRelationAsync(relation.Id);
            return Json(response);
        }
        #endregion

        public PartialViewResult AddEntityRelation(int id)
        {
            var entity = 
        }
    }
}