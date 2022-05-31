using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallelism.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }

        [Timestamp] //параллелизм //using System.ComponentModel.DataAnnotations;
        public byte[] Timestamp { get; set; }
    }
}
