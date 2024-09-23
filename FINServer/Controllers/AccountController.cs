using FINServer.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace FINServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. Alle Konten abrufen
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM `accounts`;";
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

        // 2. Konten für einen bestimmten Kunden abrufen
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT * FROM `accounts` WHERE customer_id = @CustomerId;";
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

        // 3. Neues Konto hinzufügen
        [HttpPost]
        public JsonResult Post(Account account)
        {
            string query = @"
                INSERT INTO accounts (customer_id, account_type, balance)
                VALUES (@CustomerId, @AccountType, @Balance);";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CustomerId", account.CustomerId);
                    myCommand.Parameters.AddWithValue("@AccountType", account.AccountType.ToString());
                    myCommand.Parameters.AddWithValue("@Balance", account.Balance);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Account added successfully");
        }

        // 4. Bestehendes Konto aktualisieren
        [HttpPut]
        public JsonResult Put(Account account)
        {
            string query = @"
                UPDATE accounts
                SET customer_id = @CustomerId, account_type = @AccountType, balance = @Balance
                WHERE account_id = @AccountId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@AccountId", account.AccountId);
                    myCommand.Parameters.AddWithValue("@CustomerId", account.CustomerId);
                    myCommand.Parameters.AddWithValue("@AccountType", account.AccountType.ToString());
                    myCommand.Parameters.AddWithValue("@Balance", account.Balance);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Account updated successfully");
        }

        // 5. Konto löschen
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM accounts WHERE account_id = @AccountId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@AccountId", id);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Account deleted successfully");
        }
    }
}
