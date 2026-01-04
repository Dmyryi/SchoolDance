using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Visit
    {
        public Guid VisitId {  get; set; }
        public Guid SubId { get; set; }
        public Subscription Subscription { get; set; } = null!;
        public Guid SheduleId {  get; set; }
        public Shedule Shedule { get; set; } = null!;
        public DateTime ActualDate {  get; set; }  
    }
}
