using FINServer.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace FINServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 1. Alle Karten abrufen
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM `cards`;";
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

        // 2. Karten für einen bestimmten Kunden abrufen
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT * FROM `cards` WHERE customer_id = @CustomerId;";
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

        // 3. Neue Karte hinzufügen
        [HttpPost]
        public JsonResult Post(Card card)
        {
            string query = @"
                INSERT INTO cards (customer_id, card_number, card_type, expiry_date, cvv, pin, monthly_spending, cred_limit, active)
                VALUES (@CustomerId, @CardNumber, @CardType, @ExpiryDate, @CVV, @PIN, @MonthlySpending, @CredLimit, @Active);";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CustomerId", card.CustomerId);
                    myCommand.Parameters.AddWithValue("@CardNumber", card.CardNumber);
                    myCommand.Parameters.AddWithValue("@CardType", card.card_type.ToString());
                    myCommand.Parameters.AddWithValue("@ExpiryDate", card.ExpiryDate);
                    myCommand.Parameters.AddWithValue("@CVV", card.CVV);
                    myCommand.Parameters.AddWithValue("@PIN", card.PIN);
                    myCommand.Parameters.AddWithValue("@MonthlySpending", card.MonthlySpending);
                    myCommand.Parameters.AddWithValue("@CredLimit", card.CredLimit);
                    myCommand.Parameters.AddWithValue("@Active", card.Active);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Card added successfully");
        }

        // 4. Bestehende Karte aktualisieren
        [HttpPut]
        public JsonResult Put(Card card)
        {
            string query = @"
                UPDATE cards
                SET customer_id = @CustomerId, card_number = @CardNumber, card_type = @CardType, expiry_date = @ExpiryDate,
                    cvv = @CVV, pin = @PIN, monthly_spending = @MonthlySpending, cred_limit = @CredLimit, active = @Active
                WHERE card_id = @CardId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CardId", card.CardId);
                    myCommand.Parameters.AddWithValue("@CustomerId", card.CustomerId);
                    myCommand.Parameters.AddWithValue("@CardNumber", card.CardNumber);
                    myCommand.Parameters.AddWithValue("@CardType", card.card_type.ToString());
                    myCommand.Parameters.AddWithValue("@ExpiryDate", card.ExpiryDate);
                    myCommand.Parameters.AddWithValue("@CVV", card.CVV);
                    myCommand.Parameters.AddWithValue("@PIN", card.PIN);
                    myCommand.Parameters.AddWithValue("@MonthlySpending", card.MonthlySpending);
                    myCommand.Parameters.AddWithValue("@CredLimit", card.CredLimit);
                    myCommand.Parameters.AddWithValue("@Active", card.Active);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Card updated successfully");
        }

        // 5. Karte löschen
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM cards WHERE card_id = @CardId;";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CardId", id);
                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Card deleted successfully");
        }
    }
}
