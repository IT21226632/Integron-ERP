using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;

namespace IntegronERP.Modules.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/v1/inventory/warehouses")]
[Authorize]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public WarehousesController(
        IMediator mediator,
        ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<ActionResult<CreateWarehouseResponse>>
        CreateWarehouse(
            CreateWarehouseRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(
                new CreateWarehouseResponse
                {
                    Success = false,
                    Message = "Company information not found."
                });
        }

        var response = await _mediator.Send(
            new CreateWarehouseCommand(
                request,
                _currentUser.CompanyId));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetWarehousesResponse>>
        GetWarehouses(
            [FromQuery] bool activeOnly = false)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetWarehousesResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetWarehousesQuery(
                _currentUser.CompanyId,
                activeOnly));

        return Ok(response);
    }
}