using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Tariff
    {

        public Guid TariffId {  get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price {  get; set; }
        public int DaysValid {  get; set; }

        public List<Subscription> Subscriptions { get; set; } = new();

    }
}
