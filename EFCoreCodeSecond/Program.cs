using EFCoreCodeSecond;

Console.WriteLine("CodeSecond_GO");

using(UsersContext db = new UsersContext())
{
    // получаем объекты из бд и выводим на консоль
    var users = db.Users.ToList();
    Console.WriteLine("Список объектов:");
    foreach (User u in users)
    {
        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
    }
}