using ServiceContracts;
using ServiceContracts.DTO;
using Emtities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using ServiceContracts.Enums;

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

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();

            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) 
            {
                return matchingPersons;
            }

            switch (searchBy) 
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                    (string.IsNullOrEmpty(temp.PersonName) ?
                    temp.PersonName!.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp =>
                    (string.IsNullOrEmpty(temp.Email) ?
                    temp.Email!.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(temp =>
                    (string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender!.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(temp =>
                    (string.IsNullOrEmpty(temp.Country) ?
                    temp.Country!.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(temp =>
                    (string.IsNullOrEmpty(temp.Address) ?
                    temp.Address!.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) 
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName),SortOrderOptions.ASC)
                => allPersons.OrderBy(temp => temp.PersonName,
                   StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(temp => temp.PersonName,
                   StringComparer.OrdinalIgnoreCase).ToList(),
                //ignore the other attributes for now
                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) 
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);

            if (matchingPerson == null) 
            {
                throw new ArgumentException("Given persin id doesn't exist");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address  = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }
    }
}
