using FINServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using FINServer.Models;
using System.Threading.Tasks;

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

        //Eigentlich nicht nötig
        //public async Task<Subscription> GetSubscriptionById(int id)
        //{
        //    using (var connection = new MySqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        var query = "SELECT * FROM `subscriptions` WHERE customer_id = @id";
        //        using (var command = new MySqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@id", id);
        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    // Subscriptionobjek erstellen und Daten aus der Datenbank lesen

        //                    return new Subscription
        //                    {
        //                        SubscriptionId = reader.GetInt32(reader.GetOrdinal("subscription_id")),
        //                        CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
        //                        ServiceName = reader.GetString(reader.GetOrdinal("service_name")),
        //                        MonthlyFee = reader.GetDecimal(reader.GetOrdinal("monthly_fee")),
        //                        StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
        //                        EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
        //                        Active = reader.GetInt32(reader.GetOrdinal("active"))
        //                    };

        //                }
        //                return null;
        //            }
        //        }
        //    }
        //}

    }
}
