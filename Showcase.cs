using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lab7
{
    internal class Showcase<Item> : IEnumerable<BakeryProduct>,
        IShowсaseManeger<Item,BakeryProduct>
        where Item : BakeryProduct
    {
        public Item[,] Products { get; set; }
        private int _height;
        private int _width;
        public int Height 
        {
            get { return _height; }
            set { _height = value; }
        }
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public Showcase (int height, int width)
        {
            Width = width;
            Height = height;
            Products = new Item[height, width];
        }
        
        public bool AddItem(Item product)
        {
            for(int i = 0; i < Height; i++)
            {
                for(int k = 0; k < Width; k++)
                {
                    if (Products[i, k] == null)
                    {
                        Products[i, k] = product;
                        return true;
                    }
                    else continue;
                }
            }
            Console.WriteLine($"Витрина заполнена, {product.Name} не добавлен");
            return false;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Products[i, j] != null)
                    {
                        result += $"{Products[i, j].ToString()}\t"; // Добавляем элемент и табуляцию для лучшего отображения
                    }
                    else
                    {
                        result += "Empty\t";  // Если элемент null, выводим "Empty"
                    }
                }
                result += "\n";  // Добавляем перенос строки для каждой строки массива
            }
            return result;
        }

        public IEnumerator<BakeryProduct> GetEnumerator()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Products[i, j] != null)
                    {
                        yield return Products[i, j];
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Delete(Item itemToDelete)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Products[i, j] != null && Products[i, j].Name == itemToDelete.Name)
                    {
                        Products[i, j] = null;
                        Console.WriteLine($"Продукт {itemToDelete.Name} был удален из витрины.");
                        return;
                    }
                }
            }
            Console.WriteLine($"Продукт {itemToDelete.Name} не найден в витрине.");
        }

        public BakeryProduct Get(string name)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Products[i, j].Name != null && Products[i, j].Name == name)
                    {
                        return Products[i, j]; 
                    }
                }
            }
            return null;
        }

        public IEnumerable<BakeryProduct> GetAll()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Products[i, j] != null)
                    {
                        yield return Products[i, j]; // Возвращаем BakeryProduct или производные
                    }
                }
            }
        }

        public async Task BubbleSortAsync()
        {
            var consoleLogger = new Logger<string>(new ConsoleLoger());
            var fileLogger = new Logger<string>(new FileLoger("log_sort.txt"));

            await consoleLogger.LogAsync("Старт сортировки");
            await fileLogger.LogAsync("Старт сортировки");

            int rows = this.Height;  // Число строк
            int cols = this.Width;   // Число столбцов

            List<Item> sortingList = new List<Item>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sortingList.Add(Products[i, j]);
                }
            }

            int sortedCount = 0;
            int n = sortingList.Count;
            int numOfNulls = sortingList.Count(x => x == null);
            int range = sortingList.Count - numOfNulls - 1;

            for (int i = range; i > -1; i--)
            {
                decimal maxPrice = 0;
                decimal currentPrice;
                Item maxPriceItem = sortingList[0];
                for (int j = 0; j < range-i; j++)
                {
                    TryGetPrice(sortingList[j], out currentPrice);
                    if (maxPrice < currentPrice) 
                    { 
                        maxPrice = currentPrice;
                        maxPriceItem = sortingList[j];
                    }
                }
                (sortingList[i], maxPriceItem) = (maxPriceItem, sortingList[i]);
                sortedCount++;
                await consoleLogger.LogAsync($"Отсортировано элементов: {sortedCount}");
                await fileLogger.LogAsync($"Отсортировано элементов: {sortedCount}");
            }
        }

        public bool TryGetPrice(BakeryProduct product, out decimal price)
        {
            price = 0;

            // Проверяем, является ли продукт одним из наследников BakeryProduct
            if (product is Bread<object> bread && ConvertToDecimal(bread.Price, out price))
                return true;
            else if (product is Cake<object> cake && ConvertToDecimal(cake.Price, out price))
                return true;
            else if (product is Samsa<object> samsa && ConvertToDecimal(samsa.Price, out price))
                return true;
            else if (product is Cookie<object> cookie && ConvertToDecimal(cookie.Price, out price))
                return true;
            else if (product is Pie<object> pie && ConvertToDecimal(pie.Price, out price))
                return true;

            return false;
        }

        public bool ConvertToDecimal(object priceValue, out decimal result)
        {
            result = 0;

            try
            {
                result = Convert.ToDecimal(priceValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    class ShowcaseJsonConverter<Item> : JsonConverter<Showcase<Item>> where Item : BakeryProduct
    {
        public override Showcase<Item> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;

            int height = jsonObject.GetProperty("Height").GetInt32();
            int width = jsonObject.GetProperty("Width").GetInt32();

            var showcase = new Showcase<Item>(height, width);

            var productsArray = jsonObject.GetProperty("Products").EnumerateArray();
            int i = 0;

            foreach (var row in productsArray)
            {
                var columnArray = row.EnumerateArray();
                int j = 0;

                foreach (var cell in columnArray)
                {
                    if (cell.ValueKind != JsonValueKind.Null)
                    {
                        var bakeryProduct = JsonSerializer.Deserialize<Item>(cell.GetRawText(), options);
                        showcase.Products[i, j] = bakeryProduct;
                    }
                    j++;
                }
                i++;
            }

            return showcase;
        }

        public override void Write(Utf8JsonWriter writer, Showcase<Item> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber("Height", value.Height);
            writer.WriteNumber("Width", value.Width);

            writer.WritePropertyName("Products");
            writer.WriteStartArray();

            for (int i = 0; i < value.Height; i++)
            {
                writer.WriteStartArray();
                for (int j = 0; j < value.Width; j++)
                {
                    var product = value.Products[i, j];
                    if (product != null)
                    {
                        JsonSerializer.Serialize(writer, product, options);
                    }
                    else
                    {
                        writer.WriteNullValue();
                    }
                }
                writer.WriteEndArray();
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
