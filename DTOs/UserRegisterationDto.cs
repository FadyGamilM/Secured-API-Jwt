using System.ComponentModel.DataAnnotations;

namespace jwtToken.DTOs
{
   public class UserRegisterationDto
   {
      [Required]
      public string Username { get; set; } 
      [Required]
      public string Email {get; set;}
      [Required]
      public string Password { get; set; }
   }
}