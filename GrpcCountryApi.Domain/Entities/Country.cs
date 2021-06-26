using System;

namespace GrpcCountryApi.Domain
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Description { get; set; }
    }
}
