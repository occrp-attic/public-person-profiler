using Quince.Admin.Core.Managers;
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
    }
}