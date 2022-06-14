using EF_CRUD.Contexts;
using EF_CRUD.Models;

class Program
{
    static void Main(string[] args)
    {
        // Добавление
        using (ApplicationContext db = new ApplicationContext())
        {
            User tom = new User { Name = "Tom", Age = 33 };
            User alice = new User { Name = "Alice", Age = 26 };

            // Добавление
            db.Users.AddAsync(tom);
            db.Users.AddAsync(alice);
            //db.Users.AddRangeAsync(tom, alice);
            db.SaveChangesAsync();
        }

        // получение
        using (ApplicationContext db = new ApplicationContext())
        {
            // получаем объекты из бд и выводим на консоль
            var users = db.Users.ToList();
            Console.WriteLine("Данные после добавления:");
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }

        // Редактирование
        using (ApplicationContext db = new ApplicationContext())
        {
            // получаем первый объект
            User? user = db.Users.FirstOrDefault();
            if (user != null)
            {
                user.Name = "Bob";
                user.Age = 44;
                //обновляем объект
                //db.Users.Update(user); //если обьект не связан с текущим контекстом
                db.SaveChangesAsync();
            }
            // выводим данные после обновления
            Console.WriteLine("\nДанные после редактирования:");
            var users = db.Users.ToList();
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }

        // Удаление
        using (ApplicationContext db = new ApplicationContext())
        {
            // получаем первый объект
            User? user = db.Users.FirstOrDefault();
            if (user != null)
            {
                //удаляем объект
                db.Users.Remove(user);
                //db.Users.RemoveRange(firstUser, secondUser);
                db.SaveChangesAsync();
            }
            // выводим данные после обновления
            Console.WriteLine("\nДанные после удаления:");
            var users = db.Users.ToList();
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }
    }
}