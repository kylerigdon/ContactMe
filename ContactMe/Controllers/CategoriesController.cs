using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using ContactMe.Helpers.Extensions;
using ContactMe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryDTOService _categoryDTOService;
        private string _userId => User.GetUserId()!; // [authorize] means userId cannot be null

        public CategoriesController(ICategoryDTOService categoryDTOService)
        {
            _categoryDTOService = categoryDTOService;
        }

        // GET: "api/categories" -> returns the users categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            try
            {
                IEnumerable<CategoryDTO> categories = await _categoryDTOService.GetCategoriesAsync(_userId);

                return Ok(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // GET: "api/categories/5" -> returns a category or 404
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO?>> GetCategory([FromRoute] int id)
        {
            try
            {
                CategoryDTO? category = await _categoryDTOService.GetCategoryByIdAsync(id, _userId);

                if (category == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(category);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // POST: "api/categories" -> creates a category and returns the created category
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO category)
        {
            try
            {
                CategoryDTO createdCategory = await _categoryDTOService.CreateCategoryAsync(category, _userId);

                return Ok(createdCategory);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // PUT: "api/categories/5" -> updates the selected category and returns Ok
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryDTO updateCategory)
        {
            try
            {
                if(id != updateCategory.Id)
                {
                    return BadRequest();
                }
                else
                {
                    await _categoryDTOService.UpdateCategoryAsync(updateCategory, _userId);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // DELETE: "api/categories/5" -> deletes the selected category and returns NoContent
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            try
            {
                await _categoryDTOService.DeleteCategoryAsync(id, _userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // POST: "api/categories/5/email" -> sends an email to category and returns Ok or BadRequest to indicate success or failure
        [HttpPost("{id:int}/email")]
        public async Task<ActionResult> EmailCategory([FromRoute] int id, EmailData emailData)
        {
            try
            {
                await _categoryDTOService.EmailCategoryAsync(id, emailData, _userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }
    }
}
