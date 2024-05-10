using ContactMe.Client.Models;

namespace ContactMe.Client.Services.Interfaces
{
    public interface IContactDTOService
    {
        Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId);
        Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId);
        Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId);
        Task UpdateContactAsync(ContactDTO contact, string userId);
        Task DeleteContactAsync(int contactId, string userId);
    }
}
