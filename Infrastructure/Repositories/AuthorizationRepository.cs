using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.DTOs;
using Domain.User;
using Microsoft.EntityFrameworkCore;

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

            
            var (access, accessExp) = jwt.CreateAccessToken(user.UserId, user.Email);
            var (refresh, refreshExp) = await rts.IssueAsync(user.UserId, ct);

            return new RegisterResponse(
                new UserDto(user.UserId, user.Email),
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
      new UserDto(user.UserId, user.Email),
      new TokensDto(access, accessExp, refresh, refreshExp)
  );
            return resp;
        }

    }
}
