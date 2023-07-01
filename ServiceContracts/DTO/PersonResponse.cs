using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Emtities;

namespace ServiceContracts.DTO
{

    // <summary>
    // represents DTO class that is used as return type of most methods of Persons service
    // <return>

    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Eamil { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        // <summary>
        // compare the details with in PersonResponse type of objects
        // <return>
        // return true of false based on the matched details
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is PersonResponse other)) return false;

            PersonResponse person = (PersonResponse)obj;

            return PersonID == person.PersonID &&
                   PersonName == person.PersonName &&
                   Eamil == person.Eamil &&
                   DateOfBirth == person.DateOfBirth &&
                   Gender == person.Gender &&
                   CountryID == person.CountryID &&
                   Address == person.Address &&
                   ReceiveNewsLetters == person.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    // <summary>
    // convert Person type of object to the PersonResponse object
    // <return>
    // return the PersonResponse type of object with details from current Person object
    public static class PersonExtensions 
    {
        public static PersonResponse ToPersonResponse(this Person person) 
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Eamil = person.Eamil,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                CountryID = person.CountryID,
                Address = person.Address,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null)
                ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25)
                : null,
            };
        }
    }
}
