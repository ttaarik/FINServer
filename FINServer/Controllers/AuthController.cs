using FINServer.Models;
using FINServer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FINServer.Controllers
{
    [Route("/register")]
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

    }
}
