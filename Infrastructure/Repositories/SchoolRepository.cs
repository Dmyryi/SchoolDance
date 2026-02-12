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


        public async Task<List<SheduleDto>> GetSheduleAsync(Guid trainerId, CancellationToken ct)
        {
            Console.WriteLine($"Запит розкладу для тренера: {trainerId}");
            return await _dbContext.Shedules
                .Where(s => s.TrainerId == trainerId) 
                .Select(s => new SheduleDto(
                    s.SheduleId,
                    s.DanceId,
                    s.DayOfWeek,
                    s.StartTime,
                    s.Room,
                    s.Status
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
                .Where(t => t.Specialization == specDance)
                .Select(t => new TrainerDto
                (
                    t.TrainerId,
                    t.Specialization,
                  
                    t.User != null ? t.User.Name : "Невідомо",
                    t.User != null ? t.User.UserImg : "default.jpg"
                ))
                .ToListAsync(ct);
        }
    }
}
