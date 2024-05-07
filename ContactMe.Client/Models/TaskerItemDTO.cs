using System.ComponentModel.DataAnnotations;

namespace ContactMe.Client.Models
{
    public class TaskerItemDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Every task must have a name.")]
        public string? Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
