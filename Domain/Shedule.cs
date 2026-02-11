using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.User;

namespace Domain
{
    public class Shedule
    {
        public Guid SheduleId {  get; set; }
        public Guid DanceId { get; set; }
        public DanceType DanceType { get; set; } = null!;
        public Guid TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;
        public string DayOfWeek { get; set; } = string.Empty;
        public TimeOnly StartTime { get; set; }

        public int Status { get; set; }

        public string Room { get; set; } = string.Empty;
        public int Status { get; set; } = 1;
        public List<Visit> Visits { get; set; } = new();
    }
}
