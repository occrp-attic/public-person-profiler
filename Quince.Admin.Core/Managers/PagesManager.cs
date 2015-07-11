using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Models;
using Quince.Admin.Core.Models.Entity;
using Quince.Admin.Core.Models.Pages;
using Quince.Admin.Core.Models.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public class PagesManager
    {
        public static HomePageModel GetHomePageModel()
        {
            var pageModel = new HomePageModel();
            var context = new AdminDbContext();
            pageModel.RelationTypes = RelationTypeManager.GetHomeRelationTypes(context);
            pageModel.EntityTypes = EntityTypeManager.GetHomeEntitiesTypes(context);
            return pageModel;
        }

        public static Models.Pages.EntitiesBrowsePageModel GetEntitiesBrowsePageModel(long entityTypeId, int page)
        {
            var pageModel = new EntitiesBrowsePageModel();
            var pageSize = 20;
            var skip = page * pageSize;
            var context = new AdminDbContext();
            var entityType = context.EntityTypes.Find(entityTypeId);
            var totalResult = entityType.Entities.Count();
            pageModel.Entities = entityType.Entities.OrderBy(e => e.Name).Skip(skip).Take(pageSize).Select(e => new EntityCardModel { Id = e.Id, Name = e.Name, Image = e.Image ?? e.Type.DefaultImage, Type = e.Type.Name, TypeId = e.TypeId });
            pageModel.TotalPages = totalResult % 20 == 0 ? totalResult / 20 : (totalResult / 20) + 1;
            pageModel.CurrentPage = page + 1;
            pageModel.Type = entityType.Name;
            pageModel.CurrentEntityTypeId = entityTypeId;
            return pageModel;
        }
        public static RelationBrowsePageModel GetRelationBrowsePageModel(long entityTypeId, int page)
        {
            var pageModel = new RelationBrowsePageModel();
            var pageSize = 20;
            var skip = page * pageSize;
            var context = new AdminDbContext();
            var relationType = context.RelationTypes.Find(entityTypeId);
            var totalResult = relationType.Relations.Count();
            pageModel.Type = relationType.Name;
            pageModel.Relations = relationType.Relations.OrderByDescending(rt => rt.Id).Skip(skip).Take(pageSize)
                .Select(r => new RelationCardModel
                {
                    Id = r.Id,
                    Attributes = r.Attributes.Take(3).Select(attr => new AttributeDisplayModel
                    {
                        Name = attr.Name,
                        Value = attr.Value
                    }),
                    Entities = r.RelationEntities.Select(re => new RelationCardEntity
                    {
                        Id = re.EntityId,
                        Name = re.Entity.Name,
                        Type = re.MemberType != null ? re.Type.Name : "",
                        EntityType = re.Entity.Type.Name
                    })
                });

            pageModel.TotalPages = totalResult % 20 == 0 ? totalResult / 20 : (totalResult / 20) + 1;
            pageModel.CurrentPage = page + 1;
            pageModel.CurrentEntityTypeId = entityTypeId;
            return pageModel;
        }

        public static EntityRelationsPageModel GetEntityRelationsPageModel(long entityId, long relationTypeId, int page)
        {
            var pageModel = new EntityRelationsPageModel();
            var pageSize = 20;
            var skip = page * pageSize;
            var context = new AdminDbContext();
            var entity = context.Entities.Find(entityId);
            var relationType = context.RelationTypes.Find(relationTypeId);
            var relations = context.Relations.Where(r => r.TypeId.Equals(relationTypeId) && r.RelationEntities.Any(re => re.EntityId.Equals(entityId))).OrderByDescending(r => r.Id).Skip(skip).Take(pageSize).ToList();
            pageModel.CurrentRelationTypeId = relationTypeId;
            pageModel.Id = entity.Id;
            pageModel.Name = entity.Name;
            pageModel.RelationType = relationType.Name;
            pageModel.Type = entity.Type.Name;
            pageModel.Image = entity.Image ?? entity.Type.DefaultImage;
            var totalRelations = relations.Count();
            pageModel.TotalPages = totalRelations % 20 == 0 ? totalRelations / 20 : (totalRelations / 20) + 1;
            pageModel.CurrentPage = page + 1;
            pageModel.Relations = relations.Select(r => new RelationCardModel
            {
                Id = r.Id,
                Attributes = r.Attributes.Take(3).Select(attr => new AttributeDisplayModel
                {
                    Name = attr.Name,
                    Value = attr.Value
                }),
                Entities = r.RelationEntities.Select(re => new RelationCardEntity
                {
                    Id = re.EntityId,
                    Name = re.Entity.Name,
                    Type = re.MemberType != null ? re.Type.Name : "",
                    EntityType = re.Entity.Type.Name
                })
            });

            return pageModel;
        }
    }
}
