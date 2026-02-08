using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain;
using Domain.User;
using Isopoh.Cryptography.Blake2b;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProfileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<ProfileMeDto?> GetProfileMe(Guid userId, CancellationToken ct)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Subscriptions)
                .Include(u => u.Trainer)
                    .ThenInclude(t => t!.Shedules)
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);

            if (user == null) return null;

            StudentSummary? studentPart = null;
            if (user.Student != null)
            {
                var activeCount = user.Student.Subscriptions.Count(s => s.Status == "Active");
                studentPart = new StudentSummary(activeCount);
            }

            TrainerSummary? trainerPart = null;
            if (user.Trainer != null)
                trainerPart = new TrainerSummary(user.Trainer.TrainerId, user.Trainer.Specialization, user.Trainer.Shedules.Count);

            return new ProfileMeDto(
                user.Email,
                user.Name,
                user.Phone,
                user.Role,
                user.UserImg ?? "",
                studentPart,
                trainerPart
            );
        }

        public async Task<IReadOnlyList<SubscriptionDto>> GetMySubscriptions(Guid userId, CancellationToken ct)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Subscriptions)
                    .ThenInclude(sub => sub.Tariff)
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Subscriptions)
                    .ThenInclude(sub => sub.Discount)
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Subscriptions)
                    .ThenInclude(sub => sub.Visits)
                    .ThenInclude(v => v.Shedule)
                    .ThenInclude(sh => sh.DanceType)
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Subscriptions)
                    .ThenInclude(sub => sub.Visits)
                    .ThenInclude(v => v.Shedule)
                    .ThenInclude(sh => sh.Trainer)
                    .ThenInclude(tr => tr!.User)
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);

            if (user?.Student == null) return Array.Empty<SubscriptionDto>();

            return user.Student.Subscriptions.Select(sub => new SubscriptionDto(
                sub.SubId,
                sub.StartDate,
                sub.EndDate,
                sub.Status,
                new TariffDto(sub.Tariff.TariffId, sub.Tariff.Name, sub.Tariff.Price, sub.Tariff.DaysValid),
                sub.Discount == null ? null : new DiscountDto(sub.Discount.DiscountId, sub.Discount.Name, sub.Discount.Percent),
                sub.Visits.Select(v => new VisitDto(v.VisitId, v.ActualDate, MapSchedule(v.Shedule))).ToList()
            )).ToList();
        }

        public async Task<IReadOnlyList<ScheduleDto>> GetMySchedules(Guid userId, CancellationToken ct)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Subscriptions)
                    .ThenInclude(sub => sub.Visits)
                    .ThenInclude(v => v.Shedule)
                    .ThenInclude(sh => sh.DanceType)
                .Include(u => u.Student)
                    .ThenInclude(s => s!.Subscriptions)
                    .ThenInclude(sub => sub.Visits)
                    .ThenInclude(v => v.Shedule)
                    .ThenInclude(sh => sh.Trainer)
                    .ThenInclude(tr => tr!.User)
                .Include(u => u.Trainer)
                    .ThenInclude(t => t!.Shedules)
                    .ThenInclude(sh => sh.DanceType)
                .Include(u => u.Trainer)
                    .ThenInclude(t => t!.Shedules)
                    .ThenInclude(sh => sh.Trainer)
                    .ThenInclude(tr => tr!.User)
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);

            if (user == null) return Array.Empty<ScheduleDto>();

            var schedules = new List<ScheduleDto>();
            if (user.Student != null)
            {
                var fromVisits = user.Student.Subscriptions
                    .SelectMany(s => s.Visits)
                    .Select(v => v.Shedule)
                    .DistinctBy(sh => sh.SheduleId)
                    .ToList();
                foreach (var sh in fromVisits)
                    schedules.Add(MapSchedule(sh));
            }
            if (user.Trainer != null)
            {
                foreach (var sh in user.Trainer.Shedules)
                {
                    if (schedules.Any(s => s.SheduleId == sh.SheduleId)) continue;
                    schedules.Add(MapSchedule(sh));
                }
            }
            return schedules;
        }

        private static ScheduleDto MapSchedule(Shedule sh)
        {
            return new ScheduleDto(
                sh.SheduleId,
                sh.DayOfWeek,
                sh.StartTime,
                sh.Room,
                sh.Status,
                sh.DanceType?.Name ?? "",
                sh.DanceType?.DanceId ?? Guid.Empty,
                sh.Trainer?.User?.Name ?? "",
                sh.Trainer?.TrainerId ?? Guid.Empty
            );
        }

        public async Task<User?> Get(Guid userId, CancellationToken ct)
        {
            return await _dbContext.Users
                .Include(u => u.Student) 
                .AsNoTracking()         
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        public async Task<User?> Update(Guid id, ProfileDto req, CancellationToken ct)
        {
            var res = await _dbContext.Users.FirstOrDefaultAsync(c => c.UserId == id) ?? throw new Exception();
            res.Email = req.Email;
            res.Name = req.Name;
            res.Phone = req.Phone;
            res.Role = req.Role;
            await _dbContext.SaveChangesAsync(ct);
            return res;
        }

        public async Task CreateVisitAsync(Guid userId, Guid sheduleId, DateTime actDate, CancellationToken ct)
        {
            var subscription = await _dbContext.Subscriptions.Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.Student.UserId == userId && s.Status == "Active", ct);

            if (subscription == null)
            {
                throw new InvalidOperationException("ACTIVE_SUBSCRIPTION_NOT_FOUND");
            }
            var sheduleExists = await _dbContext.Shedules.AnyAsync(s => s.SheduleId == sheduleId, ct);
            if (!sheduleExists)
            {
                throw new InvalidOperationException("SCHEDULE_NOT_FOUND");
            }

            var visit = new Visit
            {
                VisitId = Guid.NewGuid(),
                SubId = subscription.SubId,   
                SheduleId = sheduleId,          
                ActualDate = actDate    
            };

            _dbContext.Visits.Add(visit);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task RescheduleVisitAsync(Guid visitId, Guid newSheduleId, DateTime newDate, CancellationToken ct)
        {
            var visit = await _dbContext.Visits
                .FirstOrDefaultAsync(v => v.VisitId == visitId, ct);

            if (visit == null)
                throw new InvalidOperationException("VISIT_NOT_FOUND");

            visit.SheduleId = newSheduleId;
            visit.ActualDate = newDate;
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
