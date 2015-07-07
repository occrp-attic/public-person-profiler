using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Contracts;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.Entity;
using Quince.Admin.Core.Models.Relation;
using Quince.Utils.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public class RelationManager
    {
        public static DTResponse GetRelations(DTRequest request)
        {
            var context = new AdminDbContext();
            var response = new DTResponse()
            {
                draw = request.draw
            };
            var entities = context.Relations;
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
                    case 12:
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
            var responseData = data.Select(u => new RelationDisplayModel { Id = u.Id, Type = u.Type.Name});
            response.data = responseData;

            response.recordsTotal = context.EntityTypes.Count();
            return response;
        }
        public static async Task<Utils.Messages.Response> AddRelationAsync(RelationAddEditModel relationModel)
        {
            var response = new Response();
            var context = new AdminDbContext();
            ValidateRelation(relationModel, response);
            if (!response.Success)
            {
                return response;
            }
            else
            {
                var relation = new Relation();
                relation.TypeId = relationModel.TypeId;
                context.Relations.Add(relation);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Utils.Messages.Response> EditRelationAsync(RelationAddEditModel relationModel)
        {
            var response = new Response();
            var context = new AdminDbContext();
            ValidateRelation(relationModel, response);
            if (!response.Success)
            {
                return response;
            }
            var relation = context.Relations.Find(relationModel.Id);
            relation.TypeId = relationModel.TypeId;
            await context.SaveChangesAsync();
            return response;
        }
        public static async Task<Utils.Messages.Response> DeleteRelationAsync(long id)
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
        public static RelationAddEditModel GetRelationEditModel(long id)
        {
            var context = new AdminDbContext();
            var entity = context.Relations.Find(id);
            if (entity != null)
            {
                return new RelationAddEditModel() { Id = entity.Id, TypeId = entity.TypeId};
            }
            return null;
        }

        public static RelationDisplayModel GetRelationDisplayModel(long id)
        {
            var context = new AdminDbContext();
            var entity = context.Relations.Find(id);
            if (entity != null)
            {
                return new RelationDisplayModel() { Id = entity.Id, Type = entity.Type.Name };
            }
            return null;
        }
        public static Response ValidateRelation(RelationAddEditModel relation, Response response = null)
        {
            response = response ?? new Response();
            
            if (relation.TypeId == 0)
            {
                response.AddMessage(false, "Select a Relation Type", ResponseMessageType.Warning);
            }
            return response;
        }
    }
}
