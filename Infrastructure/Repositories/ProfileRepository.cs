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

        public async Task CreateVisitAsync(Guid userId, Guid sheduleId,DateTime actDate, CancellationToken ct)
        {
            // 1. Находим студента и его активную подписку
            // Мы используем .Include, чтобы сразу подтянуть подписку одним запросом
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
            {
                throw new InvalidOperationException("VISIT_NOT_FOUND");
            }

            
            visit.SheduleId = newSheduleId;
            visit.ActualDate = newDate;

            
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
