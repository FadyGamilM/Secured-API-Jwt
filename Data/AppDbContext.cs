using jwtToken.Models;
using Microsoft.EntityFrameworkCore;
namespace jwtToken.Data
{
   public class AppDbContext : DbContext
   {
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
      {
         
      }
      public DbSet<Book> Books {get; set;}     
   }
}