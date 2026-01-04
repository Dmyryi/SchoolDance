using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Isopoh.Cryptography.Argon2;
using Application.Interfaces;

namespace SchoolDance.Infrastructure.Services.UserProfile
{
    public sealed class Argon2PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
            => Argon2.Hash(password);

        public bool Verify(string hash, string password)
            => Argon2.Verify(hash, password);
    }
}
