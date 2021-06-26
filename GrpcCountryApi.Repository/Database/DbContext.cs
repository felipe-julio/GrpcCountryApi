using GrpcCountryApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
