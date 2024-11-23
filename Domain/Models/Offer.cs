using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Models
{
    public class Offer : ModelBase
    {
        public decimal? Price { get; set; }        
        public EntityStatus Status { get; set; }
    }
}
