using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Contracts;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.Entity;
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
            if(!response.Success)
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

        private static Response ValidateEntityRelation(AddEntityRelationModel addEntityRelationModel, Response response=null)
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
    }
}
