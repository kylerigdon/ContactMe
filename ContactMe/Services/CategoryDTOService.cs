using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using ContactMe.Models;
using ContactMe.Services.Interfaces;

namespace ContactMe.Services
{
    public class CategoryDTOService(ICategoryRepository repository) : ICategoryDTOService
    {
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                AppUserId = userId
            };

            Category createdCategory = await repository.CreateCategoryAsync(newCategory);

            return createdCategory.ToDTO();
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            IEnumerable<Category> categories = await repository.GetCategoriesAsync(userId);

            List<CategoryDTO> categoriesDTO = new List<CategoryDTO>();

            foreach(Category category in categories)
            {
                CategoryDTO categoryDTO = category.ToDTO();

                categoriesDTO.Add(categoryDTO);
            }

            return categoriesDTO;
        }
    }
}
