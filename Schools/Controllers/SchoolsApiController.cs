using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Schools.Models;
using Schools.Models.GoogleApi;
using Schools.Services;

namespace Schools.Controllers
{
    public class SchoolsApiController : Controller
    {
        private readonly ISchoolsDataJanitor _schoolsDataJanitor;

        public SchoolsApiController(ISchoolsDataJanitor schoolsDataJanitor)
        {
            _schoolsDataJanitor = schoolsDataJanitor;
        }

        [HttpPost]
        public ActionResult LoadSchoolLocationData()
        {
            //parse school-enrollments.csv file (does not include location data)
            IList<SchoolEnrollment> schoolEnrollments = _schoolsDataJanitor.LoadSchoolEnrollmentsFromFile();

            //build each school with location info
            var schools = _schoolsDataJanitor.CreateSchools(schoolEnrollments);

            //save to file
            _schoolsDataJanitor.SaveSchoolsToFile(schools);

            return Json(schools, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SchoolEnrollments()
        {
            IList<SchoolEnrollment> allEnrollments = _schoolsDataJanitor.LoadSchoolEnrollmentsFromFile();
            return Json(allEnrollments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Schools()
        {
            IList<School> allSchools = _schoolsDataJanitor.LoadSchoolsFromFile();
            return Json(allSchools, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategorizedSchools()
        {
            IList<School> allSchools = _schoolsDataJanitor.LoadSchoolsFromFile();
            ILookup<string, School> schoolCategories = allSchools.ToLookup(x => x.Category, y => y);
            Dictionary<string, IEnumerable<School>> schoolCategoryDictionary = schoolCategories.ToDictionary(x => x.Key, y => y.Where(x => x.Category == y.Key));

            return Json(schoolCategoryDictionary, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult SchoolCategories()
        {
            IList<SchoolCategory> categories = _schoolsDataJanitor.GetSchoolCategories();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

    }
}
