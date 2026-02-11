using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.UserProfile
{
    public sealed class RefreshTokenService(ApplicationDbContext db, IConfiguration cfg) : IRefreshTokenService
    {
        public async Task<(string raw, DateTimeOffset expires)> IssueAsync(Guid userId, CancellationToken ct)
        {

            Span<byte> b = stackalloc byte[32];
            RandomNumberGenerator.Fill(b);
            var raw = Base64UrlEncode(b);
            var hash = Sha256Hex(raw);
            var expires = DateTimeOffset.UtcNow.AddDays(30);

            db.RefreshTokens.Add(new RefreshToken
            {
                UserId = userId,
                TokenHash = hash,
                ExpiresAt = expires.UtcDateTime,

            });
            await db.SaveChangesAsync(ct);
            return (raw, expires);
        }
        public async Task<(Guid userId, string newRaw, DateTimeOffset newExpires)> RotateAsync(string incomingRaw, CancellationToken ct)
        {
            var incomingHash = Sha256Hex(incomingRaw);


            var current = await db.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == incomingHash, ct);

            if (current is null || current.RevokedAt != null || current.ExpiresAt <= DateTime.UtcNow)
                throw new InvalidOperationException("INVALID_REFRESH");

            current.RevokedAt = DateTime.UtcNow;

            Span<byte> b = stackalloc byte[32];
            RandomNumberGenerator.Fill(b);
            var newRaw = Base64UrlEncode(b);
            var newHash = Sha256Hex(newRaw);
            var days = int.TryParse(cfg["Jwt:RefreshDays"], out var d) ? d : 30;
            var newExpires = DateTimeOffset.UtcNow.AddDays(days);

            db.RefreshTokens.Add(new RefreshToken
            {
                UserId = current.UserId,
                TokenHash = newHash,
                ExpiresAt = newExpires.UtcDateTime
            });

            await db.SaveChangesAsync(ct);
            return (current.UserId, newRaw, newExpires);
        }

        private static string Base64UrlEncode(ReadOnlySpan<byte> bytes)
            => Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');

        private static string Sha256Hex(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }

        public async Task RevokeAsync(string incomingRaw, CancellationToken ct)
{

    var incomingHash = Sha256Hex(incomingRaw);

    var token = await db.RefreshTokens
        .FirstOrDefaultAsync(x => x.TokenHash == incomingHash && x.RevokedAt == null, ct);

    if (token != null)
    {

        token.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }
}
    }
}
