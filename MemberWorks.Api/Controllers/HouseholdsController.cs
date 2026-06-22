using MemberWorks.Application.Households.Commands.AddHouseholdMember;
using MemberWorks.Application.Households.Commands.AddHouseholdRelationship;
using MemberWorks.Application.Households.Commands.CreateHousehold;
using MemberWorks.Application.Households.Dtos;
using MemberWorks.Application.Households.Queries.GetHouseHoldById;
using MemberWorks.Application.Households.Queries.GetHouseHolds;
using MemberWorks.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MemberWorks.Api.Controllers;

public class HouseholdsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<HouseholdSummaryDto>>> GetAll()
        => Ok(await Mediator.Send(new GetHouseholdsQuery()));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HouseholdDetailDto>> GetById(Guid id)
        => Ok(await Mediator.Send(new GetHouseholdByIdQuery(id)));

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateHouseholdCommand command)
    {
        var id = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>Add an existing organization user to the household.</summary>
    [HttpPost("{id:guid}/members")]
    public async Task<ActionResult<Guid>> AddMember(Guid id, [FromBody] AddMemberRequest request)
        => Ok(await Mediator.Send(new AddHouseholdMemberCommand(id, request.UserId)));

    /// <summary>Record a directed relationship between two members of the household.</summary>
    [HttpPost("{id:guid}/relationships")]
    public async Task<ActionResult<Guid>> AddRelationship(Guid id, [FromBody] AddRelationshipRequest request)
        => Ok(await Mediator.Send(
            new AddHouseholdRelationshipCommand(id, request.FromMemberId, request.ToMemberId, request.Type)));

    public record AddMemberRequest(Guid UserId);
    public record AddRelationshipRequest(Guid FromMemberId, Guid ToMemberId, RelationshipType Type);
}