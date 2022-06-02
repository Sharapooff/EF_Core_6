using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transaction.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
