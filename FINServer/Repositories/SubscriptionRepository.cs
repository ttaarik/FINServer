using Microsoft.EntityFrameworkCore;

namespace FINServer.Repositories
{
    public class SubscriptionRepository
    {
        private readonly string _connectionString;
        private readonly DbContext _context;


        public SubscriptionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        
    }
}
