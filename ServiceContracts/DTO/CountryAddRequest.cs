﻿using System;
using System.Collections.Generic;
using Emtities;

namespace ServiceContracts.DTO
{
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country ToCountry() 
        {
            return new Country() { CountryName = CountryName };
        }
    }

}
