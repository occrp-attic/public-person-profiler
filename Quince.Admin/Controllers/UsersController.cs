using Quince.Admin.Core.Managers;
using Quince.Admin.Core.Models.DataTables;
using Quince.Admin.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Quince.Admin.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        // GET: Users
        public PartialViewResult Index()
        {
            var users = UserManager.GetUsers();
            return PartialView(users);
        }
        [HttpPost]
        public JsonResult GetUsers(DTRequest dtRequest)
        {
            var response = UserManager.GetUsers(dtRequest);
            return Json(response);
        }

        public PartialViewResult Add()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<JsonResult> Add(AddUserModel userModel)
        {
            var response = await UserManager.CreateUserAsync(userModel);
            return Json(response);
        }

        public PartialViewResult Edit(int id)
        {
            var user = UserManager.GetUser(id);
            return PartialView("Add", user);
        }
        [HttpPost]
        public async Task<JsonResult> Edit(AddUserModel userModel)
        {
            var response = await UserManager.UpdateUserAsync(userModel);
            return Json(response);
        }

        public PartialViewResult Delete(int id)
        {
            var user = UserManager.GetUser(id);
            return PartialView(user);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(UserModel model)
        {
            var response = await UserManager.DeleteUserAsync(model.Id);
            return Json(response);
        }

    }
}