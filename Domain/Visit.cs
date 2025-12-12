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
        public Guid ScheduleId {  get; set; }
        public DateTime ActualDate {  get; set; }  
    }
}
