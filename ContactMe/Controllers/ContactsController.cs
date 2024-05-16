using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using ContactMe.Data;
using ContactMe.Helpers.Extensions;
using ContactMe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContactMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactDTOService _contactDTOService;
        private string _userId => User.GetUserId()!; // [authorize] means userId cannot be null

        public ContactsController(IContactDTOService contactDTOService)
        {
            _contactDTOService = contactDTOService;
        }

        // GET: "api/contacts" OR "api/contacts?categoryId=4" -> list of user contacts, optionally filtered by category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts([FromQuery] int? categoryId)
        {
            try
            {
                if (categoryId == null)
                {
                    IEnumerable<ContactDTO> contacts = await _contactDTOService.GetContactsAsync(_userId);

                    return Ok(contacts);
                }
                else
                {
                    IEnumerable<ContactDTO> Contacts = await _contactDTOService.GetContactsByCategoryIdAsync(categoryId.Value, _userId);

                    return Ok(Contacts);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // GET: "api/contacts/5" -> a contact or 404
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ContactDTO?>> GetContactById([FromRoute] int id)
        {
            try
            {
                ContactDTO? contact = await _contactDTOService.GetContactByIdAsync(id, _userId);

                if (contact == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(contact);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // GET: "api/contacts/search?query=whatever" -> contacts matching the search query
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ContactDTO>>> SearchContacts([FromQuery] string SearchTerm)
        {
            try
            {
                IEnumerable<ContactDTO> contacts = await _contactDTOService.SearchContactsAsync(SearchTerm, _userId);

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // POST: "api/contacts" -> creates and returns the created contact
        [HttpPost]
        public async Task<ActionResult<ContactDTO>> CreateContact([FromBody] ContactDTO contact)
        {
            try
            {
                ContactDTO createdContact = await _contactDTOService.CreateContactAsync(contact, _userId);

                return Ok(createdContact);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // PUT: "api/contacts/5" -> updates the selected contact and returns Ok
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateContact([FromRoute] int id, [FromBody] ContactDTO updateContact)
        {
            try
            {
                if(id != updateContact.Id)
                {
                    return BadRequest();
                }
                else
                {
                    await _contactDTOService.UpdateContactAsync(updateContact, _userId);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // DELETE: "api/contacts/5" -> deletes the selected contact and returns NoContent
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteContact([FromRoute] int id)
        {
            try
            {
                await _contactDTOService.DeleteContactAsync(id, _userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // POST: "api/contacts/5/email" -> sends an email to contact and returns Ok or BadRequest to indicate success or failure
        [HttpPost("{id:int}/email")]
        public async Task<ActionResult> EmailContact([FromRoute] int id, EmailData emailData)
        {
            try
            {
                await _contactDTOService.EmailContactAsync(id, emailData, _userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }
    }
}
