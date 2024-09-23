using FINServer.Models;
using FINServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace FINServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SubscriptionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. Alle Abos abrufen
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM subscriptions;";
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

        // 2. Einzelnes Abo nach ID abrufen
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT * FROM subscriptions WHERE subscription_id = @SubscriptionId;";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@SubscriptionId", id);
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return new JsonResult(table);
        }

        // 3. Neues Abo erstellen
        [HttpPost]
        public JsonResult Post(Subscription subscription)
        {
            string query = @"
                INSERT INTO subscriptions (customer_id, service_name, monthly_fee, start_date, end_date, active)
                VALUES (@CustomerId, @ServiceName, @MonthlyFee, @StartDate, @EndDate, @Active);";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CustomerId", subscription.CustomerId);
                    myCommand.Parameters.AddWithValue("@ServiceName", subscription.ServiceName);
                    myCommand.Parameters.AddWithValue("@MonthlyFee", subscription.MonthlyFee);
                    myCommand.Parameters.AddWithValue("@StartDate", subscription.StartDate);
                    myCommand.Parameters.AddWithValue("@EndDate", subscription.EndDate);
                    myCommand.Parameters.AddWithValue("@Active", subscription.Active);

                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Subscription created successfully");
        }

        // 4. Abo aktualisieren
        [HttpPut]
        public JsonResult Put(Subscription subscription)
        {
            string query = @"
                UPDATE subscriptions
                SET customer_id = @CustomerId, service_name = @ServiceName, monthly_fee = @MonthlyFee, 
                start_date = @StartDate, end_date = @EndDate, active = @Active
                WHERE subscription_id = @SubscriptionId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@SubscriptionId", subscription.SubscriptionId);
                    myCommand.Parameters.AddWithValue("@CustomerId", subscription.CustomerId);
                    myCommand.Parameters.AddWithValue("@ServiceName", subscription.ServiceName);
                    myCommand.Parameters.AddWithValue("@MonthlyFee", subscription.MonthlyFee);
                    myCommand.Parameters.AddWithValue("@StartDate", subscription.StartDate);
                    myCommand.Parameters.AddWithValue("@EndDate", subscription.EndDate);
                    myCommand.Parameters.AddWithValue("@Active", subscription.Active);

                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Subscription updated successfully");
        }

        // 5. Abo löschen
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM subscriptions WHERE subscription_id = @SubscriptionId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@SubscriptionId", id);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Subscription deleted successfully");
        }
    }
}
