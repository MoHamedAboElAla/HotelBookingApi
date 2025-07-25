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

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            this.userManager = userManager;
            SignInManager = signInManager;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto userDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    Email = userDTO.Email,
                    UserName = userDTO.UserName,
                    PhoneNumber = userDTO.PhoneNumber,
                    Name = userDTO.Name,
                    CommercialRegister = userDTO.CommercialRegister,
                    TaxVisa = userDTO.TaxVisa,
                    HotelId = userDTO.HotelId
                };

                IdentityResult result = await userManager.CreateAsync(appUser, userDTO.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(appUser, false);
                    return Created();
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }

            }

            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> login(LoginDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                //check
                ApplicationUser user = await userManager.FindByNameAsync(userDTO.UserName);
                if (user != null)
                {

                    bool isPasswordValid = await userManager.CheckPasswordAsync(user, userDTO.Password);
                    if (isPasswordValid)
                    {
                        // Generate token
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

                        // Secret key
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