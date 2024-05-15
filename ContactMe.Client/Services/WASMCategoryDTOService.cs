using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace ContactMe.Client.Services
{
    public class WASMCategoryDTOService : ICategoryDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMCategoryDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/categories", category);
            response.EnsureSuccessStatusCode();

            CategoryDTO? categoryDTO = await response.Content.ReadFromJsonAsync<CategoryDTO>();
            return categoryDTO!;
        }

        public async Task DeleteCategoryAsync(int categoryId, string userId)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/categories/{categoryId}");
            result.EnsureSuccessStatusCode();
        }

        public async Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/categories/{categoryId}/email", emailData);

            if(result.IsSuccessStatusCode == true) 
            {
                return true;            
            }
            return false;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            IEnumerable<CategoryDTO> categories = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>("api/categories") ?? [];

            return categories;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId)
        {
            CategoryDTO? categoryDTO = await _httpClient.GetFromJsonAsync<CategoryDTO>($"api/categories/{categoryId}");

            return categoryDTO;
        }

        public async Task UpdateCategoryAsync(CategoryDTO category, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}", category);
            response.EnsureSuccessStatusCode();
        }
    }
}
