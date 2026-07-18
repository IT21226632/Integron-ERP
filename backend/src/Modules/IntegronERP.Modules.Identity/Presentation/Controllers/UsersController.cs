using System.Security.Claims;
using IntegronERP.Modules.Identity.Application.Features.Users.Commands;
using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegronERP.Modules.Identity.Presentation.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(
        CreateUserRequest request)
    {
        var companyIdClaim = User.FindFirst("CompanyId")?.Value;

        if (string.IsNullOrWhiteSpace(companyIdClaim))
        {
            return Unauthorized(new CreateUserResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var companyId = Guid.Parse(companyIdClaim);

        var command = new CreateUserCommand(
            request,
            companyId);

        var response = await _mediator.Send(command);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}