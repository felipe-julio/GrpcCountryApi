using GrpcCountryApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrpcCountryApi.Repository.Database
{
    public class CountryDbContext : DbContext
    {
        public CountryDbContext(DbContextOptions<CountryDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Country { get; set; }

    }
}
