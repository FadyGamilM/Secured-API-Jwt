using jwtToken.Models;
using jwtToken.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using jwtToken.Configurations;
namespace jwtToken.Controllers
{
   [ApiController]
   [Route("api/auth")]
   public class AuthController : ControllerBase
   {
      private readonly UserManager<IdentityUser> _userManager;
      private readonly jwtConfig _jwtConfig;
      public AuthController(UserManager<IdentityUser> userManager, jwtConfig jwtConfig)
      {
         this._jwtConfig = jwtConfig;
         this._userManager = userManager;
      }

      [HttpPost]
      [Route("register")]
      public async Task<IActionResult> Register([FromBody] UserRegisterationDto registerationDto)
      {
         // we need to validate the incoming request.
         if (ModelState.IsValid){
            // we need to check if this email is already exists
            
         }
         return BadRequest();
      }

   }
}