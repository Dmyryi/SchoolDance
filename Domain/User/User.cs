using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
       
        public TypeUser Role { get; set; } = TypeUser.User;
        public enum TypeUser
        {
            Admin,
            Trainer,
            User
        }
    }
}
