using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.User;

namespace Application.DTOs
{
    public record ProfileDto( string Email, string Name, string Phone, User.TypeUser Role);
}
