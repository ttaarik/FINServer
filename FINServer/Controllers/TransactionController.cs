using FINServer.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace FINServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TransactionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. Alle Transaktionen abrufen (GET)
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM `transactions`;";
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

        // 2. Einzelne Transaktion nach ID abrufen (GET)
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = $@"
                SELECT t.transaction_id, t.sender_account_id, t.receiver_account_id, t.amount, t.transaction_type, t.timestamp, t.description,
                a1.customer_id AS sender_customer_id, a1.account_type AS sender_account_type,
                a2.customer_id AS receiver_customer_id, a2.account_type AS receiver_account_type,
                c1.first_name AS sender_first_name, c1.last_name AS sender_last_name, c1.email AS sender_email,
                c2.first_name AS receiver_first_name, c2.last_name AS receiver_last_name, c2.email AS receiver_email
                FROM transactions t
                JOIN accounts a1 ON t.sender_account_id = a1.account_id
                LEFT JOIN accounts a2 ON t.receiver_account_id = a2.account_id
                LEFT JOIN customers c1 ON a1.customer_id = c1.customer_id
                LEFT JOIN customers c2 ON a2.customer_id = c2.customer_id
                WHERE a1.customer_id = {id} OR a2.customer_id = {id};";

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

        // 3. Neue Transaktion erstellen (POST)
        [HttpPost]
        public JsonResult Post(Transaction transaction)
        {
            string query = @"
                INSERT INTO transactions (sender_account_id, receiver_account_id, amount, transaction_type, timestamp, description)
                VALUES (@SenderAccountID, @ReceiverAccountID, @Amount, @TransactionType, @TimeStamp, @Description);";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@SenderAccountID", transaction.SenderAccountID);
                    myCommand.Parameters.AddWithValue("@ReceiverAccountID", transaction.ReceiverAccountID);
                    myCommand.Parameters.AddWithValue("@Amount", transaction.Amount);
                    myCommand.Parameters.AddWithValue("@TransactionType", transaction.TransactionType.ToString());
                    myCommand.Parameters.AddWithValue("@TimeStamp", transaction.TimeStamp);
                    myCommand.Parameters.AddWithValue("@Description", transaction.Description);

                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Transaction created successfully");
        }

        // 4. Transaktion aktualisieren (PUT)
        [HttpPut]
        public JsonResult Put(Transaction transaction)
        {
            string query = @"
                UPDATE transactions
                SET sender_account_id = @SenderAccountID, receiver_account_id = @ReceiverAccountID, 
                amount = @Amount, transaction_type = @TransactionType, timestamp = @TimeStamp, description = @Description
                WHERE transaction_id = @TransactionID;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
                    myCommand.Parameters.AddWithValue("@SenderAccountID", transaction.SenderAccountID);
                    myCommand.Parameters.AddWithValue("@ReceiverAccountID", transaction.ReceiverAccountID);
                    myCommand.Parameters.AddWithValue("@Amount", transaction.Amount);
                    myCommand.Parameters.AddWithValue("@TransactionType", transaction.TransactionType.ToString());
                    myCommand.Parameters.AddWithValue("@TimeStamp", transaction.TimeStamp);
                    myCommand.Parameters.AddWithValue("@Description", transaction.Description);

                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Transaction updated successfully");
        }

        // 5. Transaktion löschen (DELETE)
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM transactions WHERE transaction_id = @TransactionID;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TransactionID", id);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Transaction deleted successfully");
        }
    }
}
