using FINServer.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace FINServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupportTicketController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SupportTicketController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. Alle Support-Tickets abrufen
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM support_tickets;";
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

        // 2. Einzelnes Support-Ticket nach ID abrufen
        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            string query = $@"SELECT * FROM support_tickets WHERE ticket_id = {id};";

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

        // 3. Neues Support-Ticket erstellen
        [HttpPost]
        public JsonResult Post(SupportTicket ticket)
        {
            string query = $@"
                INSERT INTO support_tickets (customer_id, subject, description, status, created_at, updated_at)
                VALUES (@CustomerId, @Subject, @Description, @Status, @CreatedAt, @UpdatedAt);";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CustomerId", ticket.CustomerId);
                    myCommand.Parameters.AddWithValue("@Subject", ticket.Subject);
                    myCommand.Parameters.AddWithValue("@Description", ticket.Description);
                    myCommand.Parameters.AddWithValue("@Status", ticket.Status);
                    myCommand.Parameters.AddWithValue("@CreatedAt", ticket.CreatedAt);
                    myCommand.Parameters.AddWithValue("@UpdatedAt", ticket.UpdatedAt);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Ticket created successfully");
        }

        // 4. Support-Ticket aktualisieren
        [HttpPut]
        public JsonResult Put(SupportTicket ticket)
        {
            string query = $@"
                UPDATE support_tickets 
                SET customer_id = @CustomerId, subject = @Subject, description = @Description, 
                status = @Status, updated_at = @UpdatedAt 
                WHERE ticket_id = @TicketId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@TicketId", ticket.TicketId);
                    myCommand.Parameters.AddWithValue("@CustomerId", ticket.CustomerId);
                    myCommand.Parameters.AddWithValue("@Subject", ticket.Subject);
                    myCommand.Parameters.AddWithValue("@Description", ticket.Description);
                    myCommand.Parameters.AddWithValue("@Status", ticket.Status);
                    myCommand.Parameters.AddWithValue("@UpdatedAt", ticket.UpdatedAt);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Ticket updated successfully");
        }

        // 5. Support-Ticket löschen
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = $@"DELETE FROM support_tickets WHERE ticket_id = {id};";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Ticket deleted successfully");
        }
    }
}
