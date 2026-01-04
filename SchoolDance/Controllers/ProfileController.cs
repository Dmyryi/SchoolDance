using Application.DTOs;
using Domain.User;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SchoolDance.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ProfileRepository _profileRepo;

        public ProfileController(ProfileRepository profileRepo)
        {
            _profileRepo = profileRepo;
        }

        [HttpGet("me")]
        public async Task<ActionResult<ProfileDto>> GetMyProfile(CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

            
            var user = await _profileRepo.Get(userId, ct);

            if (user == null) return NotFound();

            var response = new ProfileDto(
                user.Email,
                user.Name,
                user.Phone,
                user.Role
            );

            return Ok(response);
        }

        [HttpPatch("redact")]

        public async Task<ActionResult<ProfileDto>> UpdatetMyProfile([FromBody] ProfileDto req, CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var userId = Guid.Parse(userIdClaim);

           User updateUser = await _profileRepo.Update(userId, req,ct);

            if (updateUser == null) return NotFound("User not found");

            
            var response = new ProfileDto(
                updateUser.Email,
                updateUser.Name,
                updateUser.Phone,
                updateUser.Role
            );


            return Ok(response);
        }
    }
}