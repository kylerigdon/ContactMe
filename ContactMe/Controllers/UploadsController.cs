using ContactMe.Data;
using ContactMe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace ContactMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController(ApplicationDbContext context) : ControllerBase
    {

        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            ImageUpload? image = await context.Images.FirstOrDefaultAsync(i => i.Id == id);

            return image == null ? NotFound() : File(image.Data!, image.Type!);
        }

    }
}
