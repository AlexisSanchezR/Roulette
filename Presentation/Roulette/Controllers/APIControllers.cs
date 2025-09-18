using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Roulette.Bussines.Interfaces;
using Roulette.Domain.Models;
using Roulette.Models;

namespace Roulette.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class APIControllers : ControllerBase
    {
        private readonly IUserService _userService;
        public APIControllers(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("Create-Roulette")]
        public async Task<IActionResult> CreateRoulette([FromBody]RouletteRequest rouletteRequest)
        {
            var model = new UserModel();
            model.IdRoulette = Guid.NewGuid().ToString();
            await _userService.CreateRoulette(model);
            return Created("Roulette: ",model);
        }
    }
}
