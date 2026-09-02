using IntegronERP.Modules.Inventory.Application.Features.Suppliers.DTOs;
using IntegronERP.Modules.Inventory.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Inventory.Application.Features.Suppliers.Queries;

public class GetSuppliersQueryHandler
    : IRequestHandler<GetSuppliersQuery, GetSuppliersResponse>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICurrentUserService _currentUser;

    public GetSuppliersQueryHandler(
        ISupplierRepository supplierRepository,
        ICurrentUserService currentUser)
    {
        _supplierRepository = supplierRepository;
        _currentUser = currentUser;
    }

    public async Task<GetSuppliersResponse> Handle(
        GetSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId;

        var suppliers =
            await _supplierRepository.GetByCompanyIdAsync(
                companyId,
                request.ActiveOnly,
                cancellationToken);

        return new GetSuppliersResponse
        {
            Suppliers = suppliers
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    ContactPerson = s.ContactPerson,
                    Address = s.Address,
                    IsActive = s.IsActive,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToList()
        };
    }
}