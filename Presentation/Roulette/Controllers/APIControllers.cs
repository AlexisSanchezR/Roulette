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
        public async Task<IActionResult> CreateRoulette([FromBody]UserRequest rouletteRequest)
        {
            var model = new RouletteModel();
            model.IdRoulette = Guid.NewGuid().ToString();
            await _userService.CreateRoulette(model);
            return Created("Roulette: ",model);
        }

        [HttpPost("{rouletteId}/Open")]
        public async Task<IActionResult> OpenRoulette (string rouletteId)
        {
            var updated = await _userService.ChangeState(rouletteId, RouletteState.Open);
            if (!updated)
            {
                return NotFound(new { Message = "Roulette not found." });
            }
            return Ok(new { Message = "Roulette Opened Successfully" });
        }

        [HttpPost("{rouletteId}/Close")]
        public async Task<IActionResult> CloseRoulette (string rouletteId)
        {
            var updated = await _userService.ChangeState(rouletteId, RouletteState.Close);
            if (!updated)
            {
                return NotFound(new { Message = "Roulette not found." });
            }
            return Ok(new { Message = "Roulette Closed Successfully" });
        }

        [HttpPost("Create-User")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest )
        {
            var model = new UserModel();
            model.IdUser = Guid.NewGuid().ToString();
            model.Credit = userRequest.Credit;
            await _userService.CreateUser(model);
            return Created("created user", userRequest);
        }
    }
}
