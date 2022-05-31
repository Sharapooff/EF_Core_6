using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transaction.Models
{
    public class User //pзависимая сущность, содержащая внешний ключ CompanyId
    {        
        [Key]
        [Required] 
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string? Name { get; set; }
        public int Age { get; set; }
        public int CompanyId { get; set; }      // внешний ключ (можно не указывать в Core 6)
        public int? PositionId { get; set; }

        public virtual Position Position { get; set; }
        public virtual Company Company { get; set; }    // навигационное свойство
        public virtual UserProfile Profile { get; set; }
        public virtual List<Course> Courses { get; set; } = new();

        /* Если внешний ключ не допускает значения null и требует наличия конкретного значения
         * - id связанного объекта Company, то устанавливается каскадное удаление записей: ON DELETE CASCADE.
         * Если допускает (public int? CompanyId), то без каскадного удаления.
        
        
        /* В принципе название свойства - внешнего ключа необязательно должно следовать выше описанным условностям.
         * Чтобы установить свойство в качестве внешнего ключа, применяется атрибут [ForeignKey]
         * 
         *  public int CompanyInfoKey { get; set; }
         *  [ForeignKey("CompanyInfoKey")]
         *  public Company Company { get; set; }
         */

    }
}
