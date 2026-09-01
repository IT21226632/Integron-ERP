using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Commands;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.DTOs;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegronERP.Modules.Inventory.Application.Features.Warehouses.Queries;
using IntegronERP.Modules.Inventory.Domain.Constants;

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

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetWarehouseByIdResponse>>
        GetWarehouseById(Guid id)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetWarehouseByIdResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetWarehouseByIdQuery(
                id,
                _currentUser.CompanyId));

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateWarehouseResponse>>
        UpdateWarehouse(
            Guid id,
            UpdateWarehouseRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new UpdateWarehouseResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new UpdateWarehouseCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            if (response.Message == "Warehouse not found.")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<UpdateWarehouseStatusResponse>>
        UpdateWarehouseStatus(
            Guid id,
            UpdateWarehouseStatusRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(
                new UpdateWarehouseStatusResponse
                {
                    Success = false,
                    Message = "Company information not found."
                });
        }

        var response = await _mediator.Send(
            new UpdateWarehouseStatusCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpGet("{id:guid}/stock")]
    public async Task<ActionResult<GetWarehouseStockResponse>>
        GetWarehouseStock(Guid id)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(
                new GetWarehouseStockResponse
                {
                    Success = false,
                    Message = "Company information not found."
                });
        }

        var response = await _mediator.Send(
            new GetWarehouseStockQuery(
                id,
                _currentUser.CompanyId));

        if (!response.Success &&
            response.Message == "Warehouse not found.")
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPost("transfer-stock/{productId:guid}")]
    public async Task<ActionResult<TransferWarehouseStockResponse>>
        TransferWarehouseStock(
            Guid productId,
            TransferWarehouseStockRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(
                new TransferWarehouseStockResponse
                {
                    Success = false,
                    Message = "Company information not found."
                });
        }

        var response = await _mediator.Send(
            new TransferWarehouseStockCommand(
                productId,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            if (response.Message == "Product not found." ||
                response.Message == "Source warehouse not found." ||
                response.Message == "Destination warehouse not found.")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("{id:guid}/stock-movements")]
    public async Task<
        ActionResult<GetWarehouseStockMovementsResponse>>
        GetStockMovements(
            Guid id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery]
            WarehouseStockMovementType? movementType = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(
                new GetWarehouseStockMovementsResponse
                {
                    Success = false,
                    Message = "Company information not found."
                });
        }

        if (page < 1)
        {
            return BadRequest(
                new GetWarehouseStockMovementsResponse
                {
                    Success = false,
                    Message =
                        "Page must be greater than or equal to 1."
                });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(
                new GetWarehouseStockMovementsResponse
                {
                    Success = false,
                    Message =
                        "Page size must be between 1 and 100."
                });
        }

        if (fromDate.HasValue &&
            toDate.HasValue &&
            fromDate.Value > toDate.Value)
        {
            return BadRequest(
                new GetWarehouseStockMovementsResponse
                {
                    Success = false,
                    Message =
                        "From date cannot be later than to date."
                });
        }

        var response =
            await _mediator.Send(
                new GetWarehouseStockMovementsQuery(
                    id,
                    _currentUser.CompanyId,
                    page,
                    pageSize,
                    movementType,
                    fromDate,
                    toDate));

        if (!response.Success &&
            response.Message == "Warehouse not found.")
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPost("{warehouseId:guid}/products/{productId:guid}/return")]
    public async Task<ActionResult<ReturnWarehouseStockResponse>>
        ReturnWarehouseStock(
            Guid warehouseId,
            Guid productId,
            ReturnWarehouseStockRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(
                new ReturnWarehouseStockResponse
                {
                    Success = false,
                    Message = "Company information not found."
                });
        }

        var response = await _mediator.Send(
            new ReturnWarehouseStockCommand(
                productId,
                _currentUser.CompanyId,
                warehouseId,
                request));

        if (!response.Success)
        {
            if (response.Message == "Product not found." ||
                response.Message == "Warehouse not found." ||
                response.Message ==
                    "Product has no stock in this warehouse.")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }
}