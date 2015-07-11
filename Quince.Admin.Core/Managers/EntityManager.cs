using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Contracts;
using Quince.Admin.Core.Models;
using Quince.Admin.Core.Models.Charts;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.Entity;
using Quince.Admin.Core.Models.Pages;
using Quince.Admin.Core.Models.Relation;
using Quince.Admin.Core.Models.RelationType;
using Quince.Utils.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public class EntityManager
    {
        public static DTResponse GetEntities(DTRequest request)
        {
            var context = new AdminDbContext();
            var response = new DTResponse()
            {
                draw = request.draw
            };
            var entities = context.Entities;
            var data = entities.AsEnumerable();
            if (request.order != null && request.order.Any())
            {
                var asc = request.order[0].dir == "asc";
                switch (request.order[0].column)
                {
                    case 0:
                        {
                            data = asc ? data.OrderBy(u => u.Id) : data.OrderByDescending(u => u.Id);
                            break;
                        }
                    case 1:
                        {
                            data = asc ? data.OrderBy(u => u.Name) : data.OrderByDescending(u => u.Name);
                            break;
                        }
                    case 2:
                        {
                            data = asc ? data.OrderBy(u => u.Type.Name) : data.OrderByDescending(u => u.Type.Name);
                            break;
                        }

                    default:
                        {
                            data = data.OrderBy(u => u.Id);
                            break;
                        }

                }
            }
            response.recordsFiltered = data.Count();
            data = data.Skip(request.start).Take(request.length);
            var responseData = data.Select(u => new EntityTableModel { Id = u.Id, Type = u.Type.Name, Name = u.Name });
            response.data = responseData;

            response.recordsTotal = context.Entities.Count();
            return response;
        }
        public static IEnumerable<object> GetEntities(string query)
        {
            var context = new AdminDbContext();
            var response = context.Entities.Where(e => e.Name.Contains(query ?? "")).OrderBy(e => e.Name).Take(25).Select(o => new { id = o.Id, text = o.Name });
            return response;
        }
        public static async Task<Utils.Messages.Response> AddEntityAsync(EntityAddEditModel entityModel)
        {
            var response = new Response();
            var context = new AdminDbContext();
            ValidateEntity(entityModel, response);
            if (!response.Success)
            {
                return response;
            }
            else
            {
                var entity = new Entity();
                entity.TypeId = entityModel.TypeId;
                entity.Name = entityModel.Name;
                context.Entities.Add(entity);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Utils.Messages.Response> EditEntityAsync(EntityAddEditModel entityModel)
        {
            var response = new Response();
            var context = new AdminDbContext();
            ValidateEntity(entityModel, response);
            if (!response.Success)
            {
                return response;
            }
            var entity = context.Entities.Find(entityModel.Id);
            entity.TypeId = entityModel.TypeId;
            entity.Name = entityModel.Name;
            await context.SaveChangesAsync();
            return response;

        }
        public static async Task<Utils.Messages.Response> DeleteEntityAsync(long id)
        {
            var response = new Response();
            var context = new AdminDbContext();
            var entity = context.Entities.Find(id);
            if (entity != null)
            {
                context.Entities.Remove(entity);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static EntityAddEditModel GetEntityEditModel(long id)
        {
            var context = new AdminDbContext();
            var entity = context.Entities.Find(id);
            if (entity != null)
            {
                return new EntityAddEditModel() { Id = entity.Id, TypeId = entity.TypeId, Name = entity.Name };
            }
            return null;
        }
        public static EntityTableModel GetEntityTableModel(long id)
        {
            var context = new AdminDbContext();
            var entity = context.Entities.Find(id);
            if (entity != null)
            {
                return new EntityTableModel() { Id = entity.Id, Type = entity.Type.Name, Name = entity.Name };
            }
            return null;
        }
        public static Response ValidateEntity(EntityAddEditModel entity, Response response = null)
        {
            response = response ?? new Response();
            if (string.IsNullOrEmpty(entity.Name))
            {
                response.AddMessage(false, "The name should not be empty", ResponseMessageType.Warning);
            }
            if (entity.TypeId == 0)
            {
                response.AddMessage(false, "Select an Entity Type", ResponseMessageType.Warning);
            }
            return response;
        }
        public static AddEntityRelationModel GetEntityRelationModel(long id)
        {
            var context = new AdminDbContext();
            var entity = context.Entities.Find(id);
            var entityRelation = new AddEntityRelationModel
            {
                Id = entity.Id,
                Name = entity.Name
            };
            return entityRelation;
        }
        public static async Task<Response> AddEntityRelation(AddEntityRelationModel addEntityRelationModel)
        {
            var response = ValidateEntityRelation(addEntityRelationModel);
            if (!response.Success)
            {
                return response;
            }
            var relation = new Relation() { TypeId = addEntityRelationModel.RelationTypeId };
            relation.RelationEntities.Add(new RelationEntity { EntityId = addEntityRelationModel.Id });
            relation.RelationEntities.Add(new RelationEntity { EntityId = addEntityRelationModel.OtherEntityId });

            var context = new AdminDbContext();
            context.Relations.Add(relation);
            await context.SaveChangesAsync();
            return response;
        }
        private static Response ValidateEntityRelation(AddEntityRelationModel addEntityRelationModel, Response response = null)
        {
            response = response ?? new Response();
            if (addEntityRelationModel.Id == 0)
            {
                response.AddMessage(false, "Please select an entity", ResponseMessageType.Warning);
            }
            if (addEntityRelationModel.RelationTypeId == 0)
            {
                response.AddMessage(false, "Please select a relation type", ResponseMessageType.Warning);
            }
            if (addEntityRelationModel.OtherEntityId == 0)
            {
                response.AddMessage(false, "Please select the second entity", ResponseMessageType.Warning);
            }
            return response;
        }
        public static EntityDisplayModel GetEntity(long id)
        {
            var context = new AdminDbContext();
            var entity = context.Entities.Find(id);
            var model = new EntityDisplayModel();
            model.Id = entity.Id;
            model.Image = entity.Image ?? entity.Type.DefaultImage;
            model.Name = entity.Name;
            model.Type = entity.Type.Name;
            model.Description = entity.Description;
            model.Attributes = entity.Attributes.Select(a => new AttributeDisplayModel { Name = a.Name, Value = a.Value });
            model.RelationTypes = entity.RelationEntities.Select(re => re.Relation.Type).Distinct().Select(rt => new SiteRelationTypeDisplayModel { Id = rt.Id, Name = rt.Name }).ToList();
            // var relations = entity.RelationEntities.Select(re => re.Relation).Select(r => new SiteRelationDisplayModel() {TypeId=r.TypeId, Type= r.Type.Name, Entities = r.RelationEntities.Where(re => re.EntityId != entity.Id).Select(re => re.Entity).Select(e=> new EntityTableModel(){Id = e.Id, Name = e.Name, Type = e.Type.Name, Image = e.Image??e.Type.DefaultImage})});
            var relations = entity.RelationEntities.Select(re => re.Relation);//.Select(r => new SiteRelationDisplayModel() { TypeId = r.TypeId, Type = r.Type.Name, Attributes = r.Attributes.Select(a => new AttributeDisplayModel { Name = a.Name, Value = a.Value }), Entities = r.RelationEntities.Where(re => re.EntityId != entity.Id).Select(re => re.Entity).Select(e => new EntityTableModel() { Id = e.Id, Name = e.Name, Type = e.Type.Name, Image = e.Image ?? e.Type.DefaultImage }) });
            for (var i=0; i< model.RelationTypes.Count(); i++)
            {
                var relationType = model.RelationTypes.ElementAt(i);
                relationType.TotalRelations = relations.Where(r => r.TypeId.Equals(relationType.Id)).Count();
                relationType.Relations.AddRange(relations.Where(r => r.TypeId.Equals(relationType.Id)).OrderByDescending(r => r.Id).Take(5).Select(r => new SiteRelationDisplayModel() { TypeId = r.TypeId, Type = r.Type.Name, Attributes = r.Attributes.Select(a => new AttributeDisplayModel { Name = a.Name, Value = a.Value }), Entities = r.RelationEntities.Where(re => re.EntityId != entity.Id).Select(re => re.Entity).Select(e => new EntityTableModel() { Id = e.Id, Name = e.Name, Type = e.Type.Name, Image = e.Image ?? e.Type.DefaultImage }) }));
            }
            model.References = entity.References.Select(r => new ReferenceDisplayModel { Description = r.Description, Document = r.Document, Link = r.Link, Title = r.Title });
            return model;
        }
        public static bool CheckEntityByAttributeNameAndValue(int entityType, string name, string value, AdminDbContext context = null)
        {
            context = context ?? new AdminDbContext();
            return context.Entities.Any(e => e.Type.Code.Equals(entityType) && e.Attributes.Any(attr => attr.Name.Equals(name) && attr.Value.Equals(value)));
        }

        public static IEnumerable<EntityAmount> GetEntityAmount(int entityId, int entityCode, int relationEntityCode)
        {
            var context = new AdminDbContext();
            var entity = context.Entities.Find(entityId);
            var relations = entity.RelationEntities.Where(re => re.MemberType == entityCode)
            .Select(re => re.Relation)
            .Where(r => r.Attributes.Any(attr => attr.Name.Equals("Amount"))).Distinct();
            var result = new List<EntityAmount>();
            foreach (var relation in relations)
            {
                var amount = Convert.ToDecimal(relation.Attributes.First(attr => attr.Name.Equals("Amount")).Value);
                var entityName = relation.RelationEntities.First(re => re.MemberType == relationEntityCode).Entity.Name;
                if (result.Any(r => r.Name.Equals(entityName)))
                {
                    result.First(r => r.Name.Equals(entityName)).Amount += amount;
                }
                else
                {
                    var entityAmount = new EntityAmount() { Amount = amount, Name = entityName };
                    result.Add(entityAmount);
                }
            }
            return result;
        }
        public static IEnumerable<DateAmount> GetDateAmount(int entityId, int entityCode, int relationEntityCode)
        {
            var context = new AdminDbContext();
            var entity = context.Entities.Find(entityId);
            //var relations = entity.RelationEntities.Where(re => re.MemberType == entityCode)
            //.Select(re => re.Relation)
            //.Where(r => r.Attributes.Any(attr => attr.Name.Equals("Amount"))&&r.Attributes.Any(attr1=>attr1.Name.Equals("Contract date"))).Distinct();
            var relations = context.Relations.Where(r => r.RelationEntities.Any(re => re.EntityId.Equals(entityId) && re.MemberType == entityCode) && r.RelationEntities.Any(re1 => re1.MemberType == relationEntityCode) && r.Attributes.Any(attr => attr.Name.Equals("Amount")) && r.Attributes.Any(attr1 => attr1.Name.Equals("Contract date")));
            var result = new List<DateAmount>();
            foreach (var relation in relations)
            {
                var amount = Convert.ToDecimal(relation.Attributes.First(attr => attr.Name.Equals("Amount")).Value);
                var entityName = relation.Attributes.First(attr => attr.Name.Equals("Contract date")).Value;
                if (result.Any(r => r.Date.Equals(entityName)))
                {
                    result.First(r => r.Date.Equals(entityName)).Amount += amount;
                }
                else
                {
                    var entityAmount = new DateAmount() { Amount = amount, Date = entityName };
                    result.Add(entityAmount);
                }
            }
            return result;
        }

    }
}
