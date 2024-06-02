using Application.Cqrs.Queries.UserQueries;
using Domain.ApiException;
using Domain.Authentication;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.Controllers
{
    public class Authentication : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginQueries queries)
        {
            try
            {
                return Ok(await Mediator.Send(queries));
            }
            catch (ApiException ex)
            {
                // Return the status code from the ApiException
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
    }
}
