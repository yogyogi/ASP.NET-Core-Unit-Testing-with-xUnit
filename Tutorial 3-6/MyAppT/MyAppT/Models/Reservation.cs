using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAppT.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
    }
}
