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
            var ExistingUser = await this._userManager.FindByEmailAsync(registerationDto.Email);
            //* if it exists, so we can't register this user again
            if (ExistingUser != null){
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

      [HttpPost]
      [Route("login")]
      public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
      {
         //! (1) check if the request format is valid
         if(ModelState.IsValid){
            //! (2) lookup for this email if its exists in DB or not
            var ExistingUser = await this._userManager.FindByEmailAsync(loginDto.Email);
            if (ExistingUser == null){
               return BadRequest(
                  new AuthResult(){
                     Result = false,
                     Errors = new List<string>(){
                        "Invalid Credentials"
                     }
                  }
               );
            }else{
               //! (3) check if the password matches the hashed pass in the DB
               var PassMatches = await this._userManager.CheckPasswordAsync(ExistingUser, loginDto.Password);
               if (PassMatches == true){
                  //! (4) generate a token
                  var token = GenerateJwtToken(ExistingUser);
                  return Ok(
                     new AuthResult(){
                        Result = true,
                        Token = token
                     }
                  );
               }else{
                  return BadRequest(
                  new AuthResult(){
                     Result = false,
                     Errors = new List<string>(){
                        "Invalid Credentials"
                     }
                  }
               );
               }
            }
         }else{
            return BadRequest(
               new AuthResult(){
                  Errors = new List<String>(){
                     "The Email and Password are required, Check your email and password format"
                  },
               Result = false
               }
            );
         }  
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