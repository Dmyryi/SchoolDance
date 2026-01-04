using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class Student
    {
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
     
        public List<Subscription> Subscriptions { get; set; } = new();
        
    }
}
