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
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AccountRepository _accountRepository;

        public AccountController(IConfiguration configuration, AccountRepository accountRepository)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
        }

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

        [HttpPost]
        [Route("/id")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT * FROM `accounts` WHERE customer_id = " + id + ";";
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


        //[HttpPost]
        //[Route("/acc")]
        //public async Task<IActionResult> Account([FromBody] int id)
        //{
        //    try
        //    {
        //        await _accountRepository.GetAccountById(id);
        //        return Ok("nice");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error registering user: {ex.Message}");
        //    }
        //}
    }
}
