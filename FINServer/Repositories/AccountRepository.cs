using FINServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using FINServer.Models;
using System.Threading.Tasks;

namespace FINServer.Repositories
{
    public class AccountRepository
    {
        private readonly string _connectionString;
        private readonly DbContext _context;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Account> GetAccountById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM accounts WHERE customer_id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Account
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                AccountType = Enum.TryParse<AccountType>(reader.GetString(reader.GetOrdinal("account_type")), out var accountType) ? accountType : throw new InvalidCastException(),
                                Balance = reader.GetDecimal(reader.GetOrdinal("balance")),
                            };

                        }
                        // Keinen Account mit der angegebenen ID gefunden
                        return null;
                    }
                }
            }
        }
    }
}
