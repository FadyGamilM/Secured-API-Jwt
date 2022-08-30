namespace jwtToken.Models
{
   public class AuthResult
   {
      public string Token { get; set; } = "";
      public bool Result { get; set; } = false;
      public List<string> Errors { get; set; } = new List<string> (){};
   }
}