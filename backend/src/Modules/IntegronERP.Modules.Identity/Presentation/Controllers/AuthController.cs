using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.Commands;
using IntegronERP.Modules.Identity.Application.Features.CompanyRegistration.DTOs;
using IntegronERP.Modules.Identity.Application.Features.Authentication.Commands;
using IntegronERP.Modules.Identity.Application.Features.Authentication.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IntegronERP.Modules.Identity.Presentation.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register-company")]
    public async Task<ActionResult<RegisterCompanyResponse>> RegisterCompany(
        RegisterCompanyRequest request)
    {
        var command = new RegisterCompanyCommand(request);

        var response = await _mediator.Send(command);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginRequest request)
    {
        var command = new LoginCommand(request);

        var response = await _mediator.Send(command);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Testing global exception middleware.");
        }
}