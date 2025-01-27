using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab7
{
    public enum BakeryType
    {
        Bread,
        Cake,
        Samsa,
        Cookie,
        Pie
    }
    internal abstract class BakeryProduct
    {
        public BakeryType BType { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public CalorieContent Calorie { get; set; }
        public int ExpirationDateInt { get; set; }
        private bool? _isExpiredStatus = null;
        public virtual bool? IsExpired
        {
            get => _isExpiredStatus;
        }
        public BakeryProduct(string name, int expirationDate, CalorieContent calorie)
        {
            Name = name;
            ExpirationDate = DateTime.Now.AddDays(expirationDate);
            Calorie = calorie;
            ExpirationDateInt = expirationDate;
        }
        public BakeryProduct() { }
        public void UpdateExpirationStatus(Shop shop)
        {
            _isExpiredStatus = shop.IsExpired(this);
        }
    }

    internal class Bread<T> : BakeryProduct
    {
        public enum BreadType {White, Black};
        public T Price { get; set; }
        public BreadType Type { get; set; }

        public Bread() { }
        public Bread(string name, T price, BreadType type, int expirationDate, CalorieContent calorie) :
            base(name, expirationDate, calorie)
        {
            Price = price;
            Type = type;
            BType = BakeryType.Bread;
        }

        public override string ToString()
        {
            if (IsExpired != null) { return $"Bread: name:{Name}, price:{Price}, bread type:{Type}, expiration date:{ExpirationDate}, {Calorie}, {IsExpired}"; }
            return $"Bread: name:{Name}, price:{Price}, bread type:{Type}, expiration date:{ExpirationDate}, {Calorie}";
        }
    }

    internal class Cake<T> : BakeryProduct
    {
        public enum CakeType
        {
            Napoleon,
            Muraveinik
        };
        public T Price { get; set; }
        public CakeType Type { get; set; }

        public override string ToString()
        {
            if (IsExpired != null) { return $"Cake: name:{Name}, price:{Price}, cake type:{Type}, expiration date:{ExpirationDate}, {Calorie}, {IsExpired}"; }
            return $"Cake: name:{Name}, price:{Price}, cake type:{Type}, expiration date:{ExpirationDate}, {Calorie}";
        }
        public Cake() { }
        public Cake(string name, T price, CakeType type, int expirationDate, CalorieContent calorie) :
            base(name, expirationDate, calorie) 
        {
            Price = price;
            Type = type;
            BType = BakeryType.Cake;
        }
    }

    internal class Samsa<T> : BakeryProduct
    {
        public enum MeatType
        {
            Beef,    
            Chicken  
        };
        public T Price { get; set; }
        public MeatType Type { get; set; }
        public Samsa() { }
        public Samsa(string name, T price, MeatType type, int expirationDate, CalorieContent calorie) :
            base(name, expirationDate, calorie) 
        {
            Price = price;
            Type = type;
            BType = BakeryType.Samsa;
        }

        public override string ToString()
        {
            if (IsExpired != null)
            {
                return $"Samsa: name:{Name}, price:{Price}, meat type:{Type}, expiration date:{ExpirationDate}, {Calorie}, {IsExpired}";
            }
            return $"Samsa: name:{Name}, price:{Price}, meat type:{Type}, expiration date:{ExpirationDate}, {Calorie}";
        }
    }

    internal class Cookie<T> : BakeryProduct
    {
        public enum CookieType
        {
            Rich,   // Сдобное
            Plain   // Не сдобное
        };
        public T Price { get; set; }
        public CookieType Type { get; set; }
        public Cookie () { }
        public Cookie(string name, T price, CookieType type,
            int expirationDate, CalorieContent calorie) :
            base(name, expirationDate, calorie)
        {
            Price = price;
            Type = type;
            BType = BakeryType.Cookie;
        }

        public override string ToString()
        {
            if (IsExpired != null)
            {
                return $"Cookie: name:{Name}, price:{Price}, cookie type:{Type}, expiration date:{ExpirationDate}, {Calorie}, {IsExpired}";
            }
            return $"Cookie: name:{Name}, price:{Price}, cookie type:{Type}, expiration date:{ExpirationDate}, {Calorie}";
        }
    }

    internal class Pie<T> : BakeryProduct
    {
        public enum FillingType
        {
            Jam,        // Повидло
            Preserves   // Варенье
        }
        public T Price { get; set; }
        public FillingType Type { get; set; } 
        public Pie () { }
        public Pie(string name, T price, FillingType type, int expirationDate, CalorieContent calorie) :
        base(name, expirationDate, calorie) // Передача параметра shop в базовый конструктор
        {
            Price = price;
            Type = type;
            BType = BakeryType.Pie;
        }

        public override string ToString()
        {
            if (IsExpired != null)
            {
                return $"Pie: name:{Name}, price:{Price}, filling:{Type}, expiration date:{ExpirationDate}, {Calorie}, {IsExpired}";
            }
            return $"Pie: name:{Name}, price:{Price}, filling:{Type}, expiration date:{ExpirationDate}, {Calorie}";    
        }
    }

    internal class BakeryProductFactory
    {
        public static BakeryProduct BPCreate<T>(string[] parts)
            where T : BakeryProduct
        {
            var price = ParsePrice(parts[3]);
            var calorie = new CalorieContent(int.Parse(parts[5]), int.Parse(parts[6]), int.Parse(parts[7]));
            int expirationDate = int.Parse(parts[4]);

            if (typeof(T) == typeof(Bread<object>))
            {
                var breadType = (Bread<object>.BreadType)Enum.Parse(typeof(Bread<object>.BreadType), parts[2]);
                return new Bread<object>(parts[1], price, breadType, expirationDate, calorie);
            }
            else if (typeof(T) == typeof(Cake<object>))
            {
                var cakeType = (Cake<object>.CakeType)Enum.Parse(typeof(Cake<object>.CakeType), parts[2]);
                return new Cake<object>(parts[1], price, cakeType, expirationDate, calorie);
            }
            else if (typeof(T) == typeof(Samsa<object>))
            {
                var meatType = (Samsa<object>.MeatType)Enum.Parse(typeof(Samsa<object>.MeatType), parts[2]);
                return new Samsa<object>(parts[1], price, meatType, expirationDate, calorie);
            }
            else if (typeof(T) == typeof(Cookie<object>))
            {
                var cookieType = (Cookie<object>.CookieType)Enum.Parse(typeof(Cookie<object>.CookieType), parts[2]);
                return new Cookie<object>(parts[1], price, cookieType, expirationDate, calorie);
            }
            else if (typeof(T) == typeof(Pie<object>))
            {
                var fillingType = (Pie<object>.FillingType)Enum.Parse(typeof(Pie<object>.FillingType), parts[2]);
                return new Pie<object>(parts[1], price, fillingType, expirationDate, calorie);
            }
            else
            {
                throw new ArgumentException("Invalid BakeryProduct type.");
            }
        }

        public static object ParsePrice(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentException("Input string cannot be null or whitespace.");

            // Попытка распознать целое число
            if (int.TryParse(str, out int intResult))
                return intResult;

            // Попытка распознать число с плавающей точкой двойной точности (без суффикса "d")
            if (double.TryParse(str, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double doubleResult))
            {
                // Проверка, является ли число целым при представлении как double
                if (doubleResult == Math.Truncate(doubleResult))
                {
                    return (int)doubleResult; // Вернуть как int, если оно не содержит дробной части
                }
                return doubleResult;
            }

            // Попытка распознать число с плавающей точкой одинарной точности (с суффиксом "f")
            if (str.EndsWith("f", StringComparison.OrdinalIgnoreCase) && float.TryParse(str.Substring(0, str.Length - 1), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float floatResult))
            {
                return floatResult;
            }

            // Попытка распознать число с суффиксом "d" как double
            if (str.EndsWith("d", StringComparison.OrdinalIgnoreCase) && double.TryParse(str.Substring(0, str.Length - 1), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double doubleWithSuffixResult))
            {
                return doubleWithSuffixResult;
            }

            throw new FormatException("Input string is not a valid number.");
        }
    }
}
