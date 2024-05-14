using FINServer.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;


namespace FINServer.Repositories
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task AddCustomerAsync(Register Register)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO customers (email, password) VALUES (@email, @password)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", Register.Email);
                    command.Parameters.AddWithValue("@password", Register.Password);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
