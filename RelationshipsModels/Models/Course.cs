using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationshipsModels.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual List<User> Users { get; set; } = new(); //= new();
    }
}
