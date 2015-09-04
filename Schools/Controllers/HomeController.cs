using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schools.Services;

namespace Schools.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISchoolsDataJanitor _schoolDataJanitor;

        public HomeController(ISchoolsDataJanitor schoolDataJanitor)
        {
            _schoolDataJanitor = schoolDataJanitor;
        }

        public ActionResult Index()
        {
            var model = _schoolDataJanitor.GetSchoolCategories();
            return View(model);
        }
    }
}
