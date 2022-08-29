using jwtToken.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace jwtToken.Data
{
   public class AppDbContext : IdentityDbContext
   {
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
      {
         
      }
      public DbSet<Book> Books {get; set;}     
   }
}