using System.ComponentModel.DataAnnotations;

namespace jwtToken.Models
{
   public class Book
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