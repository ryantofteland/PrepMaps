using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper.Configuration;

namespace Schools.Models
{
    public sealed class CsvSchoolMap : CsvClassMap<School>
    {
        public CsvSchoolMap()
        {
            Map(m => m.Latitude).Ignore();
            Map(m => m.Longitude).Ignore();
        }
    }
}