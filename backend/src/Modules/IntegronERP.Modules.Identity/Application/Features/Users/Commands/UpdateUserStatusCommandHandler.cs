using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public class UpdateUserStatusCommandHandler
    : IRequestHandler<UpdateUserStatusCommand, UpdateUserStatusResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserStatusCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateUserStatusResponse> Handle(
        UpdateUserStatusCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Prevent self-deactivation

        if (command.UserId == command.CurrentUserId &&
            request.IsActive == false)
        {
            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "You cannot deactivate your own account."
            };
        }

        // Find user

        var user = await _userRepository.GetByIdAsync(
            command.UserId,
            cancellationToken);

        if (user == null)
        {
            Console.WriteLine("USER NOT FOUND FROM DATABASE");

            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Tenant protection

        if (user.CompanyId != command.CompanyId)
        {
            Console.WriteLine("COMPANY ID MISMATCH");

            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "Company not found."
            };
        }

        if (command.CurrentUserId == user.Id && !request.IsActive)
        {
            return new UpdateUserStatusResponse
            {
                Success = false,
                Message = "You cannot deactivate your own account."
            };
        }

        // Update status

        user.IsActive = request.IsActive;

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateUserStatusResponse
        {
            Success = true,
            Message = request.IsActive
                ? "User activated successfully."
                : "User deactivated successfully."
        };
    }
}