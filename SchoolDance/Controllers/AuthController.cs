using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace SchoolDance.Controllers
{



    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly AuthorizationRepository _authRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController(
         AuthorizationRepository authRepository,
         IPasswordHasher passwordHasher,
         IJwtService jwtService,
         IRefreshTokenService refreshTokenService)
        {
            _authRepository = authRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request, CancellationToken ct)
        {
            try
            {
                RegisterResponse? result = await _authRepository.RegisterAsync(
                    request,
                    _passwordHasher,
                    _jwtService,
                    _refreshTokenService,
                    ct
                    );

                    return Ok( result );

            }catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<LoginRequest>> Login(LoginRequest request, CancellationToken ct)
        {
            try
            {
                LoginResponse? result = await _authRepository.LoginAsync(
                request,
                _passwordHasher,
                _jwtService,
                _refreshTokenService,
                ct);

                return Ok(result);

            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = "INVALID_CREDENTIALS" });
            }
        }

        [HttpPost("logout")]
    [Authorize] 
    public async Task<IActionResult> Logout([FromBody] LogoutRequest req, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(req.RefreshToken))
        {
            return BadRequest("Token is required");
        }
        await _authRepository.LogoutAsync(req.RefreshToken, _refreshTokenService, ct);

        return Ok(new { message = "Logged out successfully" });
    }

    }
}
