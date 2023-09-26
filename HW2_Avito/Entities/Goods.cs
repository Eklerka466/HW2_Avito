using System.Globalization;

namespace HW2_Avito.Entities
{
    public class Goods
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Double Price { get; set; }
        public GoodsType GoodsType { get; set; }

        public List<Purchase> Purchases { get; set; } = new();

        public override string ToString()
            => $"Id: {Id}, Name: {Name}, Description: {Description}, Price: {Price}, GoodsType: {GoodsType}";

        public static void Add()
        {
            Console.WriteLine("Введите P, если хотите создать товар или S, если хотите создать услугу:");
            var type = Console.ReadLine().ToLower().Contains("p") ? GoodsType.Product : GoodsType.Service;
            Console.WriteLine("Введите наименовние товара или услуги:");
            var name = Console.ReadLine();
            if (String.IsNullOrEmpty(name))
                throw new ApplicationException("Наименовние не заполнено");
            Console.WriteLine("Введите описание товара или услуги:");
            var description = Console.ReadLine();
            if (String.IsNullOrEmpty(description))
                throw new ApplicationException("Описание не заполнено");
            Console.WriteLine("Введите стоимость:");
            var email = Console.ReadLine();
            if (!Double.TryParse(Console.ReadLine(), NumberStyles.None, CultureInfo.InvariantCulture, out var price))
                price = -1;//упадет при записи в бд
            var goods =  new Goods { Name = name, Description = description, Price = price, GoodsType = type };
            using (var dbContext = new DataContext())
            {
                dbContext.Goods.Add(goods);
                dbContext.SaveChanges();
            }
            Console.WriteLine("Товар\\услуга успешно добавлена");
        }

        public static void View()
        {
            using (var dbContext = new DataContext())
                Console.WriteLine(String.Join("\n", dbContext.Goods.Select(goods => goods.ToString())));
        }

        public static void Delete(Int32 id)
        {
            using (var dbContext = new DataContext())
            {
                var item = dbContext.Goods.FirstOrDefault(goods => goods.Id == id);
                if (item != null)
                {
                    dbContext.Goods.Remove(item);
                    dbContext.SaveChanges();
                    Console.WriteLine("Товар\\услуга удалена");
                }
                else
                    Console.WriteLine("Товара\\услуги с таким id не существует");
            }
        }

    }

    public enum GoodsType
    {
        Product,
        Service
    }
}