using Application.DTOs;
using Azure;
using Domain;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace SchoolDance.Controllers
{

    [ApiController]
    [Route("api/school")]
    [AllowAnonymous]
    public class SchoolController : Controller
    {
        private readonly SchoolRepository _schoolRepo;
        private readonly ILogger<SchoolController> _logger;
        public SchoolController(ILogger<SchoolController> logger, SchoolRepository schoolRepo)
        {
            _schoolRepo = schoolRepo;
            _logger = logger;
        }
        
        [HttpGet("shedule/{trainerId}")]
        public async Task<ActionResult<List<SheduleDto>>> GetShedule([FromRoute] Guid trainerId, CancellationToken ct)
        {

            try
            {
                var res = await _schoolRepo.GetSheduleAsync(trainerId, ct);
                return Ok(res);
            }
            catch (OperationCanceledException)
            {
             
                return NoContent();
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error fetching schedule for trainer {TrainerId}",trainerId);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("dances")]
        public async Task<ActionResult<List<DanceDto>>> GetDance( CancellationToken ct)
        {

            try
            {
                var res = await _schoolRepo.GetDancesAsync(ct);
                return Ok(res);
            }
            catch (OperationCanceledException)
            {

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error fetching dances");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("dance/{danceId}")]
        public async Task<ActionResult<List<DanceDto>>> GetDanceById([FromRoute] Guid danceId, CancellationToken ct)
        {

            try
            {
                var res = await _schoolRepo.GetDanceByIdAsync(danceId, ct);
                if (res == null) return NotFound(new { message = "Dance type not found" });
                return Ok(res);
            }
            catch (OperationCanceledException)
            {

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error fetching dances for id {danceId}", danceId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("trainer/{specDance}")]
        public async Task<ActionResult<List<TrainerDto>>> GetTrainer([FromRoute] string specDance, CancellationToken ct)
        {

            try
            {
                var res = await _schoolRepo.GetTrainerAsync(specDance, ct);
                return Ok(res);
            }
            catch (OperationCanceledException)
            {

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error fetching trainer {specDance}", specDance);
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
