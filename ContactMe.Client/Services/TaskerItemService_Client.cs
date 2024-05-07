using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using System.Net;
using System.Net.Http.Json;

namespace ContactMe.Client.Services
{
    public class TaskerItemService_Client : ITaskerItemService
    {
        private readonly HttpClient _httpClient;

        public TaskerItemService_Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TaskerItemDTO> CreateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/TaskerItems", taskerItem);
            response.EnsureSuccessStatusCode();

            TaskerItemDTO? TaskerItemDTO = await response.Content.ReadFromJsonAsync<TaskerItemDTO>();
            return TaskerItemDTO!;
        }

        public async Task<IEnumerable<TaskerItemDTO>> GetTaskerItemsAsync(string userId)
        {
            IEnumerable<TaskerItemDTO> TaskerItems = await _httpClient.GetFromJsonAsync<IEnumerable<TaskerItemDTO>>("api/TaskerItems") ?? [];

            return TaskerItems;
        }

        public async Task<TaskerItemDTO> GetTaskerItemByIdAsync(Guid id, string userId)
        {
            TaskerItemDTO? TaskerItemDTO = await _httpClient.GetFromJsonAsync<TaskerItemDTO>($"api/TaskerItems/{id}");

            return TaskerItemDTO;
        }

        public async Task UpdateTaskerItemAsync(Guid id, TaskerItemDTO updateItem, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/TaskerItems/{id}", updateItem);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTaskerItemAsync(Guid id, string userId)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/TaskerItems/{id}");
            result.EnsureSuccessStatusCode();
        }
    }
}
