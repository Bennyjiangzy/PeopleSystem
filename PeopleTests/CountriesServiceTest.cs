using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using ServiceContracts;
using Emtities;
using Services;
using NuGet.Frameworks;

namespace PeopleTests
{
    public class CountriesServiceTest
    {
        private readonly ICountryService _countryService;

        public CountriesServiceTest()
        {
            _countryService = new CountriesService();
        }

        #region AddCountry
        //When CountryAddRequest is null
        [Fact]
        public void AddCountry_NullCountry() 
        {
            CountryAddRequest? request = null;

            Assert.Throws<ArgumentNullException>(() => 
            {
                _countryService.AddCountry(request);
            });
         
        }

        //When the CountryName is null

        [Fact]
        public void AddCountry_NullCountryName()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _countryService.AddCountry(request);
            });
        }

        //When the CountryName is duplicate

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _countryService.AddCountry(request1);
                _countryService.AddCountry(request2);
            });
        }

        //When supply proper country name, response should show ID

        [Fact]
        public void AddCountry_CheckExistCountryName()
        {
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "USA"
            };
          
            CountryResponse response = _countryService.AddCountry(request);
            List<CountryResponse> country_fromList = _countryService.GetAllCountries();


            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, country_fromList);
        }

        #endregion

        #region GetAllCountries

        [Fact]
        // When list is empty 
        public void GetALLCountries_EmptyList()
        {
            List<CountryResponse> countryList = _countryService.GetAllCountries();

            Assert.Empty(countryList);
        }

        [Fact]
        // When list is empty 
        public void GetALLCountries_AddFewCountries()
        {
            List<CountryAddRequest> countryList = new List<CountryAddRequest>
            {
                new CountryAddRequest() { CountryName = "USA"},
                new CountryAddRequest() { CountryName = "China"}
            };

            List<CountryResponse> Temp = new List<CountryResponse>();

            foreach (var country in countryList)
            {
                Temp.Add(
                _countryService.AddCountry(country));
            }

            List<CountryResponse> countryResponsesList = _countryService.GetAllCountries();
            foreach (CountryResponse countryresponse in Temp)
            {
                Assert.Contains(countryresponse, countryResponsesList);
            }
        }
        #endregion
    }
}
