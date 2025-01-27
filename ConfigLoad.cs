using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Lab7
{
    internal partial class Program
    {
        static void LoadFunction(string fileName, Shop shop)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, fileName);
            if (!File.Exists(filePath)) 
            { 
                Console.WriteLine("Начальные параметры установлены на:\n" +
                    "Продавец: Vitalik, работает с 0 до 1\n" +
                    "Витрина:\n1 товар: \"b\", торт Наполеон, цена: 2.1," +
                    " просрочен через 3 дня, калории: 2, 10, 60\n" +
                    "2 товар: \"a\", белый хлеб, цена: 2," +
                    " просрочен через 7 дней, калории : 2, 0, 20");
                string message = "Vitalik 0 1\n" +
                    "Cake \"b\" Napoleon 2.1 3 2 10 60\n" +
                    "Bread \"a\" White 2 7 2 0 20\n" +
                    "Pie \"c\" Jam 5 5 5 5 10\n" +
                    "Pie \"c\" Jam 11 6 1 2 10";
                File.WriteAllText(filePath, message);
                ReadConfig(fileName, shop);
                foreach(BakeryProduct product in shop.CurrentShowcase)
                {
                    product.UpdateExpirationStatus(shop);
                }
                return;
            }
            else
            {
                ReadConfig(fileName, shop);
                foreach (BakeryProduct product in shop.CurrentShowcase)
                {
                    product.UpdateExpirationStatus(shop);
                }
                return;
            }
        }

        static void ReadConfig(string fileName, Shop shop) 
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(filePath, fileName);
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] parts = line.Split(' ');
                    try
                    {
                        if (parts.Length == 3)
                            {
                                string name = parts[0];
                                int startWorkingTime = int.Parse(parts[1]);
                                int endWorkingTime = int.Parse(parts[2]);
                                shop.Seller = new Seller(shop, name, startWorkingTime, endWorkingTime);
                            }
                        else if (parts.Length == 8)
                            {
                                // Определяем тип BakeryProduct по имени в parts[0]
                                string productType = parts[0];
                                BakeryProduct bakeryProduct = null;

                                switch (productType)
                                {
                                    case "Bread":
                                        bakeryProduct = BakeryProductFactory.BPCreate<Bread<object>>(parts);
                                        break;
                                    case "Cake":
                                        bakeryProduct = BakeryProductFactory.BPCreate<Cake<object>>(parts);
                                        break;
                                    case "Samsa":
                                        bakeryProduct = BakeryProductFactory.BPCreate<Samsa<object>>(parts);
                                        break;
                                    case "Cookie":
                                        bakeryProduct = BakeryProductFactory.BPCreate<Cookie<object>>(parts);
                                        break;
                                    case "Pie":
                                        bakeryProduct = BakeryProductFactory.BPCreate<Pie<object>>(parts);
                                        break;
                                    default:
                                        throw new ArgumentException($"Такого типа нет: {productType}");
                                }
                                shop.CurrentShowcase.AddItem(bakeryProduct);

                            }
                        else if (parts.Length != 0)
                        {
                            Console.WriteLine("Одна из строк записана не по формату");
                            throw new InvalidStringException($"{string.Join(" ", parts)} записана не по формату\n\n");
                        }

                    }
                    catch(InvalidStringException ex)
                    {
                        Console.WriteLine(ex.Message);

                        // Дополнение до трех элементов
                        while (parts.Length < 3)
                        {
                            Array.Resize(ref parts, parts.Length + 1);
                            parts[parts.Length - 1] = "0"; // Добавляем "0" как значение по умолчанию
                        }

                        try
                        {
                            string name = parts[0];
                            int startWorkingTime = int.Parse(parts[1]);
                            int endWorkingTime = int.Parse(parts[2]);
                            shop.Seller = new Seller(shop, name, startWorkingTime, endWorkingTime);
                        }
                        catch (FormatException formatEx)
                        {
                            Console.WriteLine($"Ошибка формата: {formatEx.Message}");
                        }
                    }
                    line = sr.ReadLine();
                }
            } 
        }
    }
}
