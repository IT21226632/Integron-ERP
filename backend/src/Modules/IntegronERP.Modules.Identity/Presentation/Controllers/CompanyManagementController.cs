using IntegronERP.Modules.Identity.Application.Features.CompanyManagement.Commands;
using IntegronERP.Modules.Identity.Application.Features.CompanyManagement.DTOs;
using IntegronERP.Modules.Identity.Application.Features.CompanyManagement.Queries;
using IntegronERP.Modules.Identity.Domain.Constants;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegronERP.Modules.Identity.Presentation.Controllers;

[ApiController]
[Route("api/v1/company")]
[Authorize]
public class CompanyManagementController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public CompanyManagementController(
        IMediator mediator,
        ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<GetCompanyResponse>> GetCompany()
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetCompanyResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetCompanyQuery(_currentUser.CompanyId));

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = Roles.Owner)]
    public async Task<ActionResult<UpdateCompanyResponse>> UpdateCompany(
        UpdateCompanyRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new UpdateCompanyResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new UpdateCompanyCommand(
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}