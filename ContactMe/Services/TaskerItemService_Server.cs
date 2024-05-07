using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using ContactMe.Models;
using ContactMe.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace ContactMe.Services
{
    public class TaskerItemService_Server : ITaskerItemService
    {
        // Now we need a way to talk to the database!?!?!? ApplicationDBContext??
        private readonly ITaskerItemRepository _taskerItemRepository;

        public TaskerItemService_Server(ITaskerItemRepository taskerItemRepository)
        {
            _taskerItemRepository = taskerItemRepository;
        }

        public async Task<TaskerItemDTO> CreateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            TaskerItem newTaskerItem = new()
            {
                Name = taskerItem.Name,
                IsComplete = taskerItem.IsComplete,
                UserId = userId
            };

            TaskerItem createdTaskerItem = await _taskerItemRepository.CreateTaskerItemAsync(newTaskerItem);

            return createdTaskerItem.ToDTO();

        }

        public async Task<IEnumerable<TaskerItemDTO>> GetTaskerItemsAsync(string userId)
        {
            IEnumerable<TaskerItem> taskerItems = await _taskerItemRepository.GetTaskerItemsAsync(userId);

            List<TaskerItemDTO> taskerItemsDTO = new List<TaskerItemDTO>();

            foreach (TaskerItem item in taskerItems)
            {
                TaskerItemDTO taskerItemDTO = item.ToDTO();

                taskerItemsDTO.Add(taskerItemDTO);
            }

            return taskerItemsDTO;
        }

        public async Task<TaskerItemDTO> GetTaskerItemByIdAsync(Guid id, string userId)
        {
            TaskerItem? taskerItem = await _taskerItemRepository.GetTaskerItemByIdAsync(id, userId);

            TaskerItemDTO taskerItemDTO = taskerItem!.ToDTO();

            return taskerItemDTO;

        }

        public async Task UpdateTaskerItemAsync(Guid id, TaskerItemDTO updateItem, string userId)
        {
            TaskerItem updateTaskerItem = new()
            {
                Id = updateItem.Id,
                Name = updateItem.Name,
                IsComplete = updateItem.IsComplete,
                UserId = userId
            };

            await _taskerItemRepository.UpdateTaskerItemAsync(id, updateTaskerItem, userId);
        }

        public async Task DeleteTaskerItemAsync(Guid id, string userId)
        {
            await _taskerItemRepository.DeleteTaskerItemAsync(id, userId);
        }
    }
}
