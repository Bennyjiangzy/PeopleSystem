using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using ServiceContracts;
using Emtities;
using Services;
using NuGet.Frameworks;
using ServiceContracts.Enums;
using Xunit.Abstractions;

namespace PeopleTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService();
            _countryService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        [Fact]
        // When null value given should return ArgumentNullException
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? request = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.AddPerson(request);
            });
        }

        [Fact]
        // When PersonName is null value should return ArgumentException
        public void AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? request = new PersonAddRequest
            {
                PersonName = null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(request);
            });
        }

        [Fact]
        // When it given a proper person details, it should insert into the person list
        // return the personresponse object
        public void AddPerson_Person()
        {
            PersonAddRequest? request = new PersonAddRequest
            {
                PersonName = "Sample person",
                Email = "abc@example.com",
                Address = "Sample",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true

            };


            PersonResponse person_from_add = _personsService.AddPerson(request);
            List<PersonResponse> person_from_list = _personsService.GetAllPersons();

            Assert.True(person_from_add.PersonID != Guid.Empty);
            Assert.Contains(person_from_add, person_from_list);


        }
        #endregion

        #region GetPersonByPersonID

        [Fact]
        // when PersonID is null it should return null value
        public void GetPersonByPersonID_NullPersonID()
        {
            Guid? personID = null;

            PersonResponse? personresponse = _personsService.GetPersonByPersonID(personID);

            Assert.Null(personID);
        }

        [Fact]
        // when valid PersonID supply, it should return a PersonResponse with matched ID
        public void GetPersonByPersonID_PersonID()
        {
            CountryAddRequest country_request = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryResponse country_response =
                _countryService.AddCountry(country_request);

            PersonAddRequest request = new PersonAddRequest
            {
                PersonName = "sample",
                Address = "Sample",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello@eampl.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = country_response.CountryID
            };

            PersonResponse person_request = 
                _personsService.AddPerson(request);

            PersonResponse? person_getbyid = 
                _personsService.GetPersonByPersonID(person_request.PersonID);

            Assert.Equal(person_request, person_getbyid);
        }
        #endregion

        #region GetAllPersons
        [Fact]
        // The GetAllPersons() should return an empty list by default
        public void GetAllPersons_EmptyList() 
        {
            List<PersonResponse> personResponses = 
                _personsService.GetAllPersons();

            Assert.Empty(personResponses);
        }

        [Fact]
        // After called personaddrequest and getall method,
        // it should return a list of personresponse obejct
        // with correct details.
        public void GetAllPersons_List() 
        {
            CountryAddRequest country_request1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryAddRequest country_request2 = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryResponse country_response1 =
                _countryService.AddCountry(country_request1);
            CountryResponse country_response2 =
                _countryService.AddCountry(country_request2);

            PersonAddRequest request1 = new PersonAddRequest
            {
                PersonName = "sample1",
                Address = "Sample1",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello1@eampl.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = country_response1.CountryID
            };

            PersonAddRequest request2 = new PersonAddRequest
            {
                PersonName = "sample2",
                Address = "Sample2",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello2@eampl.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                CountryID = country_response2.CountryID
            };
            List<PersonAddRequest> person_to_add = new List<PersonAddRequest>()
            {
                request1,
                request2,
            };

            List<PersonResponse> person_after_add = new List<PersonResponse>(){};
            foreach (PersonAddRequest request in person_to_add) 
            {
                PersonResponse personResponse = _personsService.AddPerson(request);
                person_after_add.Add(personResponse);
            }

            //print the test case
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in person_after_add) 
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }

            //Act
            List<PersonResponse> personResponseList = _personsService.GetAllPersons();

            //print the test case
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personResponseList)
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }

            foreach (PersonResponse person in person_after_add)
            {
                Assert.Contains(person, personResponseList);
            }
        }
        #endregion

        #region GetFilteredPersons
        //If the search text is empty and serach by is "PersonName", it should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest country_request1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryAddRequest country_request2 = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryResponse country_response1 =
                _countryService.AddCountry(country_request1);
            CountryResponse country_response2 =
                _countryService.AddCountry(country_request2);

            PersonAddRequest request1 = new PersonAddRequest
            {
                PersonName = "sample1",
                Address = "Sample1",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello1@eampl.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = country_response1.CountryID
            };

            PersonAddRequest request2 = new PersonAddRequest
            {
                PersonName = "sample2",
                Address = "Sample2",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello2@eampl.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                CountryID = country_response2.CountryID
            };
            List<PersonAddRequest> person_to_add = new List<PersonAddRequest>()
            {
                request1,
                request2,
            };

            List<PersonResponse> person_after_add = new List<PersonResponse>() { };
            foreach (PersonAddRequest request in person_to_add)
            {
                PersonResponse personResponse = _personsService.AddPerson(request);
                person_after_add.Add(personResponse);
            }

            //print the test case
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in person_after_add)
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }

            //Act
            List<PersonResponse> personResponseList = 
                _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print the test case
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personResponseList)
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }

            foreach (PersonResponse person in person_after_add)
            {
                Assert.Contains(person, personResponseList);
            }
        }

        [Fact]
        // add with few persons; and then search based on person name with some search string
        // it should retrun the matching persons
        public void GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest country_request1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryAddRequest country_request2 = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryResponse country_response1 =
                _countryService.AddCountry(country_request1);
            CountryResponse country_response2 =
                _countryService.AddCountry(country_request2);

            PersonAddRequest request1 = new PersonAddRequest
            {
                PersonName = "samTeple1",
                Address = "Sample1",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello1@eampl.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = country_response1.CountryID
            };

            PersonAddRequest request2 = new PersonAddRequest
            {
                PersonName = "test",
                Address = "Sample2",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello2@eampl.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                CountryID = country_response2.CountryID
            };
            List<PersonAddRequest> person_to_add = new List<PersonAddRequest>()
            {
                request1,
                request2,
            };

            List<PersonResponse> person_after_add = new List<PersonResponse>() { };
            foreach (PersonAddRequest request in person_to_add)
            {
                PersonResponse personResponse = _personsService.AddPerson(request);
                person_after_add.Add(personResponse);
            }

            //print the test case
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in person_after_add)
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }

            //Act
            List<PersonResponse> personResponseList =
                _personsService.GetFilteredPersons(nameof(Person.PersonName), "te");

            //print the test case
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personResponseList)
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }

            foreach (PersonResponse person in person_after_add)
            {
                if (person.PersonName != null) 
                {
                    if (person.PersonName.
                        Contains("te", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person, personResponseList);
                    }
                }

            }
        }

        #endregion

        #region GetSortedPersons
        [Fact]
        // When we sort based on PersonName in DESC, it should return persons list with DESC order
        public void GetSortedPersons()
        {
            CountryAddRequest country_request1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryAddRequest country_request2 = new CountryAddRequest()
            {
                CountryName = "China"
            };
            CountryAddRequest country_request3 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryResponse country_response1 =
                _countryService.AddCountry(country_request1);
            CountryResponse country_response2 =
                _countryService.AddCountry(country_request2);
            CountryResponse country_response3 =
                _countryService.AddCountry(country_request3);

            PersonAddRequest request1 = new PersonAddRequest
            {
                PersonName = "samTeple1",
                Address = "Sample1",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello1@eampl.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = country_response1.CountryID
            };

            PersonAddRequest request2 = new PersonAddRequest
            {
                PersonName = "test",
                Address = "Sample2",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello2@eampl.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                CountryID = country_response2.CountryID
            };
            PersonAddRequest request3 = new PersonAddRequest
            {
                PersonName = "test3",
                Address = "Sample3",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello3@eampl.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
                CountryID = country_response3.CountryID
            };
            List<PersonAddRequest> person_to_add = new List<PersonAddRequest>()
            {
                request1,
                request2,
                request3,
            };

            List<PersonResponse> person_after_add = new List<PersonResponse>() { };
            foreach (PersonAddRequest request in person_to_add)
            {
                PersonResponse personResponse = _personsService.AddPerson(request);
                person_after_add.Add(personResponse);
            }

            //print the test case
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person in person_after_add)
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }

            //Act
            List<PersonResponse> allPersons = _personsService.GetAllPersons();
            List<PersonResponse> personResponseList =
                _personsService.GetSortedPersons
                (allPersons,nameof(Person.PersonName), SortOrderOptions.DESC);

            //print the test case
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person in personResponseList)
            {
                _testOutputHelper.WriteLine
                    (person.ToString());
            }
            person_after_add = person_after_add.OrderByDescending(temp =>
            temp.PersonName).ToList();

            //Assert
            //Test the sorted method is equal to the sorted list here
            for (int i = 0; i < person_after_add.Count; i++) 
            {
                Assert.Equal(person_after_add[i], personResponseList[i]);
            }
        }
        #endregion

        #region UpdatePerson
        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPerson() 
        {
            PersonUpdateRequest? person_update_request = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.UpdatePerson(person_update_request);
            });
            
        }

        //When we supply invalid person ID, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            PersonUpdateRequest? person_update_request = new PersonUpdateRequest() 
            {
                PersonID = Guid.NewGuid(),
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(person_update_request);
            });

        }

        // When PersonName is null, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            CountryAddRequest country_request1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryResponse country_response1 =
                _countryService.AddCountry(country_request1);
            PersonAddRequest request1 = new PersonAddRequest
            {
                PersonName = "sample1",
                Address = "Sample1",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello1@eampl.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = country_response1.CountryID
            };
            List<PersonAddRequest> person_to_add = new List<PersonAddRequest>()
            {
                request1
            };

            List<PersonResponse> person_after_add = new List<PersonResponse>() { };
            foreach (PersonAddRequest request in person_to_add)
            {
                PersonResponse personResponse = _personsService.AddPerson(request);
                person_after_add.Add(personResponse);
            }

            PersonUpdateRequest? person_update_request = person_after_add[0].ToPersonUpdateRequest();
            person_update_request.PersonName = null;


            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(person_update_request);
            });

        }

        // When update person details is good, it should return a updated personresponse
        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdation()
        {
            CountryAddRequest country_request1 = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryResponse country_response1 =
                _countryService.AddCountry(country_request1);
            PersonAddRequest request1 = new PersonAddRequest
            {
                PersonName = "sample1",
                Address = "Sample1",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email = "hello1@eampl.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true,
                CountryID = country_response1.CountryID
            };
            List<PersonAddRequest> person_to_add = new List<PersonAddRequest>()
            {
                request1
            };

            List<PersonResponse> person_after_add = new List<PersonResponse>() { };
            foreach (PersonAddRequest request in person_to_add)
            {
                PersonResponse personResponse = _personsService.AddPerson(request);
                person_after_add.Add(personResponse);
            }

            PersonUpdateRequest? person_update_request = person_after_add[0].ToPersonUpdateRequest();
            person_update_request.PersonName = "Update";
            person_update_request.Email = "Update.@update.com";

            //print the test case
            _testOutputHelper.WriteLine("Expected:");
         
            _testOutputHelper.WriteLine(person_after_add[0].ToString());
            
            //Act
            PersonResponse update_response = _personsService.UpdatePerson(person_update_request);
            PersonResponse? person_from_get = _personsService.GetPersonByPersonID(update_response.PersonID);
            _testOutputHelper.WriteLine("Actual:");
            _testOutputHelper.WriteLine(person_from_get!.ToString());

            Assert.Equal(person_from_get, update_response);

        }
        #endregion
    }
}
