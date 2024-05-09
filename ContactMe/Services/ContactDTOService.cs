using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using ContactMe.Helpers;
using ContactMe.Models;
using ContactMe.Services.Interfaces;

namespace ContactMe.Services
{
    public class ContactDTOService(IContactRepository repository) : IContactDTOService
    {
        public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId)
        {
            // translate the DTO into contact
            Contact newContact = new Contact()
            {
                FirstName = contactDTO.FirstName,
                LastName = contactDTO.LastName,
                BirthDate = contactDTO.BirthDate,
                Address1 = contactDTO.Address1,
                Address2 = contactDTO.Address2,
                City = contactDTO.City,
                State = contactDTO.State,
                ZipCode = contactDTO.ZipCode,
                Email = contactDTO.Email,
                PhoneNumber = contactDTO.PhoneNumber,
                Created = DateTimeOffset.Now,
                AppUserId = userId,
            };

            if (contactDTO.ImageUrl.StartsWith("data:"))
            {
                newContact.Image = UploadHelper.GetImageUpload(contactDTO.ImageUrl);
            }

            // sent it to the repository
            Contact createdContact = await repository.CreateContactAsync(newContact);
            
            IEnumerable<int> categoryIds = contactDTO.Categories.Select(c => c.Id);
            await repository.AddCategoriesToContactAsync(createdContact.Id, userId, categoryIds);

            // return the created DTO
            return createdContact.ToDTO();
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
        {
            IEnumerable<Contact> contacts = await repository.GetContactsAsync(userId);

            List<ContactDTO> contactsDTO = new List<ContactDTO>();

            foreach(Contact contact in contacts)
            {
                contactsDTO.Add(contact.ToDTO());
            }

            return contactsDTO;
        }
    }
}
