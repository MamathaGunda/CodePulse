using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTOs;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBooksRepositoy _booksRepository;
        public BooksController(IBooksRepositoy booksRepositoy)
        {
            _booksRepository = booksRepositoy;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _booksRepository.GetBooksAsync();
            var result = new List<BookDTO>();
            foreach (var book in books)
            {
                result.Add(new BookDTO { Id = book.Id, Name = book.Name, Price = book.Price });
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBookById([FromRoute] Guid id)
        {
            var book = await _booksRepository.GetBookById(id);
            if(book == null)
            {
                return NotFound();
            }
            var result = new BookDTO { Id = book.Id, Name = book.Name, Price = book.Price };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBookAsync([FromBody] CreateBookRequestDTO book)
        {
            var request = new Book { Name = book.Name, Price = book.Price };
            var c = await _booksRepository.AddBookAsync(request);
            var result = new BookDTO { Name = c.Name, Price = c.Price };
            return Ok(result);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBookAsync([FromRoute] Guid id,[FromBody] UpdateBookRequestDTO book)
        {
            var result = new Book {Id = id, Name = book.Name, Price = book.Price };
            result = await _booksRepository.UpdateBookAsync(result);
            if(result == null)
            {
                return NotFound();
            }
            var response = new BookDTO {Id = result.Id, Name = result.Name, Price = result.Price };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBookAsync([FromRoute] Guid id)
        {
            var result = await _booksRepository.DeleteBookAsync(id);
            if (result == null) { return NotFound(); }
            var response = new BookDTO { Id = result.Id, Name = result.Name, Price = result.Price };
            return Ok(response);
        }
    }
}
