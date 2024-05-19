using FINServer.Models;
using FINServer.Repositories;
using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mysqlx.Session;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FINServer.Controllers
{
    //TODO
    //Sessions im Backend mit Microsoft.AspNetCore.Session oder so
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CustomerRepository _userRepository;

        public AuthController(IConfiguration configuration, CustomerRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }
        
        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            try
            {
                await _userRepository.AddCustomerAsync(model);
                return Ok("Registration successful");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registering user: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            try
            {
                Customer customer = await _userRepository.GetCustomerByEmail(model.Email, model.Password);

                if (customer != null && customer.Password == model.Password)
                {
                    var token = Generate(customer);
                    return Ok(token);
                }
                else
                {
                    return Unauthorized("Invalid email or password");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error logging in: {ex.Message}");
            }
        }



        private string Generate(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, customer.Email),
                // Zusätzliche Benutzerdaten hinzufügen
                new Claim("first_name", customer.FirstName), // Vorname
                new Claim("last_name", customer.LastName)    // Nachname
                // Weitere Claims hinzufügen, falls erforderlich
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }





        //Der Login Basic klappt
        //[HttpPost]
        //[Route("/login")]
        //public async Task<IActionResult> Login([FromBody] Login model)
        //{
        //    try
        //    {
        //        bool loginSuccess = await _userRepository.LoginCustomerAsync(model);
        //        if (loginSuccess)
        //            return Ok("Login successful");

        //        //Create Token

        //        else
        //            return Unauthorized("Invalid email or password");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error logging in: {ex.Message}");
        //    }
        //}

    }
}
