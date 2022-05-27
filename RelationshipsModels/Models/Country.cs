using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationshipsModels.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CapitalId { get; set; }

        public virtual City Capital { get; set; }  // столица страны
        public virtual List<Company> Companies { get; set; } = new List<Company>();
    }
}
