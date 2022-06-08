using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using LINQtoEntities.Models;
using LINQtoEntities.Contexts;
using Microsoft.Extensions.Configuration;

Console.WriteLine("_________ Start LINQ to Entities App _________");

//----------------------------- SqlServer ---------------------------
var builder_SqlServer = new ConfigurationBuilder();
builder_SqlServer.SetBasePath(Directory.GetCurrentDirectory());// установка пути к текущему каталогу
builder_SqlServer.AddJsonFile("appsettings.json");// получаем конфигурацию из файла appsettings.json
var config = builder_SqlServer.Build();// создаем конфигурацию
string connectionString_SqlServer = config.GetConnectionString("SqlServer");// получаем строку подключения

var optionsBuilder_SqlServer = new DbContextOptionsBuilder<ApplicationContext>();
var options_SqlServer = optionsBuilder_SqlServer.UseSqlServer(connectionString_SqlServer).Options;
using (ApplicationContext db = new ApplicationContext(options_SqlServer))
{
    #region пересоздаем базу данных
    Console.WriteLine("Пересоздаем базу данных ...");    
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    Country usa = new Country { Name = "USA" };

    Company microsoft = new Company { Name = "Microsoft", Country = usa };
    Company google = new Company { Name = "Google", Country = usa };
    db.Companies.AddRange(microsoft, google);

    User tom = new User { Name = "Tom", Age = 36, Company = microsoft };
    User bob = new User { Name = "Bob", Age = 39, Company = google };
    User alice = new User { Name = "Alice", Age = 28, Company = microsoft };
    User kate = new User { Name = "Kate", Age = 25, Company = google };

    db.Users.AddRange(tom, bob, alice, kate);
    db.SaveChanges();
    #endregion
}

