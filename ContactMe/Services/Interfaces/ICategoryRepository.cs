using ContactMe.Models;

namespace ContactMe.Services.Interfaces
{
    public interface ICategoryRepository
    {
        // Create
        Task<Category> CreateCategoryAsync(Category category);
        // Read
        Task<IEnumerable<Category>> GetCategoriesAsync(string userId);
        Task<Category?> GetCategoryByIdAsync(int categoryId, string userId);
        // Delete
        Task DeleteCategoryAsync(int categoryId, string userId);
        // Update
        Task UpdateCategoryAsync(Category category, string userId);
    }
}
