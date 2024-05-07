using ContactMe.Models;

namespace ContactMe.Services.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetCategoriesAsync(string userId);
    }
}
