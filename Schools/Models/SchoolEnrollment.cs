using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schools.Models
{
    public class SchoolEnrollment
    {
        public string Name { get; set; }

        public string LocationQuery { get; set; }

        public int EnrollmentCount { get; set; }

        public string Category { get; set; }
    }
}