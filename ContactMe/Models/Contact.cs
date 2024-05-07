using ContactMe.Client.Helpers;
using ContactMe.Client.Models;
using ContactMe.Client.Models.Enums;
using ContactMe.Data;
using ContactMe.Helpers;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactMe.Models
{
    public class Contact
    {
        private DateTimeOffset? _birthDate;
        private DateTimeOffset _created;

        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters long.",  MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTimeOffset? BirthDate 
        { 
            get { return _birthDate; } 
            set { _birthDate = value?.ToUniversalTime(); } 
        }

        [Required]
        [Display(Name = "Address")]
        public string? Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string? Address2 { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public int ZipCode { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTimeOffset Created 
        { 
            get => _created; 
            set => _created = value.ToUniversalTime(); 
        }

        // Can only have one user so it uses a foreign key
        [Required]
        public string? AppUserId { get; set; }
        public virtual ApplicationUser? AppUser { get; set; }

        // there could be many catergories so you use an ICollection
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public Guid? ImageId { get; set; }
        public virtual ImageUpload? Image { get; set; }
    }
    
    public static class ContactExtensions
    {
        public static ContactDTO ToDTO(this Contact contact)
        {
            ContactDTO dto = new ContactDTO()
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                BirthDate = contact.BirthDate,
                Address1 = contact.Address1,
                Address2 = contact.Address2,
                City = contact.City,
                State = contact.State,
                ZipCode = contact.ZipCode,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Created = contact.Created,
                ImageUrl = contact.ImageId.HasValue ? $"api/uploads/{contact.ImageId}" : UploadHelper.DefaultContactImage,
            };

            //  to prevent Circular Object Reference aka infinite loop
            foreach(Category category in contact.Categories)
            {
                category.Contacts.Clear();

                CategoryDTO categoryDTO = category.ToDTO();
                dto.Categories.Add(categoryDTO);
            }

            return dto;
        }
    }
}
