using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTOs;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class BooksRepository : IBooksRepositoy
    {
        private readonly ApplicationDbContext _dbContext;
        public BooksRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _dbContext.Books.ToListAsync();
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book?> UpdateBookAsync(Book book)
        {
            var existingBook = _dbContext.Books.FirstOrDefault(x => x.Id == book.Id);
            if (existingBook != null)
            {
                _dbContext.Books.Entry(existingBook).CurrentValues.SetValues(book);
                await _dbContext.SaveChangesAsync();
                return book;
            }
            return null;
        }

        public async Task<Book?> GetBookById(Guid id)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Book?> DeleteBookAsync(Guid id)
        {
            var response = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (response == null) { return null; }
             _dbContext.Books.Remove(response);
            await _dbContext.SaveChangesAsync();
            return response;
        }
    }
}
