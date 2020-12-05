using Microsoft.EntityFrameworkCore;
using Customers.Models;

namespace Customers.Data
{
    public class CustomersContext : DbContext
    {
        public CustomersContext(DbContextOptions<CustomersContext> options) : base(options)
        {
        }

        public DbSet<UserSys> UserSys {get; set;}
        public DbSet<UserRole> UserRole {get; set;}
        public DbSet<Customer> Customer {get; set;}
        public DbSet<City> City {get; set;}
        public DbSet<Classification> Classification {get; set;}
        public DbSet<Gender> Gender {get; set;}
        public DbSet<Region> Region {get; set;}
    }
}