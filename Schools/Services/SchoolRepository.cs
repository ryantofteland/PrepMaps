using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Schools.Models;

namespace Schools.Services
{
    public interface ISchoolRepository : ISingletonService
    {
        IList<School> GetAllSchools();
    }

    public class SchoolRepository : ISchoolRepository
    {
        public SchoolRepository()
        {}

        public IList<School> GetAllSchools()
        {
            return null;
        }
    }
}