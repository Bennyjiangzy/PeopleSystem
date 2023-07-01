using System;
using System.Collections.Generic;
using Emtities;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string? CountryName { get; set; }


        // It compares the currrent object to another object of CountryResponse type
        // return true if all fields are same
        // The reason need to override it since the default equals will only compare
        // the reference address rather than the actual value.
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is CountryResponse other)) return false;

            CountryResponse country_to_compare = (CountryResponse) obj;
            return CountryID == country_to_compare.CountryID &&
                CountryName == country_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country) 
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }
}
