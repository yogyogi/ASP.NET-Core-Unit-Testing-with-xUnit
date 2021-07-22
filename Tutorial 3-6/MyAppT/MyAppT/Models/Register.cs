using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAppT.Models
{
    public class Register
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(40, 60)]
        public int Age { get; set; }
    }
}
