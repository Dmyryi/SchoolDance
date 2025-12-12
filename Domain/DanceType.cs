using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public class DanceType
    {
        public Guid DanceId { get; set; }
        public Guid TrainerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
