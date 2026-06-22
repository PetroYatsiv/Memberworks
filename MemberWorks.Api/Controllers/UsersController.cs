using MemberWorks.Application.Users.Commands.CreateUser;
using MemberWorks.Application.Users.Dtos;
using MemberWorks.Application.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemberWorks.Api.Controllers;

public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> GetAll()
        => Ok(await Mediator.Send(new GetUsersQuery()));
    
    [Authorize(Roles = "OrgAdmin")]
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateUserCommand command)
    {
        var id = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetAll), new { id }, id);
    }
}