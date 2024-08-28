using FINServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;

namespace FINServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SubscriptionRepository _subscriptionRepository;

        public SubscriptionController(IConfiguration configuration, SubscriptionRepository subscriptionRepository)
        {
            _configuration = configuration;
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM `subscriptions`;";
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
            string query = @"SELECT * FROM `subscriptions` WHERE customer_id = " + id + ";";
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
