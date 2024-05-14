using FINServer.Models;
using FINServer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FINServer.Controllers
{
    
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
                bool loginSuccess = await _userRepository.LoginCustomerAsync(model);
                if (loginSuccess)
                    return Ok("Login successful");
                else
                    return Unauthorized("Invalid email or password");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error logging in: {ex.Message}");
            }
        }

    }
}
