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
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CustomerRepository _customerRepository;

        public CustomerController(IConfiguration configuration, CustomerRepository customerRepository)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
        }

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

            // Hinzufügen von Standardwerten für leere Felder, um sicherzustellen, dass alle Daten vorhanden sind
            foreach (DataRow row in table.Rows)
            {
                if (string.IsNullOrEmpty(row["first_name"].ToString()))
                    row["first_name"] = "Unknown";

                if (string.IsNullOrEmpty(row["last_name"].ToString()))
                    row["last_name"] = "Unknown";
            }

            return new JsonResult(table);
        }

        //[HttpGet("current")]
        //public async Task<IActionResult> GetCurrentCustomer()
        //{
        //    var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //    if (token == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var user = await _customerRepository.GetCustomerByTokenAsync(token);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(user);
        //}
    }
}
