using ContactMe.Client.Models;
using ContactMe.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ContactMe.Models
{
    public class TaskerItem
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Every task must have a name.")]
        public string? Name { get; set; }

        public bool IsComplete { get; set; }

        [Required]
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }

    }

    //Handle convert to TaskerItemDTO

    public static class TaskerItemExtension
    {
        public static TaskerItemDTO ToDTO(this TaskerItem item) => new TaskerItemDTO
        {
            Id = item.Id,
            Name = item.Name,
            IsComplete = item.IsComplete
        };
    }
}
