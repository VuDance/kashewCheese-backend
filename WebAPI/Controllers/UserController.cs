using Application.Cqrs.Commands.UserCommands;
using Microsoft.AspNetCore.Mvc;
using MediatR;


namespace WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        
    }
}
