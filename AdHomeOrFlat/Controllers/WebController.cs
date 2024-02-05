using AdHomeOrFlat.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace AdHomeOrFlat.Controllers
{
    public class WebController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handlerUpdateService, [FromBody] Update update)
        { 
            await handlerUpdateService.EchoAsync(update);
            return Ok();
        }
    }
}
