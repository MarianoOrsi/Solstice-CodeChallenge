using Challenge.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Photo> Photos { get; set; }
    }
}