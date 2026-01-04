using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.User;

namespace Domain
{
   public class Subscription
    {
        public Guid SubId { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public Guid TariffId {  get; set; }
        public Tariff Tariff { get; set; } = null!;
        public Guid? DiscountId {  get; set; }
        public Discount? Discount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<Visit> Visits { get; set; } = new();
    }
}
