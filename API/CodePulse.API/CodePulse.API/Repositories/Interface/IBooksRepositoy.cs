using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface IBooksRepositoy
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book?> GetBookById(Guid id);
        Task<Book> AddBookAsync(Book book);
        Task<Book?> UpdateBookAsync(Book book);
        Task<Book?> DeleteBookAsync(Guid id);
    }
}
