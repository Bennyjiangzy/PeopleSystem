using ServiceContracts;
using ServiceContracts.DTO;
using Emtities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountryService _countriesService;
        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }
        private PersonResponse ConvertPersonToPersonResponse(Person person) 
        {
            PersonResponse personresponse = person.ToPersonResponse();
            personresponse.Country = 
                _countriesService.GetCountryByCountryID(personresponse.CountryID)?.CountryName;
            return personresponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null) 
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();

            //add guid id 
            person.PersonID = Guid.NewGuid();

            //add person object to the list
            _persons.Add(person);

            // fill the country name by countryID in the detail
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) 
            {
                return null;
            }

            Person? personResponse = 
                _persons.FirstOrDefault((temp)=> temp.PersonID == personID);
            if (personResponse == null) 
            {
                return null;
            }
            return personResponse.ToPersonResponse();
        }
    }
}
