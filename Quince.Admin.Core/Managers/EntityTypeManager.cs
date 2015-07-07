using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Contracts;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.EntityType;
using Quince.Utils.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public class EntityTypeManager
    {
        public static DTResponse GetEntityTypes(DTRequest request)
        {
            var context = new AdminDbContext();
            var response = new DTResponse()
            {
                draw = request.draw
            };
            var entityTypes = context.EntityTypes;
            var data = entityTypes.AsEnumerable();
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
                            data = asc ? data.OrderBy(u => u.Code) : data.OrderByDescending(u => u.Code);
                            break;
                        }
                    case 2:
                        {
                            data = asc ? data.OrderBy(u => u.Name) : data.OrderByDescending(u => u.Name);
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
            var responseData = data.Select(u => new EntityTypeTableModel { Id = u.Id, Code = u.Code, Name = u.Name, Entities = u.Entities.Count() });
            response.data = responseData;

            response.recordsTotal = context.EntityTypes.Count();
            return response;
        }
        public static IEnumerable<EntityTypeModel> GetEntityTypes()
        {
            var context = new AdminDbContext();
            var responseData = context.EntityTypes.OrderBy(et=>et.Name).Select(u => new EntityTypeTableModel { Id = u.Id, Code = u.Code, Name = u.Name });
            return responseData;
        }
        public static async Task<Utils.Messages.Response> AddEntiTypeAsync(EntityTypeAddEditModel entityTypeModel)
        {

            var response = new Response();
            var context = new AdminDbContext();
            if (context.EntityTypes.Any(u => u.Code.Equals(entityTypeModel.Code)))
            {
                response.AddMessage(false, "This code is already registered", ResponseMessageType.Warning);
            }
            else
            {
                var entityType = new EntityType();
                entityType.Code = entityTypeModel.Code;
                entityType.Name = entityTypeModel.Name;
                context.EntityTypes.Add(entityType);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Utils.Messages.Response> EditEntiTypeAsync(EntityTypeAddEditModel entityTypeModel)
        {

            var response = new Response();
            var context = new AdminDbContext();
            if (context.EntityTypes.Any(u => u.Code.Equals(entityTypeModel.Code)&&!u.Id.Equals(entityTypeModel.Id)))
            {
                response.AddMessage(false, "This code is already registered on another entity", ResponseMessageType.Warning);
            }
            else
            {
                var entityType = context.EntityTypes.Find(entityTypeModel.Id);
                entityType.Code = entityTypeModel.Code;
                entityType.Name = entityTypeModel.Name;
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Utils.Messages.Response> DeleteEntiTypeAsync(long id)
        {
            var response = new Response();
            var context = new AdminDbContext();
            var entityType = context.EntityTypes.Find(id);
            if(entityType!=null)
            {
                context.EntityTypes.Remove(entityType);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static EntityTypeAddEditModel GetEntityTypeEditModel(long id)
        {
            var context = new AdminDbContext();
            var entityType = context.EntityTypes.Find(id);
            if (entityType != null)
            {
                return new EntityTypeAddEditModel() { Id = entityType.Id, Code = entityType.Code, Name = entityType.Name };
            }
            return null;
        }
    }
}
