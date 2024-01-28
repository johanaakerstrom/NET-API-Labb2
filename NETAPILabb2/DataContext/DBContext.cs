using Microsoft.EntityFrameworkCore;
using NETAPILabb2.Models;

namespace NETAPILabb2.DataContext
{
    public class DBContext : DbContext
    {
        public DbSet<Players> players { get; set; }
        public DbSet<Teams> teams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");
        }
    }

}
