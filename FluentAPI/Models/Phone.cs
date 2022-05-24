using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluentAPI_Annotations.Models
{
    internal class Phone
    {
        //[Key] или [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Ident { get; set; }

        //Атрибут Required указывает, что данное свойство обязательно для установки,
        //то есть будет иметь определение NOT NULL в БД
        //[Required]

        // MaxLength и MinLength устанавливают максимальное и минимальное количество символов 
        //[MaxLength(20)]
        //Иногда требуется исключить определенное свойство, чтобы для него
        //не создавался столбец в таблице. И для этих целей есть атрибут NotMapped
        //[NotMapped]

        //С помощью атрибута Column можно задать тип данных для бд.
        //[Column(TypeName = "varchar(200)")]

        //Entity Framework при создании и сопоставлении таблиц и столбцов использует
        //имена моделей и их свойств. Но мы можем переопределить это поведение
        //с помощью атрибутов Table и Column
        //[Table("Mobiles")]
        //public class Phone
        //{
        //    public int Id { get; set; }
        //    [Column("ModelName")]
        //    public string Name { get; set; }
        //}

        //Чтобы установить внешний ключ для связи с другой сущностью, используется атрибут ForeignKey
        //public class Phone
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }

        //    public int? CompId { get; set; }
        //    [ForeignKey("CompId")]
        //    public Company Company { get; set; }
        //}
        //public class Company
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        //}

        //Для установки индекса для столбца к соответствующему свойству модели применяется атрибут [Index]

        //обязательные и не обязательные для установки свойства. будут сформированы поля таблицы
        //public string Name { get; set; } = "";    //"Name" TEXT NOT NULL
        //public string? Company { get; set; }      //"Company" TEXT

        public int Id { get; set; }
        public int Discount { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        //public string? LongName { get; set; }

        //вызов конструктора
        public Phone(string name, int price)
        {
            //Необязательно для всех свойств определять в конструкторе свои параметры. Те свойства, для которых
            //в конструкторе не определено параметров, устанавливаются напрямую, как в общем случае.
            //Конструкторы могут иметь любой модификатор доступа.
            Name = name;
            Price = price;
            Console.WriteLine($"Вызов конструктора для объекта {name}");
        }
    }
}
