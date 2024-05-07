using ContactMe.Models;
using System.Net;

namespace ContactMe.Services.Interfaces
{
    public interface ITaskerItemRepository
    {
        Task<IEnumerable<TaskerItem>> GetTaskerItemsAsync(string userId);
        Task<TaskerItem> CreateTaskerItemAsync(TaskerItem item);
        Task<TaskerItem?> GetTaskerItemByIdAsync(Guid id, string userId);
        Task UpdateTaskerItemAsync(Guid id, TaskerItem updateItem, string userId);
        Task DeleteTaskerItemAsync(Guid id, string userId);
    }
}
