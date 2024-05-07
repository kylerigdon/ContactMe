using ContactMe.Client.Services.Interfaces;
using ContactMe.Data;
using ContactMe.Models;
using ContactMe.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;

namespace ContactMe.Services
{
    public class TaskerItemRepository : ITaskerItemRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public TaskerItemRepository(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TaskerItem> CreateTaskerItemAsync(TaskerItem item)
        {
            using var context = _contextFactory.CreateDbContext();

            await context.TaskerItems.AddAsync(item);
            // Needed to save to the database
            await context.SaveChangesAsync();

            return item;
        }

        public async Task<IEnumerable<TaskerItem>> GetTaskerItemsAsync(string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            IEnumerable<TaskerItem> taskerItems = await context.TaskerItems.Where(t => t.UserId == userId).ToListAsync();

            return taskerItems;
        }

        public async Task<TaskerItem?> GetTaskerItemByIdAsync(Guid id, string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            TaskerItem? taskerItem = await context.TaskerItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            return taskerItem;
        }

        public async Task UpdateTaskerItemAsync(Guid id, TaskerItem updateItem, string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            TaskerItem? existingItem = await context.TaskerItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (existingItem is not null)
            {
                existingItem.Name = updateItem.Name;
                existingItem.IsComplete = updateItem.IsComplete;

                context.TaskerItems.Update(existingItem);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteTaskerItemAsync(Guid id, string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            TaskerItem? existingItem = await context.TaskerItems.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id);

            if (existingItem is not null)
            {
                context.TaskerItems.Remove(existingItem);
                await context.SaveChangesAsync();
            }
        }
    }
}