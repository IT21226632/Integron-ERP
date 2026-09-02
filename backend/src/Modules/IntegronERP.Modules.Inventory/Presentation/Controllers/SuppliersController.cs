using IntegronERP.Modules.Inventory.Application.Features.Suppliers.Commands;
using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntegronERP.Modules.Inventory.Application.Features.Suppliers.Queries;

namespace IntegronERP.Modules.Inventory.Presentation.Controllers;

[ApiController]
[Route("api/v1/inventory/suppliers")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateSupplierResponse>>
        CreateSupplier(
            CreateSupplierRequest request)
    {
        var command = new CreateSupplierCommand(
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.ContactPerson,
            request.Address);

        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<GetSuppliersResponse>>
        GetSuppliers(
            [FromQuery] bool activeOnly = false)
    {
        var query = new GetSuppliersQuery(activeOnly);

        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetSupplierByIdResponse>>
        GetSupplierById(Guid id)
    {
        var query = new GetSupplierByIdQuery(id);

        var response = await _mediator.Send(query);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateSupplierResponse>>
        UpdateSupplier(
            Guid id,
            UpdateSupplierRequest request)
    {
        var command = new UpdateSupplierCommand(
            id,
            request.Name,
            request.Email,
            request.PhoneNumber,
            request.ContactPerson,
            request.Address);

        var response = await _mediator.Send(command);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<UpdateSupplierStatusResponse>>
        UpdateSupplierStatus(
            Guid id,
            UpdateSupplierStatusRequest request)
    {
        var command = new UpdateSupplierStatusCommand(
            id,
            request.IsActive);

        var response = await _mediator.Send(command);

        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }
}