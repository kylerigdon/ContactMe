using ContactMe.Client.Models;
using ContactMe.Client.Services.Interfaces;
using ContactMe.Helpers.Extensions;
using ContactMe.Models;
using ContactMe.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ContactMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskerItemsController : ControllerBase
    {
        // Access a service that communicates with the database ApplicationDbContext??
        private readonly ITaskerItemService _taskerItemService;
        private string _userId => User.GetUserId()!;

        public TaskerItemsController(ITaskerItemService taskerItemService)
        {
            _taskerItemService = taskerItemService;
        }

        [HttpPost]
        public async Task<ActionResult<TaskerItemDTO>> PostDbTaskerItem([FromBody] TaskerItemDTO taskerItem)
        {
            TaskerItemDTO createdItem = await _taskerItemService.CreateTaskerItemAsync(taskerItem, _userId);

            return Ok(createdItem);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskerItemDTO>>> GetDBTaskerItems()
        {
            IEnumerable<TaskerItemDTO> taskerItems = await _taskerItemService.GetTaskerItemsAsync(_userId);

            return Ok(taskerItems);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskerItemDTO>> GetDBTaskerItemById([FromRoute] Guid id)
        {
            TaskerItemDTO taskerItemDTO = await _taskerItemService.GetTaskerItemByIdAsync(id, _userId);

            return Ok(taskerItemDTO);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateDBTaskerItem([FromRoute] Guid id, [FromBody] TaskerItemDTO updateItem)
        {
            if (id != updateItem.Id)
            {
                return BadRequest();
            }
            else
            {
                await _taskerItemService.UpdateTaskerItemAsync(id, updateItem, _userId);
                return Ok();
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteDBTaskerItem([FromRoute] Guid id)
        {
            try
            {
                await _taskerItemService.DeleteTaskerItemAsync(id, _userId);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
