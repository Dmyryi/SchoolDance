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

        [HttpPost("book")]
       
        public async Task<IActionResult> BookClass([FromBody] BookingRequest req, CancellationToken ct)
        {
            try
            {
                
                var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

                await _profileRepo.CreateVisitAsync(userId, req.SheduleId, req.ActualDate, ct);

                return Ok(new { message = "Запись успешно создана" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("reschedule")]
        
        public async Task<IActionResult> Reschedule([FromBody] RescheduleRequest req, CancellationToken ct)
        {
            try
            {
                await _profileRepo.RescheduleVisitAsync(req.VisitId, req.NewSheduleId, req.NewDate, ct);

                return Ok(new { message = "Занятие перенесено" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}