using GrpcCountryApi.Domain.Entities;
using GrpcCountryApi.Repository.Interfaces;
using GrpcCountryApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcCountryApi.Services
{
    public class CountryService : ICountryService
    {
        public ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public Task<List<Country>> GetAsync()
        {
            //throw new Exception("error in the service");
            return _countryRepository.GetAsync();
        }

        public Task<Country> GetByIdAsync(int countryId)
        {
            return _countryRepository.GetByIdAsync(countryId);
        }

        public async Task<Country> AddAsync(Country country)
        {
            return await _countryRepository.AddAsync(country);
        }

        public async Task<Country> UpdateAsync(Country country)
        {
            var result = await _countryRepository.UpdateAsync(country);
            if (result > 0)
                return country;

            throw new Exception("Update failed");
        }

        public async Task<bool> DeleteAsync(int countryId)
        {
            var result = await _countryRepository.DeleteAsync(countryId);
            if (result > 0)
                return true;

            throw new Exception("Delete failed");
        }
    }
}
