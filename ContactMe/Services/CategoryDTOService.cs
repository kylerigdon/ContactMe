using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using ContactMe.Models;
using ContactMe.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ContactMe.Services
{
    public class CategoryDTOService(ICategoryRepository repository, IEmailSender emailSender) : ICategoryDTOService
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

            IEnumerable<CategoryDTO> categoriesDTO = categories.Select(c => c.ToDTO());

            return categoriesDTO;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId)
        {
            Category? category = await repository.GetCategoryByIdAsync(categoryId, userId);
            return category?.ToDTO();
        }

        public async Task DeleteCategoryAsync(int categoryId, string userId)
        {
            await repository.DeleteCategoryAsync(categoryId, userId);
        }

        public async Task UpdateCategoryAsync(CategoryDTO category, string userId)
        {
            Category? categoryToUpdate = await repository.GetCategoryByIdAsync(category.Id, userId);

            if (categoryToUpdate is not null)
            {
                categoryToUpdate.Name = category.Name;

                categoryToUpdate.Contacts.Clear();
                await repository.UpdateCategoryAsync(categoryToUpdate, userId);
            }
        }

        public async Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId)
        {
            try
            {
                Category? category = await repository.GetCategoryByIdAsync(categoryId, userId);

                if (category is null || category.Contacts.Count < 1)
                {
                    return false;
                }

                string recipients = string.Join(';', category.Contacts.Select(c => c.Email!));
                await emailSender.SendEmailAsync(recipients, emailData.Subject!, emailData.Message!);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
