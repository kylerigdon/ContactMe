using ContactMe.Client.Models;

namespace ContactMe.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        // Create
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId);
        
        // Read
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId);
        Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId);
        
        // Update
        Task DeleteCategoryAsync(int categoryId, string userId);
        
        // Delete
        Task UpdateCategoryAsync(CategoryDTO category, string userId);

        Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId);
    }
}
