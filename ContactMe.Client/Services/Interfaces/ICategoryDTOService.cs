using ContactMe.Client.Models;

namespace ContactMe.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId);
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId);
    }
}
