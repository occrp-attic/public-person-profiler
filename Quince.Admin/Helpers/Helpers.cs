using Quince.Admin.Core.Managers;
using Quince.Admin.Core.Models.Entity;
using Quince.Admin.Core.Models.EntityType;
using Quince.Admin.Core.Models.RelationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;



namespace Quince.Admin.Helpers
{
    public static class Helpers {
        public static IEnumerable<SelectListItem> GetEntityTypes(string selectedId)
        {
            var entityTypes = EntityTypeManager.GetEntityTypes();
            return entityTypes.Select(et => new SelectListItem { Selected = et.Id.ToString().Equals(selectedId), Text = et.Name, Value = et.Id.ToString() });
        }
        public static IEnumerable<SelectListItem> GetRelationTypes(string selectedId)
        {
            var relationTypes = RelationTypeManager.GetRelationTypes();
            return relationTypes.Select(et => new SelectListItem { Selected = et.Id.ToString().Equals(selectedId), Text = et.Name, Value = et.Id.ToString() });
        }
        public static IEnumerable<EntityTypeModel> GetEntityTypes()
        {
            return EntityTypeManager.GetEntityTypes(true);
        }
        public static IEnumerable<RelationTypeModel> GetRelationTypes()
        {
            return RelationTypeManager.GetRelationTypes(true);
        }
    }
}