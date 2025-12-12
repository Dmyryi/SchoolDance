using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public class Subscription
    {
        public Guid SubId { get; set; }
        public Guid StudentId { get; set; }
        public Guid TariffId {  get; set; }
        public Guid DiscountId {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
