using HotelBookingApi.Data;
using HotelBookingApi.Dtos;
using HotelBookingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            if (_context.Agents == null || !_context.Agents.Any())
            {
                return NotFound("No agents found.");
            }
            return Ok(_context.Agents.ToList());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          
            var emailcount = _context.Agents.Count(e => e.Email == registerDto.Email);
            if (emailcount > 0)
            {
                return BadRequest("Email already exists.");
            }
            var hashedPassword = new PasswordHasher<Agent>();
            var encryptedPassword= hashedPassword.HashPassword(new Agent(), registerDto.Password);
            var agent = new Agent
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                CommercialRegister = registerDto.CommercialRegister,
                TaxVisa = registerDto.TaxVisa,
                Role = "Agent",
            };
       
            agent.Password = encryptedPassword;
            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Registration successful." });
        }



        private string CreateToken(Agent user)
        {
            //Claims that contain inforamtion about the user   
            List<Claim> claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier,""+ user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
            };

            //JWT Token Generation
            var strKey = _configuration["JwtSettings:Key"];
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(strKey!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agent = _context.Agents.FirstOrDefault(a => a.Email == loginDto.Email);
            if (agent == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var passwordHasher = new PasswordHasher<Agent>();
            var result = passwordHasher.VerifyHashedPassword(agent, agent.Password, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid email or password.");
            }

            var jwt = CreateToken(agent);
            Console.WriteLine("Key = " + _configuration["JwtSettings:Key"]);

            var response = new
            {
                token = jwt,
                Message = "Login successful"
            };
            return Ok(response);
        }


    }
}

/////////////////////////////