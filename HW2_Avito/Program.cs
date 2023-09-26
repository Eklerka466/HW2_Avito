using System.Globalization;
using HW2_Avito.Entities;

namespace HW2_Avito
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InitDataBase();
            var commands = new Dictionary<String, Action<String[]>>
                            {
                                { "view", View },
                                { "add", Add },
                                { "delete", Delete }
                            };
            var userCommand = "";
            try
            {
                while (!userCommand.StartsWith("exit"))
                {
                    var splits = userCommand.Split(' ');
                    var command = splits[0];
                    var param = splits.Length > 1 ? splits.Skip(1).ToArray() : new String[0];
                    if (!commands.ContainsKey(command))
                        Console.WriteLine($"Доступны следующие команды {String.Join(", ", commands.Keys)}, exit");
                    else
                        commands[command](param);
                    userCommand = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Что-то пошло не так, а именно: {ex.Message}");
            }
            finally
            {
                using (var dbContext = new DataContext())
                    dbContext.Database.EnsureDeleted();
                Logger.Dispose();
            }
        }

        public static void InitDataBase()
        {
            var users = new[]
            {
                new User { Name = "Ананастя", Email = "ananastya@avito.ru", Age = 20 },
                new User { Name = "Иннокентий", Email = "innokentiy@avito.ru", Age = 56 },
                new User { Name = "Дмитрий", Email = "dmitriy@avito.ru", Age = 26 },
                new User { Name = "Ирина", Email = "irina@avito.ru", Age = 34 },
                new User { Name = "Светлана", Email = "svetlana@avito.ru", Age = 42 }
            };

            var goods = new[]
            {
                new Goods { Name = "Кандибобер", Description = "Восхитительный головной убор, подходит женщинам и балеринам", GoodsType = GoodsType.Product, Price = 5000.00 },
                new Goods { Name = "Посидеть на пеньке", Description = "Отличная замена недели отдыха в all inclusive", GoodsType = GoodsType.Service, Price = 1000.00 },
                new Goods { Name = "Борщ", Description = "С капусткой, но не красный", GoodsType = GoodsType.Product, Price = 400.00 },
                new Goods { Name = "Дирижабль", Description = "Летает так же хорошо как ваши фантазии, ага", GoodsType = GoodsType.Product, Price = 1000000.00 },
                new Goods { Name = "Рыбы", Description = "Не на продажу, только показываем", GoodsType = GoodsType.Service, Price = 200.00 }
            };

            var purchases = new List<Purchase>();
            var rand = new Random((Int32)(DateTime.Now.Ticks % Int32.MaxValue));
            for (var i = 0; i < rand.Next(5, 10); i++)
                purchases.Add(new Purchase(users[rand.Next(0, 4)], goods[rand.Next(0, 4)],DateTime.Now.AddMinutes(rand.Next(-100, -10))));

            using (var dbContext = new DataContext())
            {
                dbContext.Users.AddRange(users);
                dbContext.Goods.AddRange(goods);
                dbContext.Purchases.AddRange(purchases);
                dbContext.SaveChanges();
            }
        }

        public static void View(String[] strings)
        {
            String tableName;
            if (strings.Length == 0)
            {
                Console.WriteLine("Введите название таблицы (доступны Users, Goods, Purchases):");
                tableName = Console.ReadLine();
            }
            else
                tableName = strings[0];
            var type = tableName == "Users" ? typeof(User) : tableName == "Goods" ? typeof(Goods) : typeof(Purchase);
            type.GetMethod("View").Invoke(null, null);
        }

        public static void Add(String[] strings)
        {
            String tableName;
            if (strings.Length == 0)
            {
                Console.WriteLine("Введите название таблицы (доступны Users, Goods, Purchases):");
                tableName = Console.ReadLine();
            }
            else
                tableName = strings[0];
            var type = tableName == "Users" ? typeof(User) : tableName == "Goods" ? typeof(Goods) : typeof(Purchase);
            type.GetMethod("Add").Invoke(null, null);
        }

        public static void Delete(String[] strings)
        {
            String tableName;
            if (strings.Length == 0)
            {
                Console.WriteLine("Введите название таблицы (доступны Users, Goods, Purchases):");
                tableName = Console.ReadLine();
            }
            else
                tableName = strings[0];
            Int32 id;
            if (strings.Length < 2)
            {
                Console.WriteLine("Введите название id удаляемого элемента:");
                if (!Int32.TryParse(Console.ReadLine(), NumberStyles.None, CultureInfo.InvariantCulture, out id))
                    id = -1;
            }
            else
                if (!Int32.TryParse(strings[1], NumberStyles.None, CultureInfo.InvariantCulture, out id))
                id = -1;
            var type = tableName == "Users" ? typeof(User) : tableName == "Goods" ? typeof(Goods) : typeof(Purchase);
            type.GetMethod("Delete").Invoke(null, new Object?[] { id });
        }
    }
}