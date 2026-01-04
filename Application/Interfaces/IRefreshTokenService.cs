using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<(string raw, DateTimeOffset expires)> IssueAsync(Guid userId, CancellationToken ct);

        Task<(Guid userId, string newRaw, DateTimeOffset newExpires)> RotateAsync(string incomingRaw, CancellationToken ct);
    }
}
