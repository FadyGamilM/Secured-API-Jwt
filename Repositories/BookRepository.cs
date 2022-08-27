using jwtToken.Data;
using jwtToken.Interfaces;
using jwtToken.Models;
using Microsoft.EntityFrameworkCore;
namespace jwtToken.Repositories
{
   public class BookRepository : IBookRepository
   {
      private readonly AppDbContext _context;
      public BookRepository(AppDbContext context)
      {
         this._context = context;
      }
      public async Task Create(Book book)
      {
         await this._context.Books.AddAsync(book);
         await this._context.SaveChangesAsync();
      }

      public async Task Delete(Book book)
      {
         this._context.Books.Remove(book);
         await this._context.SaveChangesAsync();
      }

      public async Task<IEnumerable<Book>> GetAll()
      {
         var books = await this._context.Books.AsNoTracking().ToListAsync();
         return books;
      }

      public async Task<Book> GetById(int Id)
      {
         var book = await this._context.Books.Where(book => book.Id == Id).FirstOrDefaultAsync();
         return book;
      }

      public async Task Update(Book book, Book UpdatedBook)
      {
         if (UpdatedBook.Name != null){
            book.Name = UpdatedBook.Name;
         }
         if (UpdatedBook.Year != null){
            book.Year = UpdatedBook.Year;
         }
         await this._context.SaveChangesAsync();
      }
   }
}