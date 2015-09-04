using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schools.Models.GoogleApi
{
    public class GeocodeResult
    {
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        public List<string> Types { get; set; }
    }
}