using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schools.Models.GoogleApi
{
    public class GoogleGeocodeResponse
    {
        public List<GeocodeResult> Results { get; set; }
        public string Status { get; set; }
    }
}