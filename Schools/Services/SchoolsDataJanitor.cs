using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper.Configuration;
using Schools.Models;
using CsvHelper;
using Schools.Models.GoogleApi;

namespace Schools.Services
{
    public interface ISchoolsDataJanitor : ISingletonService
    {
        IList<SchoolEnrollment> LoadSchoolEnrollmentsFromFile();        
        void SaveSchoolsToFile(IList<School> schools);
        IList<School> LoadSchoolsFromFile();
        IList<School> CreateSchools(IList<SchoolEnrollment> schoolEnrollments);
        IList<SchoolCategory> GetSchoolCategories();
    }

    public class SchoolsDataJanitor : ISchoolsDataJanitor
    {
        private readonly IGoogleApiProxy _googleApiProxy;
        private const string _schoolEnrollmentsFileName = "school-enrollments.csv";
        private const string _schoolsFileName = "schoolsgeo.csv";

        public SchoolsDataJanitor(IGoogleApiProxy googleApiProxy)
        {
            _googleApiProxy  =googleApiProxy;
        }

        public IList<SchoolEnrollment> LoadSchoolEnrollmentsFromFile()
        {
            IList<SchoolEnrollment> allSchools = new List<SchoolEnrollment>();

            string inputFilepath = string.Format("{0}/{1}", HttpContext.Current.Server.MapPath("~/App_Data"), _schoolEnrollmentsFileName);            
            var stream = File.OpenRead(inputFilepath);
            using (var csvReader = new CsvReader(new CsvParser(new StreamReader(stream))))
            {
                allSchools = csvReader.GetRecords<SchoolEnrollment>().ToList();
            }

            return allSchools;
        }

        public void SaveSchoolsToFile(IList<School> schools)
        {
            if(schools == null)
            {
                throw new ArgumentNullException("schools");
            }

            string outputFilePath = string.Format("{0}/{1}", HttpContext.Current.Server.MapPath("~/App_Data"), _schoolsFileName);            
            var stream = File.OpenWrite(outputFilePath);
            using (var csvWriter = new CsvWriter(new StreamWriter(stream)))
            {
                csvWriter.WriteRecords(schools);
            }
        }

        public IList<School> LoadSchoolsFromFile()
        {
            IList<School> allSchools = new List<School>();

            string inputFilepath = string.Format("{0}/{1}", HttpContext.Current.Server.MapPath("~/App_Data"), _schoolsFileName);
            var stream = File.OpenRead(inputFilepath);
            using (var csvReader = new CsvReader(new StreamReader(stream)))
            {
                allSchools = csvReader.GetRecords<School>().ToList();
            }

            return allSchools;
        }

        public IList<School> CreateSchools(IList<SchoolEnrollment> schoolEnrollments)
        {
            if(schoolEnrollments == null)
            {
                throw new ArgumentNullException("schoolEnrollments");
            }

            List<School> schools = new List<School>();

            //iteratve all schools and retrieve Lat/Long info from google
            foreach (var schoolEnrollment in schoolEnrollments)
            {
                System.Threading.Thread.Sleep(2200);
                GoogleGeocodeResponse geocodeResponse = _googleApiProxy.GetGeocodeAddressResponse(schoolEnrollment.LocationQuery);
                Location schoolLocation = GetLocationFromGeocodeResponse(geocodeResponse);
                string formattedAddress = GetFormattedAddressFromGeocodeResponse(geocodeResponse);

                var school = new School()
                                 {
                                     Name = schoolEnrollment.Name,
                                     EnrollmentCount = schoolEnrollment.EnrollmentCount,
                                     Category = schoolEnrollment.Category
                                 };

                school.FullAddress = formattedAddress;
                school.Latitude = schoolLocation.Lat;
                school.Longitude = schoolLocation.Lng;

                schools.Add(school);
            }

            return schools;
        }

        public IList<SchoolCategory> GetSchoolCategories()
        {
            List<SchoolCategory> categories = new List<SchoolCategory>()
                                                  {
                                                      new SchoolCategory
                                                          {
                                                              Name = "VeryLarge",
                                                              Description = "Very Large",
                                                              Range = "2000+",
                                                              Color = "blue"
                                                          },
                                                          new SchoolCategory
                                                          {
                                                              Name = "Large",
                                                              Description = "Large",
                                                              Range = "1001-2000",
                                                              Color = "purple"
                                                          },
                                                          new SchoolCategory
                                                          {
                                                              Name = "Medium",
                                                              Description = "Medium",
                                                              Range = "501-1000",
                                                              Color = "red"
                                                          },
                                                          new SchoolCategory
                                                          {
                                                              Name = "Small",
                                                              Description = "Small",
                                                              Range = "201-500",
                                                              Color = "green"
                                                          },
                                                          new SchoolCategory
                                                          {
                                                              Name = "Tiny",
                                                              Description = "Tiny",
                                                              Range = "0-200",
                                                              Color = "yellow"
                                                          },
                                                  };

            return categories;
        }


        private Location GetLocationFromGeocodeResponse(GoogleGeocodeResponse response)
        {
            var location = new Location();
            if(response != null && response.Results != null && response.Results.Count > 0)
            {
                var firstResult = response.Results.First();
                if(firstResult != null && firstResult.Geometry != null)
                {
                    location = firstResult.Geometry.Location;
                }
            }

            return location;
        }

        private string GetFormattedAddressFromGeocodeResponse(GoogleGeocodeResponse response)
        {
            string address = "[unknown address]";
            if (response != null && response.Results != null && response.Results.Count > 0)
            {
                var firstResult = response.Results.First();
                if (firstResult != null && firstResult.FormattedAddress != null)
                {
                    address = firstResult.FormattedAddress;
                }
            }

            return address;
        }
    }
}