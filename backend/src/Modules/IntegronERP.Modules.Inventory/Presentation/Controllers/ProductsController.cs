using IntegronERP.Modules.Inventory.Application.Features.Products.Commands;
using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegronERP.Modules.Inventory.Application.Features.Products.Queries;
using IntegronERP.Modules.Inventory.Application.Features.Stock.Commands;
using IntegronERP.Modules.Inventory.Application.Features.Stock.DTOs;
using IntegronERP.Modules.Inventory.Application.Features.Stock.Queries;

namespace IntegronERP.Modules.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/v1/inventory/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public ProductsController(
        IMediator mediator,
        ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductResponse>> CreateProduct(
        CreateProductRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new CreateProductResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new CreateProductCommand(
                request,
                _currentUser.CompanyId));

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetProductsResponse>> GetProducts(
        [FromQuery] bool activeOnly = false)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetProductsResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetProductsQuery(
                _currentUser.CompanyId,
                activeOnly));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetProductByIdResponse>> GetProductById(
        Guid id)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetProductByIdResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetProductByIdQuery(
                id,
                _currentUser.CompanyId));

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateProductResponse>> UpdateProduct(
        Guid id,
        UpdateProductRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new UpdateProductResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new UpdateProductCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            if (response.Message == "Product not found.")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<UpdateProductStatusResponse>>
        UpdateProductStatus(
            Guid id,
            UpdateProductStatusRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new UpdateProductStatusResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new UpdateProductStatusCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPatch("{id:guid}/stock")]
    public async Task<ActionResult<StockAdjustmentResponse>>
        AdjustStock(
            Guid id,
            StockAdjustmentRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new StockAdjustmentResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new StockAdjustmentCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            if (response.Message == "Product not found.")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("{id:guid}/stock/movements")]
    public async Task<ActionResult<GetStockMovementsResponse>>
        GetStockMovements(Guid id)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetStockMovementsResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetStockMovementsQuery(
                id,
                _currentUser.CompanyId));

        if (!response.Success &&
            response.Message == "Product not found.")
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpGet("{id:guid}/stock")]
    public async Task<ActionResult<GetProductStockResponse>>
        GetProductStock(Guid id)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new GetProductStockResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new GetProductStockQuery(
                id,
                _currentUser.CompanyId));

        if (!response.Success &&
            response.Message == "Product not found.")
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPost("{id:guid}/stock/reserve")]
    public async Task<ActionResult<ReserveStockResponse>>
        ReserveStock(
            Guid id,
            ReserveStockRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new ReserveStockResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new ReserveStockCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            if (response.Message == "Product not found.")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("{id:guid}/stock/release")]
    public async Task<ActionResult<ReleaseStockReservationResponse>>
        ReleaseStockReservation(
            Guid id,
            ReleaseStockReservationRequest request)
    {
        if (!_currentUser.IsAuthenticated ||
            _currentUser.CompanyId == Guid.Empty)
        {
            return Unauthorized(new ReleaseStockReservationResponse
            {
                Success = false,
                Message = "Company information not found."
            });
        }

        var response = await _mediator.Send(
            new ReleaseStockReservationCommand(
                id,
                _currentUser.CompanyId,
                request));

        if (!response.Success)
        {
            if (response.Message == "Product not found.")
            {
                return NotFound(response);
            }

            return BadRequest(response);
        }

        return Ok(response);
    }
}