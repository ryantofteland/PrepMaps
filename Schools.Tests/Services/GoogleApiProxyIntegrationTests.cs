using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Schools.Models;
using Schools.Models.GoogleApi;
using Schools.Services;

namespace Schools.Tests.Services
{
    [TestClass]
    public class GoogleApiProxyIntegrationTests
    {
        [TestMethod]
        public void Retrieve_GoogleGeocodeResponse_from_Service()
        {
            var googleApiProxy = new GoogleApiProxy();
            GoogleGeocodeResponse response = googleApiProxy.GetGeocodeAddressResponse("luverne high school");

            Assert.AreEqual("OK", response.Status);
        }
    }
}
