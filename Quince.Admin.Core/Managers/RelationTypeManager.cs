using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Contracts;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.RelationType;
using Quince.Utils.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public class RelationTypeManager
    {
        public static DTResponse GetRelationTypes(DTRequest request)
        {
            var context = new AdminDbContext();
            var response = new DTResponse()
            {
                draw = request.draw
            };
            var relationTypes = context.RelationTypes;
            var data = relationTypes.AsEnumerable();
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
            var responseData = data.Select(u => new RelationTypeDisplayModel { Id = u.Id, Code = u.Code, Name = u.Name, Relations = u.Relations.Count() });
            response.data = responseData;

            response.recordsTotal = context.RelationTypes.Count();
            return response;
        }
        public static IEnumerable<RelationTypeModel> GetRelationTypes(bool withItems = false)
        {
            var context = new AdminDbContext();
            var responseData = context.RelationTypes.AsEnumerable();
            if (withItems)
            {
                responseData = responseData.Where(rt => rt.Relations.Any());

            }
                return responseData.OrderBy(et => et.Name).Select(u => new RelationTypeDisplayModel { Id = u.Id, Code = u.Code, Name = u.Name });
        }
        public static async Task<Utils.Messages.Response> AddRelationTypeAsync(RelationTypeAddEditModel relationTypeModel)
        {

            var response = new Response();
            var context = new AdminDbContext();
            if (context.RelationTypes.Any(u => u.Code.Equals(relationTypeModel.Code)))
            {
                response.AddMessage(false, "This code is already registered", ResponseMessageType.Warning);
            }
            else
            {
                var relationType = new RelationType();
                relationType.Code = relationTypeModel.Code;
                relationType.Name = relationTypeModel.Name;
                context.RelationTypes.Add(relationType);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Utils.Messages.Response> EditRelationTypeAsync(RelationTypeAddEditModel relationTypeModel)
        {

            var response = new Response();
            var context = new AdminDbContext();
            if (context.RelationTypes.Any(u => u.Code.Equals(relationTypeModel.Code) && !u.Id.Equals(relationTypeModel.Id)))
            {
                response.AddMessage(false, "This code is already registered on another entity", ResponseMessageType.Warning);
            }
            else
            {
                var relationType = context.RelationTypes.Find(relationTypeModel.Id);
                relationType.Code = relationTypeModel.Code;
                relationType.Name = relationTypeModel.Name;
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static async Task<Utils.Messages.Response> DeleteRelationTypeAsync(long id)
        {
            var response = new Response();
            var context = new AdminDbContext();
            var relationType = context.RelationTypes.Find(id);
            if (relationType != null)
            {
                context.RelationTypes.Remove(relationType);
                await context.SaveChangesAsync();
            }
            return response;
        }
        public static RelationTypeAddEditModel GetRelationTypeEditModel(long id)
        {
            var context = new AdminDbContext();
            var relationType = context.RelationTypes.Find(id);
            if (relationType != null)
            {
                return new RelationTypeAddEditModel() { Id = relationType.Id, Code = relationType.Code, Name = relationType.Name };
            }
            return null;
        }

        internal static IEnumerable<HomePageRelationTypeDisplayModel> GetHomeRelationTypes(AdminDbContext context=null)
        {
            context = context ?? new AdminDbContext();
            return context.RelationTypes.OrderBy(rt => rt.Name).Where(rt=>rt.Relations.Any()).Select(rt => new HomePageRelationTypeDisplayModel() { Id = rt.Id, Name = rt.Name, Count = rt.Relations.Count() });
        }
    }
}
