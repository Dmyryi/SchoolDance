using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SchoolRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SchoolRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<SheduleDto>> GetSheduleAsync(Guid TrainerId, CancellationToken ct)
        {
            return await _dbContext.Shedules
    .Select(s => new SheduleDto(
        s.SheduleId,
        s.DayOfWeek,
        s.StartTime,
        s.Room
    ))
    .ToListAsync(ct);
        }


        public async Task<DanceDto?> GetDanceByIdAsync(Guid DanceId, CancellationToken ct)
        {
            return await _dbContext.DanceTypes
                .AsNoTracking()
                .Where(d=>d.DanceId == DanceId)
                .Select(s => new DanceDto(
        s.DanceId,
        s.Name,
        s.Category,
        s.DanceImg
    ))
                .FirstOrDefaultAsync(ct);
        }
        public async Task<List<DanceDto>> GetDancesAsync(CancellationToken ct)
        {
            return await _dbContext.DanceTypes
                .AsNoTracking()
                
                .Select(s => new DanceDto(
        s.DanceId,
        s.Name,
        s.Category,
        s.DanceImg
    ))
                .ToListAsync(ct);
        }

        public async Task<List<TrainerDto>> GetTrainerAsync(string specDance, CancellationToken ct)
        {
            return await _dbContext.Trainers
                .AsNoTracking()
                .Where(t=>t.Specialization == specDance)
                .Select(s=>new TrainerDto(
                    s.TrainerId,
                    s.Specialization
                    ))
                .ToListAsync(ct);
        }
    }
}
