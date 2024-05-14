using ContactMe.Data;
using ContactMe.Models;
using ContactMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactMe.Services
{
    public class ContactRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IContactRepository
    {
        public async Task AddCategoriesToContactAsync(int contactId, string userId, IEnumerable<int> categoryIds)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);
            
            if(contact is not null)
            {
                foreach(int categoryId in categoryIds)
                {
                    Category? category = await context.Categories
                                                      .FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);

                    if(category is not null)
                    {
                        contact.Categories.Add(category);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveCategoriesFromContactAsync(int contactId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact is not null)
            {
                contact.Categories.Clear();
                await context.SaveChangesAsync();
            }
        }

        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            return contact;
        }

        public async Task<Contact?> GetContactByIdAsync(int contactId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Contact> contacts = await context.Contacts
                                                         .Where(c => c.AppUserId == userId)
                                                         .Include(c => c.Categories)
                                                         .ToListAsync();

            return contacts;
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldEdit = await context.Contacts.AnyAsync(c => c.AppUserId == contact.AppUserId && c.Id == contact.Id);

            if (shouldEdit)
            {
                ImageUpload? oldImage = null;

                if(contact.Image is not null)
                {
                    // save the new image
                    context.Images.Add(contact.Image);

                    // check for an old image
                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == contact.ImageId);

                    // fix the foreign key
                    contact.ImageId = contact.Image.Id;
                }

                context.Contacts.Update(contact);
                await context.SaveChangesAsync();

                if(oldImage is not null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteContactAsync(int contactId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? existingContact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (existingContact is not null)
            {
                context.Contacts.Remove(existingContact);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Contact>> GetContactsByCategoryIdAsync(int categoryId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            List<Contact> contacts = new List<Contact>();

            contacts = await context.Contacts.Include(c => c.Categories)
                                             .Where(c => c.AppUserId == userId && c.Categories.Any(cat => cat.Id == categoryId))
                                             .ToListAsync();

            return contacts;
        }

        public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            string normalizedSearch = searchTerm.Trim().ToLower();

            List<Contact> contacts = await context.Contacts.Where(c => c.AppUserId == userId) // only include YOUR contacts
                                                           .Include(c => c.Categories) // make sure we bring the categories with us
                                                           .Where(c => string.IsNullOrEmpty(normalizedSearch)
                                                                       || c.FirstName!.ToLower().Contains(normalizedSearch)
                                                                       || c.LastName!.ToLower().Contains(normalizedSearch)
                                                                       || c.Categories.Any(cat => cat.Name!.ToLower().Contains(normalizedSearch))
                                                            )
                                                           .ToListAsync();
            
            return contacts;
        }
    }
}
