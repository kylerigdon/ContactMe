using ContactMe.Client.Models;
using System.Net;

namespace ContactMe.Client.Services.Interfaces
{
    public interface ITaskerItemService
    {
        Task<TaskerItemDTO> CreateTaskerItemAsync(TaskerItemDTO taskerItem, string userId);
        Task<IEnumerable<TaskerItemDTO>> GetTaskerItemsAsync(string userId);
        Task<TaskerItemDTO> GetTaskerItemByIdAsync(Guid id, string userId);
        Task UpdateTaskerItemAsync(Guid id, TaskerItemDTO updateItem, string userId);
        Task DeleteTaskerItemAsync(Guid id, string userId);
    }
}
