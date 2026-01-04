using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Discount
    {
        public Guid DiscountId {  get; set; }
        public string Name { get; set; } = string.Empty;
        public int Percent {  get; set; }

        public List<Subscription> Subscriptions { get; set; } = new();

    }
}
