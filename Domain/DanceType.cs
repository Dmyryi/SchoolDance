using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.User;

namespace Domain
{
   public class DanceType
    {
        public Guid DanceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public List<Shedule> Shedules { get; set; } = new();
    }
}
