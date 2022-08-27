using jwtToken.Models;
using jwtToken.DTOs;
using AutoMapper;
namespace jwtToken.Helpers
{
   public class BooksProfile : Profile 
   {
      public BooksProfile()
      {
         CreateMap<Book, BookReadDto>();
         CreateMap<Book, BookCreateDto>();
         CreateMap<BookReadDto, Book>();
         CreateMap<BookCreateDto, Book>();
      }
   }
}