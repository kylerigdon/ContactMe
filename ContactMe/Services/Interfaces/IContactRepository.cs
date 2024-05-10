using ContactMe.Models;

namespace ContactMe.Services.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContactAsync(Contact contact);
        
        Task AddCategoriesToContactAsync(int contactId, string userId, IEnumerable<int> categoryIds);

        Task RemoveCategoriesFromContactAsync(int contactId, string userId);


        Task<IEnumerable<Contact>> GetContactsAsync(string userId);

        Task<Contact?> GetContactByIdAsync(int contactId, string userId);

        Task UpdateContactAsync(Contact contact);
    }
}
