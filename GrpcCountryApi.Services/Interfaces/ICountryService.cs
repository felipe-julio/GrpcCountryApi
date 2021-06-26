using GrpcCountryApi.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrpcCountryApi.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetAsync();
        Task<Country> GetByIdAsync(int countryId);
        Task<Country> AddAsync(Country country);
        Task<Country> UpdateAsync(Country country);
        Task<bool> DeleteAsync(int countryId);
    }
}
