using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        /// <summary>
        /// Add a country object to the list of the countries</summary>
        /// <param name="countryAddRequest">CountryAddRequest object to add</param>
        /// <return>
        /// Return the country object after add to the list</return>
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries from the list</summary>
        /// <return>
        /// All countries from the list</return>
        public List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Returns countries by CountryID</summary>
        /// <param name = "countryID">searched Guid CountryID</param>
        /// <return>
        /// Return a CountryResponse object by parameter guid countryID</return>
        public CountryResponse? GetCountryByCountryID(Guid? countryID);
    }
}