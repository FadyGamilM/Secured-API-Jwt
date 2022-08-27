using System.ComponentModel.DataAnnotations;

namespace jwtToken.DTOs
{
   public class BookReadDto
   {
      [Key]
      [Required]
      public int Id {get; set;}
      [Required]
      public string Name {get; set;}
      [Required]
      public int Year {get; set;}
   }
}