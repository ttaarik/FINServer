using FINServer.Models;
using Microsoft.EntityFrameworkCore;

namespace FINServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Customer> Customers { get; set; }
    }
}
