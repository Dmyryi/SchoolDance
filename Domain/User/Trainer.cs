using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.User
{
    public class Trainer
    {
        public Guid TrainerId {  get; set; }
        public Guid UserId {  get; set; }
        public string Name {  get; set; }
        public string Phone { get; set; }
        public string Specialization {  get; set; }

        
    }
}
