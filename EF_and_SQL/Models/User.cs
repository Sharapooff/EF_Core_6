using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_and_SQL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}
