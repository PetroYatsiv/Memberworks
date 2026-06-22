using MemberWorks.Application.Organizations.Dtos;
using MemberWorks.Application.Organizations.Queries.GetMyOrganization;
using Microsoft.AspNetCore.Mvc;

namespace MemberWorks.Api.Controllers;

public class OrganizationsController : ApiControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<OrganizationDto>> Me()
        => Ok(await Mediator.Send(new GetMyOrganizationQuery()));
}