using FINServer.Data;
using FINServer.Models;
using FINServer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        [Route("id")]

        public JsonResult Get(int id)
        {
            string query = $@"SELECT t.transaction_id, t.sender_account_id, t.receiver_account_id, t.amount, t.transaction_type, t.timestamp, t.description,
                a1.customer_id AS sender_customer_id, a1.account_type AS sender_account_type,
                a2.customer_id AS receiver_customer_id, a2.account_type AS receiver_account_type,
                c1.first_name AS sender_first_name, c1.last_name AS sender_last_name, c1.email AS sender_email,
                c2.first_name AS receiver_first_name, c2.last_name AS receiver_last_name, c2.email AS receiver_email
                FROM transactions t
                JOIN accounts a1 ON t.sender_account_id = a1.account_id
                LEFT JOIN accounts a2 ON t.receiver_account_id = a2.account_id
                LEFT JOIN customers c1 ON a1.customer_id = c1.customer_id
                LEFT JOIN customers c2 ON a2.customer_id = c2.customer_id
                WHERE a1.customer_id = {id} OR a2.customer_id = {id};
            ";

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

    }
}
