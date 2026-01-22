using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DTOs;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure.Repositories
{
   public class AuthorizationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorizationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest req, IPasswordHasher hasher, IJwtService jwt, IRefreshTokenService rts, CancellationToken ct)
        {
            string emailNormal = req.Email.Trim().ToLowerInvariant();
            var exists = await _dbContext.Users.AnyAsync(u => u.Email == emailNormal, ct);
            if (exists) throw new InvalidOperationException("EMAIL_ALREADY_EXISTS");

            // 1. Создаем User
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = emailNormal,
                PasswordHash = hasher.Hash(req.Password),
                Name = req.Name,
                Phone = req.Phone,
                Role = User.TypeUser.User // Ученик
            };

            // 2. Создаем Student (Профиль)
            var student = new Student
            {
                StudentId = Guid.NewGuid(),
                UserId = user.UserId 
            };



            _dbContext.Users.Add(user);
            _dbContext.Students.Add(student); 

            await _dbContext.SaveChangesAsync(ct);

            await EnsureUserHasSubscription(user.UserId, ct);

            var (access, accessExp) = jwt.CreateAccessToken(user.UserId, user.Email);
            var (refresh, refreshExp) = await rts.IssueAsync(user.UserId, ct);

            return new RegisterResponse(
                new UserAuthDto(user.UserId, user.Email),
                new TokensDto(access, accessExp, refresh, refreshExp)
            );
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest req, IPasswordHasher hasher,
    IJwtService jwt,
    IRefreshTokenService rts, CancellationToken ct)
        {
            var emailNormal = req.Email.Trim().ToLowerInvariant();
            var user = await _dbContext.Users
              .SingleOrDefaultAsync(u => u.Email == emailNormal, ct);
            if (user is null)
                throw new InvalidOperationException("INVALID_CREDENTIALS");
            var ok = hasher.Verify(user.PasswordHash, req.Password);
            if (!ok)
                throw new InvalidOperationException("INVALID_CREDENTIALS");

            var (access, accessExp) = jwt.CreateAccessToken(user.UserId, user.Email);
            var (refresh, refreshExp) = await rts.IssueAsync(user.UserId, ct);


            var resp = new LoginResponse(
      new UserAuthDto(user.UserId, user.Email),
      new TokensDto(access, accessExp, refresh, refreshExp)
  );
            return resp;
        }



        public async Task EnsureUserHasSubscription(Guid userId, CancellationToken ct)
        {
            // 1. Сначала находим запись студента для этого пользователя
            var student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.UserId == userId, ct);

            if (student == null)
            {
                throw new InvalidOperationException("STUDENT_RECORD_NOT_FOUND");
            }

            // 2. Проверяем наличие подписки именно по StudentId
            var sub = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.StudentId == student.StudentId, ct);

            if (sub == null)
            {
                var defaultSub = new Subscription
                {
                    SubId = Guid.NewGuid(),
                    StudentId = student.StudentId, // ИСПОЛЬЗУЕМ ID СТУДЕНТА, А НЕ ЮЗЕРА!
                    TariffId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d471"),
                    Status = "Active",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(7)
                };

                _dbContext.Subscriptions.Add(defaultSub);
                await _dbContext.SaveChangesAsync(ct);
            }
        }

    }
}
