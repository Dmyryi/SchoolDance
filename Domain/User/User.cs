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
        
        public string Email { get; set; } = string.Empty; // Добавь сюда для авторизации
        public string PasswordHash { get; set; } = string.Empty;

          public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public TypeUser Role { get; set; } = TypeUser.User;

        public string UserImg { get; set; } = string.Empty;

        // Навигационные свойства
        public Student? Student { get; set; }
        public Trainer? Trainer { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = new();
        public enum TypeUser
        {
            Admin,
            Trainer,
            User
        }
    }
  
}
