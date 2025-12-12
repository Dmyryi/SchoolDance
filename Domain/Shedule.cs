using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Shedule
    {
        public Guid SheduleId {  get; set; }
        public Guid DanceId { get; set; }
        public Guid TrainerId { get; set; }
        public string DayOfWeek {  get; set; }  
        public TimeOnly StartTime { get; set; }
        public string Room {  get; set; }
    }
}
