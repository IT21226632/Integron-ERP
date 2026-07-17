using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using IntegronERP.Modules.Identity.Domain.Repositories;
using IntegronERP.SharedKernel.Interfaces;
using MediatR;

namespace IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;

public class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, LogoutResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LogoutResponse> Handle(
        LogoutCommand command,
        CancellationToken cancellationToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(
            command.Request.RefreshToken,
            cancellationToken);

        if (token == null)
        {
            return new LogoutResponse
            {
                Success = false,
                Message = "Invalid refresh token."
            };
        }

        if (!token.IsActive)
        {
            return new LogoutResponse
            {
                Success = false,
                Message = "Refresh token is already inactive."
            };
        }

        token.Revoked = true;
        token.RevokedAt = DateTime.UtcNow;

        await _refreshTokenRepository.UpdateAsync(
            token,
            cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new LogoutResponse
        {
            Success = true,
            Message = "Logout successful."
        };
    }
}