using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoEntities.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }
        public virtual List<User> Users { get; set; } = new();
    }
}
