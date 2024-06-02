using Application.Cqrs.Commands.UserCommands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;


namespace WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        
    }
}
