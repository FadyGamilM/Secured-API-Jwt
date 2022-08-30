using jwtToken.Models;
using jwtToken.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using jwtToken.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace jwtToken.Controllers
{
   [ApiController]
   [Route("api/auth")]
   public class AuthController : ControllerBase
   {
      private readonly UserManager<IdentityUser> _userManager;
      private readonly IConfiguration _jwtConfig;
      public AuthController(UserManager<IdentityUser> userManager, IConfiguration jwtConfig)
      {
         this._jwtConfig = jwtConfig;
         this._userManager = userManager;
      }

      [HttpPost]
      [Route("register")]
      public async Task<IActionResult> Register([FromBody] UserRegisterationDto registerationDto)
      {
         //! (1) we need to validate the incoming request.
         if (ModelState.IsValid){
            //! (2) we need to check if this email is already exists
            var UserExists = await this._userManager.FindByEmailAsync(registerationDto.Email);
            //* if it exists, so we can't register this user again
            if (UserExists != null){
               return BadRequest(
                  new AuthResult() {
                     Result = false,
                     Errors = new List<string>() {
                        "This Email Already Registered Before"
                     } 
                  }
               );
            }
            //* if it doesn't exist, we ned to add this new user to our app
            else{
               //! (3) create new user object
               var newUser = new IdentityUser(){
                  Email = registerationDto.Email,
                  UserName = registerationDto.Username,
               };
               //! (4) Check if its saved correctely 
               var isSaved = await this._userManager.CreateAsync(newUser, registerationDto.Password);
               Console.WriteLine("RESULT OF SAVING IS ===> ", isSaved);
               //* if its saved successfully, we need to generate the token
               if (isSaved.Succeeded){
                  //! (5) generate the token
                  var token = GenerateJwtToken(newUser);
                  return Ok(
                     new AuthResult() {
                        Result = true,
                        Token = token
                     }
                  );
               }else{
                  // ModelState.AddModelError("", "Error while saving the new user to the database");
                  Console.WriteLine(isSaved);
                  return StatusCode(500, new AuthResult() {
                     Result = false,
                     Errors = new List<string>() {
                        "Server error while saving the new user to the database"
                     }
                  });
               }
            }

         }
         //! if the body of the request is not valid
         return BadRequest();
      }
      private string GenerateJwtToken(IdentityUser user)
      {
         var jwtTokenHandler = new JwtSecurityTokenHandler();

         var key = Encoding.UTF8.GetBytes(this._jwtConfig.GetSection("jwtConfig:Secret").Value);
         
         // Token descriptor
         var tokenDescriptor = new SecurityTokenDescriptor() 
         {
               Subject = new ClaimsIdentity(new []
               {
                  new Claim("Id", user.Id),
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Email, value:user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
               }),
               
               Expires = DateTime.Now.AddHours(1),
               SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
         };

         var token = jwtTokenHandler.CreateToken(tokenDescriptor);
         return jwtTokenHandler.WriteToken(token);
      }
    
   }
}