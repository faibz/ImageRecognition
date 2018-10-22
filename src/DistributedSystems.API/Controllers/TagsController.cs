using System.Linq;
using System.Threading.Tasks;
using DistributedSystems.API.Models;
using DistributedSystems.API.Services;
using DistributedSystems.API.Validators;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _tagService;

        public TagsController(ITagsService tagService)
        {
            _tagService = tagService;
        }
        
        //TODO: security? API key? data so we know we can trust response? image hash? key per image?
        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitImageTags([FromBody] ImageTagData imageTagData)
        {
            //SECURITY STUFF HERE. HOW CAN WE TRUST THIS DATA? MAYBE JUST ADD AUTH ATTRIBUTE AND SETUP IN STARTUP

            await _tagService.ProcessImageTags(imageTagData);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SubmitMapImagePartTags([FromBody] MapTagData mapTagData)
        {
            return Ok();
        }
    }
}
