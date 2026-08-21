using IntegronERP.Modules.Inventory.Application.Features.Categories.Commands;
using IntegronERP.Modules.Inventory.Application.Features.Categories.DTOs;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegronERP.Modules.Inventory.Application.Features.Categories.Queries;

namespace IntegronERP.Modules.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/v1/inventory/categories")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public CategoriesController(
        IMediator mediator,
        ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<ActionResult<CreateCategoryResponse>> CreateCategory(
        CreateCategoryRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new CreateCategoryResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new CreateCategoryCommand(
                request,
                _currentUser.CompanyId));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetCategoriesResponse>> GetCategories(
        [FromQuery] bool activeOnly = false)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetCategoriesResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetCategoriesQuery(
                _currentUser.CompanyId,
                activeOnly));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetCategoryByIdResponse>> GetCategoryById(
        Guid id)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetCategoryByIdResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetCategoryByIdQuery(
                id,
                _currentUser.CompanyId));

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategory(
        Guid id,
        UpdateCategoryRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new UpdateCategoryResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new UpdateCategoryCommand(
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
        public async Task<ActionResult<UpdateCategoryStatusResponse>>
            UpdateCategoryStatus(
                Guid id,
                UpdateCategoryStatusRequest request)
        {
            if (!_currentUser.IsAuthenticated ||
                _currentUser.CompanyId == Guid.Empty)
            {
                return Unauthorized(new UpdateCategoryStatusResponse
                {
                    Success = false,
                    Message = "Company information not found."
                });
            }

        var response = await _mediator.Send(
            new UpdateCategoryStatusCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}