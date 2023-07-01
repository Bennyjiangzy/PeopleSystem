using ServiceContracts;
using ServiceContracts.DTO;
using Emtities;
using System.Collections.Generic;

namespace Services
{
    public class CountriesService : ICountryService
    {
        private readonly List<Country> _countries;

        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null) 
            {
                throw new ArgumentNullException(nameof(countryAddRequest));           
            }

            Country country = countryAddRequest.ToCountry();

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (_countries.Where(temp => 
                temp.CountryName == country.CountryName).Count() > 0) 
            {
                throw new ArgumentException("The given country name already exists");
            }

            country.CountryID = Guid.NewGuid();
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }
    }
}