using jwtToken.Models;
namespace jwtToken.Interfaces
{
   public interface IBookRepository
   {
      Task<IEnumerable<Book>> GetAll();
      Task<Book> GetById(int Id);
      Task Create(Book book);
      Task Update (Book book, Book updatedBook);
      Task Delete (Book book);
   }
}