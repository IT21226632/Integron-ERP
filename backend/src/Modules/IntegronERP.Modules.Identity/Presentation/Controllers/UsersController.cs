using IntegronERP.Modules.Identity.Application.Features.Users.Commands;
using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Application.Features.Users.Queries;
using IntegronERP.SharedKernel.Interfaces;
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
    private readonly ICurrentUserService _currentUser;

    public UsersController(
        IMediator mediator,
        ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(
        CreateUserRequest request)
    {
        if (!_currentUser.IsAuthenticated || _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new CreateUserResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var command = new CreateUserCommand(
            request,
            _currentUser.CompanyId);

        var response = await _mediator.Send(command);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetUsersResponse>> GetUsers()
    {
        if (!_currentUser.IsAuthenticated || _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetUsersResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetUsersQuery(_currentUser.CompanyId));

        return Ok(response);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetUserByIdResponse>> GetUserById(
        Guid id)
    {
        if (!_currentUser.IsAuthenticated || 
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetUserByIdResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }


    var response = await _mediator.Send(
        new GetUserByIdQuery(
            id,
            _currentUser.CompanyId));


    if (!response.Success)
    {
        return NotFound(response);
    }


    return Ok(response);
}

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateUserResponse>> UpdateUser(
        Guid id,
        UpdateUserRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new UpdateUserResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new UpdateUserCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<UpdateUserStatusResponse>> UpdateUserStatus(
        Guid id,
        UpdateUserStatusRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new UpdateUserStatusResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new UpdateUserStatusCommand(
                id,
                _currentUser.CompanyId,
                _currentUser.UserId,
                request));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}