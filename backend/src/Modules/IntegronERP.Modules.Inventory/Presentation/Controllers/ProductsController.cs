using IntegronERP.Modules.Inventory.Application.Features.Products.Commands;
using IntegronERP.Modules.Inventory.Application.Features.Products.DTOs;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegronERP.Modules.Inventory.Application.Features.Products.Queries;

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
}