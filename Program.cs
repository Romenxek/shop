using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Lab7
{
    internal partial class Program
    {
        static async Task Main(string[] args)
        {
            Shop testShop = new Shop();
            LoadFunction("config.txt", testShop);
            //CreateXmlShowcase("showcase.xml", testShop.CurrentShowcase);
            Showcase<BakeryProduct> scFromXml = DeserializeXmlShowcase<BakeryProduct>("showcase.xml");
            
            testShop.CurrentShowcase = scFromXml;
            var options = new JsonSerializerOptions
            {
                Converters = { new BakeryProductJsonConverter(), new ShowcaseJsonConverter<BakeryProduct>() },
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(testShop.CurrentShowcase, options);
            
            File.WriteAllText("showcase.json", json);
            testShop.CurrentShowcase = JsonSerializer.Deserialize<Showcase<BakeryProduct>>(json, options);
            
            var consoleLogger = new Logger<string>(new ConsoleLoger());
            var fileLogger = new Logger<string>(new FileLoger("log.txt"));

            var task1 = consoleLogger.LogAsync("Начало работы программы");
            var task2 = fileLogger.LogAsync("Начало работы программы");
            _ = task2.ContinueWith(async (t) => //сообщаем о том, что лог совершён
            {
                await consoleLogger.LogAsync("Запись в файл завершена");
            });
            
            string validNumbers = "0123456789"; //для switch'а

            /*Bread<int> testBread = new Bread<int>(testShop, "a", 2, Bread<int>.BreadType.White,
                7, new CalorieContent(2, 0, 20));

            Cake<double> testCake = new Cake<double>(testShop, "b", 2.1d, Cake<double>.CakeType.Napoleon,
                3, new CalorieContent(2, 10, 60));
           
            Action<BakeryProduct> addItem = product => testShop.CurrentShowcase.AddItem(product);
            addItem(testCake); addItem(testBread);
            */
            bool needToUpdateScreen = true; // Флаг для обновления экрана
            while (true)
            {
                if (needToUpdateScreen)
                {
                    Console.WriteLine($"\nВремя в магазине: {testShop.CurrentTime}" +
                                      $"\nПродавец магазина: {testShop.Seller.Name} " +
                                      $"\nработает с: {testShop.Seller.StartWorkingTime} " +
                                      $"\nработает до: {testShop.Seller.EndWorkingTime}\n");
                    Console.WriteLine("Увеличить время магазина: 1" +
                                      "\nУменьшить время магазина: 2" +
                                      "\nПоказать витрину: 3"+
                                      $"\nДолжен ли сейчас работать продавец {testShop.Seller.Name}?: 4" +
                                      "\nОтсортировать кассу по ценам: 5");                 
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                char chose = keyInfo.KeyChar;
                switch (chose)
                {
                    case '1':
                        Console.WriteLine("\nНа сколько часов увеличить время:");
                        int addedHours = Int32.Parse(Console.ReadLine());
                        testShop.UpperTheTime(addedHours);
                        await consoleLogger.LogAsync("Увеличено время магазина");
                        if (task2.IsCanceled) //если всё ещё лог в файл не завершён, то ждём, иначе - запускаем запись
                        {
                            var task = fileLogger.LogAsync("Увеличено время магазина");
                            await task;
                        }
                        else { await task2; await fileLogger.LogAsync("Увеличено время магазина");}
                        needToUpdateScreen = true; // Устанавливаем флаг для обновления экрана
                        continue;

                    case '2':
                        Console.WriteLine("\nНа сколько часов уменьшить время:");
                        int subtractedHours = Int32.Parse(Console.ReadLine());
                        testShop.LowerTheTime(subtractedHours);
                        await consoleLogger.LogAsync("Уменьшено время магазина");
                        if (task2.IsCanceled) //если всё ещё лог в файл не завершён, то ждём, иначе - запускаем запись
                        {
                            var task = fileLogger.LogAsync("Уменьшено время магазина");
                            await task;
                        }
                        else { await task2; await fileLogger.LogAsync("Уменьшено время магазина"); }
                        needToUpdateScreen = true; // Устанавливаем флаг для обновления экрана
                        continue;

                    case '3':
                        Console.WriteLine();
                        foreach (var product in testShop.CurrentShowcase)
                        {
                            Console.WriteLine(product.ToString());
                        }
                        needToUpdateScreen = true; // Устанавливаем флаг для обновления экрана
                        continue;

                    case '4':
                        Console.WriteLine(testShop.Seller.должен_ли_работать ? "\nДа, должен" : "\nНет, не должен");
                        needToUpdateScreen = true; // Устанавливаем флаг для обновления экрана
                        continue;
                    case '5':
                        await testShop.CurrentShowcase.BubbleSortAsync();
                        needToUpdateScreen = true; // Устанавливаем флаг для обновления экрана
                        continue;
                    default:
                        if (validNumbers.Contains(chose))
                        {
                            Console.WriteLine("Введите число из списка");
                        }
                        else Console.WriteLine("Введите число");
                        needToUpdateScreen = false; // Экран не нужно обновлять
                        continue;
                }
            }
        }
    }
}
