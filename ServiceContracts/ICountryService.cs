using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        // <summary>
        // Add a country object to the list of the countries
        // <return>
        // Return the country object after add to the list
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        // <summary>
        // Returns all countries from the list
        // <return>
        // All countries from the list
        public List<CountryResponse> GetAllCountries();
    }
}