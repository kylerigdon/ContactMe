using System.ComponentModel.DataAnnotations;

namespace ContactMe.Client.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        public ICollection<ContactDTO> Contacts { get; set; } = new HashSet<ContactDTO>();
    }
}
