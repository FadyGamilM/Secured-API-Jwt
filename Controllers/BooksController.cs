using Microsoft.AspNetCore.Mvc;
using jwtToken.Interfaces;
using jwtToken.DTOs;
using jwtToken.Models;
using AutoMapper;
namespace jwtToken.Controllers
{
   [ApiController]
   [Route("api/books")]
   public class BooksController : ControllerBase
   {
      private readonly IBookRepository _booksRepo;
      private readonly IMapper _mapper;
      public BooksController(IBookRepository booksRepo, IMapper mapper)
      {
         this._booksRepo = booksRepo;
         this._mapper = mapper;
      }
      [HttpGet]
      public async Task<IActionResult> GetBooks ()
      {
         var books = await this._booksRepo.GetAll();
         return Ok(books);
      }

      [HttpGet("{id:int}")]
      public async Task<IActionResult> GetBookById ([FromRoute] int id)
      {
         var book = await this._booksRepo.GetById(id);
         if (book == null){
            return NotFound();
         }
         var bookDto = this._mapper.Map<BookReadDto>(book);
         return Ok(bookDto);
      }

      [HttpPost]
      public async Task<IActionResult> CreateBook([FromBody] BookCreateDto bookDto)
      {
         await this._booksRepo.Create(
            this._mapper.Map<Book>(bookDto)
         );
         return Ok("Created");
      }

   
   }
}