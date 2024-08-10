using FINServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using FINServer.Models;
using System.Threading.Tasks;


namespace FINServer.Repositories
{
    public class CustomerRepository
    {
        private readonly string _connectionString;
        private readonly DbContext _context;


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


        public async Task<bool> LoginCustomerAsync(Login login)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT COUNT(*) FROM customers WHERE email = @email AND password = @password";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", login.Email);
                    command.Parameters.AddWithValue("@password", login.Password);
                    var result = await command.ExecuteScalarAsync();

                    // Überprüfen, ob ein Benutzer mit den angegebenen Anmeldeinformationen gefunden wurde
                    return Convert.ToInt32(result) > 0;
                }
            }
        }

        //Und das ist auch falsch, TODO: Wie kriege ich mithilfe des Tokens den User und seinen Firstname, Lastname

        //public async Task<Customer> GetCustomerByTokenAsync(string token)
        //{
        //    // Finde den Benutzer anhand des Tokens
        //    var user = await _context.Customers.FirstOrDefaultAsync(c => c.Token == token);
        //    return user;
        //}



        public async Task<Customer> GetCustomerByEmail(string email, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM customers WHERE email = @email";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Kundenobjekt erstellen und Daten aus der Datenbank lesen
                            var storedPasswordHash = reader.GetString(reader.GetOrdinal("password"));

                            return new Customer
                            {
                                CustomerId = reader.GetInt32(reader.GetOrdinal("customer_id")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                                LastName = reader.GetString(reader.GetOrdinal("last_name")),
                                Password = storedPasswordHash,
                                Income = reader.GetDecimal(reader.GetOrdinal("income"))
                                // Weitere Eigenschaften hinzufügen, falls erforderlich
                            };
                            
                        }
                        // Keinen Kunden mit der angegebenen E-Mail oder Passwort gefunden
                        return null;
                    }
                }
            }
        }


    }
}
