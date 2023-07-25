using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        //repositories and interfaces should only deal with domain models,
        //any conversion to DTOs and vice verss should be handled in controllers
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetById(Guid id);
        Task<Category> CreateAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
    }
}
