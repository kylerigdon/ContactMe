using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace ContactMe.Client.Services
{
    public class WASMContactDTOService : IContactDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMContactDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/contacts", contactDTO);
            response.EnsureSuccessStatusCode();

            ContactDTO? contact = await response.Content.ReadFromJsonAsync<ContactDTO>();
            return contact!;
        }

        public async Task DeleteContactAsync(int contactId, string userId)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync($"api/contacts/{contactId}");
            result.EnsureSuccessStatusCode();
        }

        public async Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId)
        {
            HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"api/contacts/{contactId}/email", emailData);

            if(result.IsSuccessStatusCode == true)
            {
                return true;
            }
            return false;
        }

        public async Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId)
        {
            ContactDTO? contactDTO = await _httpClient.GetFromJsonAsync<ContactDTO>($"api/contacts/{contactId}");

            return contactDTO;
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
        {
            IEnumerable<ContactDTO> contacts = await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>("api/contacts") ?? [];

            return contacts;
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByCategoryIdAsync(int categoryId, string userId)
        {
            IEnumerable<ContactDTO>? contacts = await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts?categoryId={categoryId}") ?? [];

            return contacts;
        }

        public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
        {
            IEnumerable<ContactDTO> contacts = await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts/search?SearchTerm={searchTerm}") ?? [];

            return contacts;
        }

        public async Task UpdateContactAsync(ContactDTO contact, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/contacts/{contact.Id}", contact);
            response.EnsureSuccessStatusCode();
        }
    }
}
