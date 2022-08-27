using System.ComponentModel.DataAnnotations;

namespace jwtToken.DTOs
{
   public class BookCreateDto
   {
      [Required]
      public string Name {get; set;}
      [Required]
      public int Year {get; set;}
   }
}