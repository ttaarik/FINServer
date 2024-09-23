using FINServer.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace FINServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. Alle Kunden abrufen
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM `customers`;";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult(table);
        }

        // 2. Einzelnen Kunden anhand der ID abrufen
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT * FROM `customers` WHERE customer_id = @CustomerId;";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CustomerId", id);
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }

            return new JsonResult(table);
        }

        // 3. Neuen Kunden hinzufügen
        [HttpPost]
        public JsonResult Post(Customer customer)
        {
            string query = @"
                INSERT INTO customers (email, password, first_name, last_name, street, plz, city, income)
                VALUES (@Email, @Password, @FirstName, @LastName, @Street, @PLZ, @City, @Income);";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Email", customer.Email);
                    myCommand.Parameters.AddWithValue("@Password", customer.Password);
                    myCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                    myCommand.Parameters.AddWithValue("@Street", customer.Street);
                    myCommand.Parameters.AddWithValue("@PLZ", customer.PLZ);
                    myCommand.Parameters.AddWithValue("@City", customer.City);
                    myCommand.Parameters.AddWithValue("@Income", customer.Income);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Customer created successfully");
        }

        // 4. Bestehenden Kunden aktualisieren
        [HttpPut]
        public JsonResult Put(Customer customer)
        {
            string query = @"
                UPDATE customers
                SET email = @Email, password = @Password, first_name = @FirstName, last_name = @LastName,
                    street = @Street, plz = @PLZ, city = @City, income = @Income
                WHERE customer_id = @CustomerId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
                    myCommand.Parameters.AddWithValue("@Email", customer.Email);
                    myCommand.Parameters.AddWithValue("@Password", customer.Password);
                    myCommand.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    myCommand.Parameters.AddWithValue("@LastName", customer.LastName);
                    myCommand.Parameters.AddWithValue("@Street", customer.Street);
                    myCommand.Parameters.AddWithValue("@PLZ", customer.PLZ);
                    myCommand.Parameters.AddWithValue("@City", customer.City);
                    myCommand.Parameters.AddWithValue("@Income", customer.Income);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Customer updated successfully");
        }

        // 5. Kunden löschen
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM customers WHERE customer_id = @CustomerId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CustomerId", id);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Customer deleted successfully");
        }
    }
}
