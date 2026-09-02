using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Queries;

public class GetSupplierByIdQueryHandler
    : IRequestHandler<GetSupplierByIdQuery, GetSupplierByIdResponse?>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICurrentUserService _currentUser;

    public GetSupplierByIdQueryHandler(
        ISupplierRepository supplierRepository,
        ICurrentUserService currentUser)
    {
        _supplierRepository = supplierRepository;
        _currentUser = currentUser;
    }

    public async Task<GetSupplierByIdResponse?> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId;

        var supplier =
            await _supplierRepository.GetByIdAsync(
                request.Id,
                companyId,
                cancellationToken);

        if (supplier is null)
        {
            return null;
        }

        return new GetSupplierByIdResponse
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            PhoneNumber = supplier.PhoneNumber,
            ContactPerson = supplier.ContactPerson,
            Address = supplier.Address,
            IsActive = supplier.IsActive,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }
}