using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using Schools.Models;
using Schools.Models.GoogleApi;

namespace Schools.Services
{
    public interface IGoogleApiProxy : ISingletonService
    {
        GoogleGeocodeResponse GetGeocodeAddressResponse(string addressQuery);
    }

    public class GoogleApiProxy : IGoogleApiProxy
    {
        private const string _baseUrl = "http://maps.googleapis.com/maps/api/geocode/json";
        private const string _addressFormat = "{0}, MN";
        private readonly RestClient _restClient;

        public GoogleApiProxy()
        {
            _restClient = new RestClient(_baseUrl);            
        }

        public GoogleGeocodeResponse GetGeocodeAddressResponse(string addressQuery)
        {
            if(string.IsNullOrEmpty(addressQuery))
            {
                throw new ArgumentException("addressQuery");
            }

            string addressParam = string.Format(_addressFormat, addressQuery);

            var restRequest = new RestRequest(Method.GET);
            restRequest.AddParameter("address", addressParam);
            restRequest.AddParameter("sensor", "true");

            var response = _restClient.Execute<GoogleGeocodeResponse>(restRequest);

            return response.Data;
        }
    }
}