using HotelBookingApi.DTO.AccountDTO;
using HotelBookingApi.Models;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly IConfiguration config;

        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.SignInManager = signInManager;
            this.config = config;
            this.roleManager = roleManager;
        }

        [HttpPost("RegisterClient")]
        public async Task<IActionResult> RegisterClient(RegisterClientDto userDTO)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    PhoneNumber = userDTO.PhoneNumber,
                    Name = userDTO.Name
                };

                var result = await userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Client"))
                        await roleManager.CreateAsync(new IdentityRole("Client"));

                    await userManager.AddToRoleAsync(user, "Client");

                    return Ok("Registered Successfully");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return BadRequest(ModelState);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(userDTO.UserName);
                if (user != null)
                {
                    bool isPasswordValid = await userManager.CheckPasswordAsync(user, userDTO.Password);
                    if (isPasswordValid)
                    {
                        List<Claim> userClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Name)
                };

                        var userRoles = await userManager.GetRolesAsync(user);
                        foreach (var roleName in userRoles)
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, roleName));
                        }

                        var key = config["JWT:secretkey"];
                        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));

                        SigningCredentials signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken myToken = new JwtSecurityToken(
                            audience: config["JWT:AudienceIP"],
                            issuer: config["JWT:IssuerIP"],
                            expires: DateTime.Now.AddHours(1),
                            claims: userClaims,
                            signingCredentials: signingCredentials
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            expires = DateTime.Now.AddHours(1),
                            role = userRoles.FirstOrDefault()
                        });
                    }
                }
                ModelState.AddModelError("username", "User name or password invalid");
            }

            return BadRequest(ModelState);
        }
    }
}

/////////////////////////////

