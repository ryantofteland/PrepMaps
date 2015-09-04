using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schools.Models
{
    public class School
    {
        public string Name { get; set; }

        public int EnrollmentCount { get; set; }

        public string Category { get; set; }

        public string FullAddress { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}