using (ApplicationContext db = new ApplicationContext(options_SqlServer))
{
    #region получаем данные из БД
    Console.WriteLine("Получаем данные из БД (операторы LINQ)");
    var users = (from user in db.Users // или .Include(p=>p.Company)
                 where user.CompanyId == 1
                 select user).ToList();
    foreach (var user in users) Console.WriteLine($"{user.Name} ({user.Age}) - {user.Company?.Name}");    

    Console.WriteLine("Получаем данные из БД (методы расширения LINQ)");
    users = await db.Users.Include(p => p.Company).Where(p => p.CompanyId == 1).ToListAsync();     // асинхронное получение данных
    //var users = db.Users.Include(p => p.Company).Where(p => p.CompanyId == 1);
    foreach (var user in users) Console.WriteLine($"{user.Name} ({user.Age}) - {user.Company?.Name}");
    #endregion

    #region Основные методы, которые мы можем использовать для создания запросов в Entity Framework Core:
    /*
     *Основные методы, которые мы можем использовать для создания запросов в Entity Framework Core:

    All / AllAsync: возвращает true, если все элементы набора удовлятворяют определенному условию, иначе возвращает false
    Any / AnyAsync: возвращает true, если хотя бы один элемент набора определенному условию
    Average / AverageAsync: подсчитывает cреднее значение числовых значений в наборе
    Contains / ContainsAsync: определяет, содержит ли набор определенный элемент
    Count / CountAsync: подсчитывает количество элементов в наборе
    First / FirstAsync: выбирает первый элемент коллекции
    FirstOrDefault / FirstOrDefaultAsync: выбирает первый элемент коллекции или возвращает значение по умолчанию
    Single / SingleAsync: выбирает единственный элемент коллекции, если коллекция содердит больше или меньше одного элемента, то генерируется исключение
    SingleOrDefault / SingleOrDefaultAsync: выбирает первый элемент коллекции или возвращает значение по умолчанию
    Select: определяет проекцию выбранных значений
    Where: определяет фильтр выборки
    OrderBy: упорядочивает элементы по возрастанию
    OrderByDescending: упорядочивает элементы по убыванию
    ThenBy: задает дополнительные критерии для упорядочивания элементов возрастанию
    ThenByDescending: задает дополнительные критерии для упорядочивания элементов по убыванию
    Join: соединяет два набора по определенному признаку
    GroupBy: группирует элементы по ключу
    Except: возвращает разность двух наборов, то есть те элементы, которые содератся только в одном наборе
    Union: объединяет два однородных набора
    Intersect: возвращает пересечение двух наборов, то есть те элементы, которые встречаются в обоих наборах элементов
    Sum / SumAsync: подсчитывает сумму числовых значений в коллекции
    Min / MinAsync: находит минимальное значение
    Max / MaxAsync: находит максимальное значение
    Take: выбирает определенное количество элементов с начала последовательности
    TakeLast: выбирает определенное количество элементов с конца последовательности
    Skip: пропускает определенное количество элементов с начала последовательности
    SkipLast: пропускает определенное количество элементов с конца последовательности
    TakeWhile: возвращает цепочку элементов последовательности, до тех пор, пока условие истинно
    SkipWhile: пропускает элементы в последовательности, пока они удовлетворяют заданному условию, и затем возвращает оставшиеся элементы
    ToList / ToListAsync: получения списка объектов

     */

    #endregion

    #region Where (фильтрация данных)
    Console.WriteLine("Where (фильтрация данных)");
    users = await db.Users.Where(p => p.Company!.Name == "Google").ToListAsync();
    //users = (from user in db.Users where user.Company!.Name == "Google" select user).ToList();
    foreach (User user in users) Console.WriteLine($"{user.Name} ({user.Company.Name})");
    #endregion

    #region EF.Functions.Like (вхождение подстроки)
    Console.WriteLine("Like (вхождение подстроки)");
    users = await db.Users.Where(p => EF.Functions.Like(p.Name!, "%Tom%")).ToListAsync();
    foreach (User user in users) Console.WriteLine($"{user.Name} ({user.Age})");
    users = await db.Users.Where(u => EF.Functions.Like(u.Age.ToString(), "2%")).ToListAsync(); //приводим к srting
                                                                                                //users = (from u in db.Users where EF.Functions.Like(u.Age.ToString(), "2%") select u).ToList();
    /*
     Для определения шаблона могут применяться ряд специальных символов подстановки:
    %: соответствует любой подстроке, которая может иметь любое количество символов, при этом подстрока может и не содержать ни одного символа
    _: соответствует любому одиночному символу
    [ ]: соответствует одному символу, который указан в квадратных скобках
    [ - ]: соответствует одному символу из определенного диапазона
    [ ^ ]: соответствует одному символу, который не указан после символа ^
     */
    #endregion

    #region Find()/FindAsync() (поиск объекта)
    Console.WriteLine("Find()/FindAsync() (поиск объекта)");
    //var user = db.Users.Find(3); // выберем элемент с id=3                                  
    var userF = await db.Users.FindAsync(3); // выберем элемент с id=3
    if (userF != null) Console.WriteLine($"{userF.Name} ({userF.Age})");
    #endregion

    #region First/FirstOrDefault/FirstAsync/FirstOrDefaultAsync
    Console.WriteLine("First/FirstOrDefault/FirstAsync/FirstOrDefaultAsync (поиск объекта)");
    // Использование метода FirstOrDefault() является более гибким, так как если выборка пуста,
    // то он вернет значение null. А метод First() в той же ситуации выбросит ошибку.
    var userFD = await db.Users.FirstOrDefaultAsync();
    if (userFD != null) Console.WriteLine(userFD.Name);
    userFD = await db.Users.FirstOrDefaultAsync(p => p.Id == 3);// с условием
    if (userFD != null) Console.WriteLine(userFD.Name);
    #endregion

    #region Проекция (Проекция позволяет получить из набора объектов одного типа набор объектов другого типа).
    //В этом случае мы можем применить метод Select для проекции извлеченных данных на новый тип
    Console.WriteLine("Проекция");
    // получим анонимный тип
    var usersAnon = db.Users.Select(p => new
    {
        Name = p.Name,
        Age = p.Age,
        Company = p.Company!.Name
    });
    foreach (var user in usersAnon) Console.WriteLine($"{user.Name} ({user.Age}) - {user.Company}");
    /* так же можно создать и свой тип и заполнить его:
        public class UserModel
        {
            public string? Name { get; set; }
            public string? Company { get; set; }
            public int Age { get; set; }
        }
        ...
        var users = db.Users.Select(p => new UserModel
        { 
            Name = p.Name, 
            Age = p.Age, 
            Company = p.Company!.Name 
        });
     */

    #endregion

    #region Сортировка OrderBy() & OrderByDescending()
    Console.WriteLine("Сортировка OrderBy & OrderByDescending");
    users = await db.Users.OrderBy(p => p.Name).ToListAsync();
    //users = from u in db.Users orderby u.Name select u;
    users = await db.Users.OrderByDescending(u => u.Name).ToListAsync();
    foreach (var user in users) Console.WriteLine($"{user.Id}.{user.Name} ({user.Age})");
    //При необходимости упорядочить данные сразу по нескольким критериям можно использовать методы
    //ThenBy()(для сортировки по возрастанию) и ThenByDescending(). Например, отсортируем по двум значениям.
    users = await db.Users.OrderBy(u => u.Age).ThenBy(u => u.Company!.Name).ToListAsync();
    #endregion

    #region Соединение таблиц Join()
    Console.WriteLine("Соединение таблиц Join");
    /* Метод Join принимает четыре параметра:
     * вторую таблицу, которая соединяется с текущей
     * свойство объекта - столбец из первой таблицы, по которому идет соединение
     * свойство объекта - столбец из второй таблицы, по которому идет соединение
     * новый объект, который получается в результате соединения
     */
    var usersJ = db.Users.Join(db.Companies, // второй набор
        u => u.CompanyId, // свойство-селектор объекта из первого набора
        c => c.Id, // свойство-селектор объекта из второго набора
        (u, c) => new // результат
        {
            Name = u.Name,
            Company = c.Name,
            Age = u.Age
        });
    foreach (var u in usersJ) Console.WriteLine($"{u.Name} ({u.Company}) - {u.Age}");
    //users = from u in db.Users join c in db.Companies on u.CompanyId equals c.Id select new { Name = u.Name, Company = c.Name, Age = u.Age };
    //Соединение трех таблиц
    Console.WriteLine("Соединение таблиц Join3");
    var usersJ3 = from user in db.Users
                join company in db.Companies on user.CompanyId equals company.Id
                join country in db.Countries on company.CountryId equals country.Id
                select new
                {
                    Name = user.Name,
                    Company = company.Name,
                    Age = user.Age,
                    Country = country.Name
                };
    foreach (var u in usersJ3) Console.WriteLine($"{u.Name} ({u.Company} - {u.Country}) - {u.Age}");
    #endregion

    #region Группировка GroupBy()
    Console.WriteLine("Группировка GroupBy");
    var groups = db.Users.GroupBy(u => u.Company!.Name).Select(g => new
    {
        g.Key,
        Count = g.Count()
    });
    //или так:
    groups = from u in db.Users
                 group u by u.Company!.Name into g
                 select new
                 {
                     g.Key,
                     Count = g.Count()
                 };
    foreach (var group in groups) Console.WriteLine($"{group.Key} - {group.Count}");
    #endregion

    #region Union() Объединение
    Console.WriteLine("Union() объединение");
    users = await db.Users.Where(u => u.Age < 30).Union(db.Users.Where(u => u.Name!.Contains("Tom"))).ToListAsync();
    foreach (var user in users) Console.WriteLine(user.Name);
    //При этом мы не можем объединить две разнородные выборки, например, таблицу, пользователей и таблицу компаний. Однако уместна следующая запись:
    var result = db.Users.Select(p => new { Name = p.Name }).Union(db.Companies.Select(c => new { Name = c.Name }));
    #endregion

    #region Intersect() Пересечение выборок, то есть те элементы, которые присутствуют сразу в двух выборках
    Console.WriteLine("Intersect() Пересечение выборокс");
    users = await db.Users.Where(u => u.Age > 30).Intersect(db.Users.Where(u => u.Name!.Contains("Tom"))).ToListAsync();
    foreach (var user in users) Console.WriteLine(user.Name);
    #endregion

    #region Разность Except()
    //элементы первой выборки, которые отсутствуют во второй выборке
    Console.WriteLine("Разность Except()");
    var selector1 = db.Users.Where(u => u.Age > 30); // 
    var selector2 = db.Users.Where(u => u.Name.Contains("Tom")); // Samsung Galaxy S8, Samsung Galaxy S7
    users = await selector1.Except(selector2).ToListAsync(); // результат -  iUser 6S
    foreach (var user in users) Console.WriteLine(user.Name);
    #endregion

    #region Агрегатные операции
    Console.WriteLine("Агрегатные операции");
    //Метод Any() позволяет проверить, есть ли в базе данных элемент с определенными признаками, и если есть, то метод возвратит значение true.
    bool Result = db.Users.Any(u => u.Company.Name == "Google");
    //All() позволяет проверит, удовлетворяют ли все элементы в базе данных определенному критерию.
    Result = db.Users.All(u => u.Company.Name == "Microsoft");
    //Count() позволяет найти количество элементов в выборке
    int number1 = db.Users.Count();
    int number2 = db.Users.Count(u => u.Name.Contains("Tom"));
    Console.WriteLine(number1); Console.WriteLine(number2);
    //Min(), Max() и Average() для нахождения минимального, максимального и среднего значений по выборке
    int minAge = db.Users.Min(u => u.Age); // максимальный возраст
    int maxAge = db.Users.Max(u => u.Age); // средний возраст пользователей, которые работают в Microsoft
    double avgAge = db.Users.Where(u => u.Company.Name == "Microsoft").Average(p => p.Age);
    Console.WriteLine(minAge); Console.WriteLine(maxAge); Console.WriteLine(avgAge);
    //Sum() сумма значений    
    int sum1 = db.Users.Sum(u => u.Age);// суммарный возраст всех пользователей    
    int sum2 = db.Users.Where(u => u.Company.Name == "Microsoft").Sum(u => u.Age);// суммарный возраст тех, кто работает в Microsoft
    Console.WriteLine(sum1); Console.WriteLine(sum2);
    #endregion

    #region Отслеживание объектов и AsNoTracking
    Console.WriteLine("Отслеживание объектов и AsNoTracking");
    /*
        Запросы могут быть отслеживаемыми и неотслеживаемыми. По умолчанию все запросы, которые возвращают объекты классов моделей
    являются отслеживаемыми. Когда контекст данных извлекает данные из базы данных, Entity Framework помещает извлеченные объекты в кэш
    и отслеживает изменения, которые происходят с этими объектами вплоть до использования метода SaveChanges()/SaveChangesAsync(),
    который фиксирует все изменения в базе данных. Но нам не всегда необходимо отслеживать изменения. Например, нам надо просто
    вывести данные для просмотра.
     */
    users = await db.Users.AsNoTracking().ToListAsync();
    var userNT = await db.Users.AsNoTracking().FirstOrDefaultAsync();
    userNT.Age = 22;
    db.SaveChanges(); //изменения не сохраняться, так как запрос неотслеживается
    //Выполнение запросов
    var user1 = db.Users.FirstOrDefault();
    var user2 = db.Users.FirstOrDefault();
    Console.WriteLine($"Before User1: {user1.Name}   User2: {user2.Name}");
    user1.Name = "Bob";
    /*
        Так как запрос db.Users.FirstOrDefault() является отслеживаемым, то при получении данных, по ним будет создаваться объект user1, 
        который добавляется в контекст и начинает отслеживаться.Далее повторно вызывается данный запрос для получения объекта user2. 
        Этот запрос то же является отслеживаемым, поэтому EF увидит, что такой объект уже есть в контексте, он уже отслеживается, 
        и возвратит ссылку на тот же объект. Поэтому все изменения с переменной user1 затронут и переменную user2.
     */
    user1 = db.Users.FirstOrDefault();
    user2 = db.Users.AsNoTracking().FirstOrDefault();//не отслеживаем
    Console.WriteLine($"Before User1: {user1.Name}   User2: {user2.Name}");
    user1.Name = "Sam";
    /*
        С первым объектом user1 все по прежнему: он также попадает в контекст и отслеживается. А вот второй запрос теперь является неотслеживаемым,
        так как использует метод AsNoTracking. Поэтому для данных, полученных в результате этого запроса, будет создаваться новый объект, 
        который никак не будет связан с объектом user1. И изменения одного из этих объектов никак не повлияют на второй объект.
     */
    #endregion

    #region IEnumerable и IQueryable
    Console.WriteLine("IEnumerable и IQueryable");
    /*
        В процессе выполнения запросов LINQ to Entities мы может получать два объекта, которые предоставляют наборы данных: IEnumerable и 
        IQueryable. С одной стороны, интерфейс IQueryable наследуется от IEnumerable, поэтому по идее объект IQueryable это и есть также 
        объект IEnumerable. Но реальность несколько сложнее. Между объектами этих интерфейсов есть разница в плане функциональности, 
        поэтому они не взаимозаменяемы.
        
        IEnumerable - может перемещаться по этим данным только вперед, выполняется немедленно и полностью, поэтому получение данных
        приложением происходит быстро. IEnumerable загружает все данные, и если нам надо выполнить их фильтрацию, то сама фильтрация
        происходит на стороне клиента.

        IQueryable - предоставляет удаленный доступ к базе данных и позволяет перемещаться по данным как в прямом порядке от начала до конца,
        так и в обратном порядке. В процессе создания запроса, возвращаемым объектом которого является IQueryable, происходит оптимизация запроса.
        В итоге в процессе его выполнения тратится меньше памяти, меньше пропускной способности сети, но в то же время он может обрабатываться чуть
        медленнее, чем запрос, возвращающий объект IEnumerable.
        
        Если разработчику нужен весь набор возвращаемых данных, то лучше использовать IEnumerable, предоставляющий максимальную скорость. 
        Если же нам не нужен весь набор, а то только некоторые отфильтрованные данные, то лучше применять IQueryable.
     */

    //IEnumerable
    int id = 1;
    IEnumerable<User> userIEnum = db.Users;
    var usersIE = userIEnum.Where(p => p.Id > id).ToList();
    //Фильтрация результата, обозначенная с помощью метода Where(p => p.Id > id) будет идти уже после выборки из бд в самом приложении.
    
    //IQueryable
    id = 1;
    IQueryable<User> userIQuer = db.Users;
    var usersIQ = userIQuer.Where(p => p.Id > id).ToList();
    //все методы суммируются, запрос оптимизируется, и только потом происходит выборка из базы данных.
    //Это позволяет динамически создавать сложные запросы. Например, мы можем последовательно наслаивать в зависимости
    //от условий выражения для фильтрации:
    userIQuer = db.Users;
    userIQuer = userIQuer.Where(p => p.Id < 7);
    userIQuer = userIQuer.Where(p => p.Name == "Tom");
    usersIQ = userIQuer.ToList();
    #endregion

    #region Фильтры запросов уровня модели
    Console.WriteLine("Фильтры запросов уровня модели");
    /*
        Фильтры запросов уровня модели (Model-level query filters) позволяют определить предикат запроса LINQ непосредственно в метаданных модели
        (обычно в методе OnModelCreating контекста данных). Такие фильтры автоматически применяются к любым запросам LINQ, в которых используются
        классы, для которых определен фильтр.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasQueryFilter(u => u.Age > 17 && u.RoleId == this.RoleId);
    }

    В метод HasQueryFilter() передается предикат, которому должен удовлетворять объект User, чтобы быть извлеченным из базы данных.
    То есть в результате запросов будут извлекаться только те объекты User, у которых значение свойства Age больше 17, а свойство RoleId
    равно значению свойства RoleId их контекста данных.

    При запросе будут учитываться только те объекты в бд, которые соответствуют фильтру.

    Если необходимо во время запроса отключить фильтр, то применяется метод IgnoreQueryFilters():
    int minAge = db.Users.IgnoreQueryFilters().Min(x => x.Age);
     */

    #endregion
}
