using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transaction.Models
{
    public class Company
    {        
        //обязательные и не обязательные для установки свойства. будут сформированы поля таблицы
        //public string Name { get; set; } = "";    //"Name" TEXT NOT NULL
        //public string? Company { get; set; }      //"Company" TEXT
        
        //[Key]
        //[Required]
        public int Id { get; set; }
        //[Required]
        //[Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public  virtual List<User> Users { get; set; } = new List<User>();

    }
}
