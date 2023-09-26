using System.Globalization;

namespace HW2_Avito.Entities
{
    public class Purchase
    {
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public User User { get; set; }
        public Int32 GoodsId { get; set; }
        public Goods Goods { get; set; }
        public DateTime Date { get; set; }

        public Purchase(User user, Goods goods, DateTime date)
        {
            User = user;
            UserId = user.Id;
            Goods = goods;
            GoodsId = goods.Id;
            Date = date.ToUniversalTime();
        }

        public Purchase(Int32 id, Int32 userId, Int32 goodsId, DateTime date)
        {
            Id = id;
            UserId = userId;
            GoodsId = goodsId;
            using (var dbContext = new DataContext())
            {
                User = dbContext.Users.FirstOrDefault(user => user.Id == userId);
                Goods = dbContext.Goods.FirstOrDefault(goods => goods.Id == goodsId);
            }
            Date = date;
        }

        public override string ToString()
            => $"Id: {Id}, UserId: {UserId}, User: ({User}), GoodsId: {GoodsId}, Goods: ({Goods}), Date: {Date:yyyy-MM-dd HH:mm:ss}";

        public static void Add()
        {
            Console.WriteLine("Введите id пользователя:");
            if (!Int32.TryParse(Console.ReadLine(), NumberStyles.None, CultureInfo.InvariantCulture, out var userId))
                userId = -1;
            Console.WriteLine("Введите id товара\\услуги:");
            if (!Int32.TryParse(Console.ReadLine(), NumberStyles.None, CultureInfo.InvariantCulture, out var goodsId))
                goodsId = -1;
            using (var dbContext = new DataContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    throw new ApplicationException("Пользователя с указанным id не существует");
                var goods = dbContext.Goods.FirstOrDefault(g => g.Id == goodsId);
                if (goods == null)
                    throw new ApplicationException("Товара\\услуги с указанным id не существует");
                var purchase = new Purchase(user, goods, DateTime.Now);
                dbContext.Purchases.Add(purchase);
                dbContext.SaveChanges();
            }
            Console.WriteLine("Покупка совершена");
        }

        public static void View()
        {
            using (var dbContext = new DataContext())
                Console.WriteLine(String.Join("\n", dbContext.Purchases.Select(purchase => purchase.ToString())));
        }

        public static void Delete(Int32 id)
        {
            using (var dbContext = new DataContext())
            {
                var item = dbContext.Purchases.FirstOrDefault(purchases => purchases.Id == id);
                if (item != null)
                {
                    dbContext.Purchases.Remove(item);
                    dbContext.SaveChanges();
                    Console.WriteLine("Покупка удалена");
                }
                else
                    Console.WriteLine("Покупки с таким id не существует");
            }
        }
    }
}