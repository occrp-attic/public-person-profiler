using Quince.Admin.Core.Managers;
using Quince.Admin.Core.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace Quince.Admin.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index(SearchModel searchModel)
        {
            var result = SearchManager.Search(searchModel);
            var onePageOfResults = result.ToPagedList(searchModel.Page??1, 20); // will only contain 25 products max because of the pageSize

            ViewBag.OnePageOfResults = onePageOfResults;
            return View(searchModel);
        }
    }
}