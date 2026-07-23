using IntegronERP.Modules.Identity.Application.Features.Users.DTOs;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Users.Commands;

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateUserResponse> Handle(
        UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        // Find user

        var user = await _userRepository.GetByIdAsync(
            command.UserId,
            cancellationToken);

        if (user == null)
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Tenant protection

        if (user.CompanyId != command.CompanyId)
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "User not found."
            };
        }

        // Email uniqueness

        var existingUser =
            await _userRepository.GetByEmailAsync(
                request.Email,
                cancellationToken);

        if (existingUser != null &&
            existingUser.Id != user.Id)
        {
            return new UpdateUserResponse
            {
                Success = false,
                Message = "Email already exists."
            };
        }

        // Update profile

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.UserName = request.Email;

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        await _unitOfWork.CommitAsync(
            cancellationToken);

        return new UpdateUserResponse
        {
            Success = true,
            Message = "User updated successfully."
        };
    }
}