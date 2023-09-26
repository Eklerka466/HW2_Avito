using System.Globalization;

namespace HW2_Avito.Entities
{
    public class User
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public Int32 Age { get; set; }

        public List<Purchase> Purchases { get; set; } = new();

        public override string ToString()
            => $"Id: {Id}, Name: {Name}, Email: {Email}, Age: {Age}";

        public static void Add()
        {
            Console.WriteLine("Введите имя нового пользователя:");
            var name = Console.ReadLine();
            if (String.IsNullOrEmpty(name))
                throw new ApplicationException("Имя пользователя не заполнено");
            Console.WriteLine("Введите email нового пользователя:");
            var email = Console.ReadLine();
            if (String.IsNullOrEmpty(email))
                throw new ApplicationException("Почта пользователя не заполнено");
            Console.WriteLine("Введите возраст нового пользователя:");
            if (!Int32.TryParse(Console.ReadLine(), NumberStyles.None, CultureInfo.InvariantCulture, out var age))
                age = -1;//упадёт на записи в бд
            var user =  new User { Name = name, Email = email, Age = age };
            using (var dbContect = new DataContext())
            {
                dbContect.Users.Add(user);
                dbContect.SaveChanges();
            }
            Console.WriteLine("Пользователь успешно добавлен");
        }

        public static void View()
        {
            using (var dbContext = new DataContext())
                Console.WriteLine(String.Join("\n", dbContext.Users.Select(user => user.ToString())));
        }

        public static void Delete(Int32 id)
        {
            using (var dbContext = new DataContext())
            {
                var item = dbContext.Users.FirstOrDefault(user => user.Id == id);
                if (item != null)
                {
                    dbContext.Users.Remove(item);
                    dbContext.SaveChanges();
                    Console.WriteLine("Пользователь удалён");
                }
                else
                    Console.WriteLine("Пользователя с таким id не существует");
            }
        }
    }
}