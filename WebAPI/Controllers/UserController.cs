using Application.Cqrs.Commands.UserCommands;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Domain.Authorization;
using Application.Cqrs.Queries.UserQueries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace WebAPI.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Authorize]
        [AuthorizePermission(ListPermission.ViewUser)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllUser()));
        }

    }
}
