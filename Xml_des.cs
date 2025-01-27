using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab7
{
    internal partial class Program
    {
        public static Showcase<Item> DeserializeXmlShowcase<Item>(string filepath)
        where Item : BakeryProduct
        {
            var xmlDoc = XDocument.Load(filepath);
            var root = xmlDoc.Element("Showcase");

            int height = int.Parse(root.Element("height").Value);
            int width = int.Parse(root.Element("width").Value);

            var showcase = new Showcase<Item>(height, width);

            var productElements = root.Element("Products").Elements("Product");
            foreach (var productElement in productElements)
            {
                string bakeryType = productElement.Element("BakeryType").Value;

                BakeryProduct product;

                switch (bakeryType)
                {
                    case "Bread":
                        var breadType = (Bread<object>.BreadType)Enum.Parse(typeof(Bread<object>.BreadType), productElement.Element("BreadType").Value);
                        product = new Bread<object>(
                            productElement.Element("Name").Value,
                            BakeryProductFactory.ParsePrice(productElement.Element("Price").Value),
                            breadType,
                            int.Parse(productElement.Element("ExpirationDate").Value),
                            new CalorieContent(
                                int.Parse(productElement.Element("Calories").Element("Protein").Value),
                                int.Parse(productElement.Element("Calories").Element("Fats").Value),
                                int.Parse(productElement.Element("Calories").Element("Carbs").Value)
                            )
                        );
                        break;

                    case "Cake":
                        var cakeType = (Cake<object>.CakeType)Enum.Parse(typeof(Cake<object>.CakeType), productElement.Element("CakeType").Value);
                        product = new Cake<object>(
                            productElement.Element("Name").Value,
                            BakeryProductFactory.ParsePrice(productElement.Element("Price").Value),
                            cakeType,
                            int.Parse(productElement.Element("ExpirationDate").Value),
                            new CalorieContent(
                                int.Parse(productElement.Element("Calories").Element("Protein").Value),
                                int.Parse(productElement.Element("Calories").Element("Fats").Value),
                                int.Parse(productElement.Element("Calories").Element("Carbs").Value)
                            )
                        );
                        break;

                    case "Samsa":
                        var meatType = (Samsa<object>.MeatType)Enum.Parse(typeof(Samsa<object>.MeatType), productElement.Element("MeatType").Value);
                        product = new Samsa<object>(
                            productElement.Element("Name").Value,
                            BakeryProductFactory.ParsePrice(productElement.Element("Price").Value),
                            meatType,
                            int.Parse(productElement.Element("ExpirationDate").Value),
                            new CalorieContent(
                                int.Parse(productElement.Element("Calories").Element("Protein").Value),
                                int.Parse(productElement.Element("Calories").Element("Fats").Value),
                                int.Parse(productElement.Element("Calories").Element("Carbs").Value)
                            )
                        );
                        break;

                    case "Cookie":
                        var cookieType = (Cookie<object>.CookieType)Enum.Parse(typeof(Cookie<object>.CookieType), productElement.Element("CookieType").Value);
                        product = new Cookie<object>(
                            productElement.Element("Name").Value,
                            BakeryProductFactory.ParsePrice(productElement.Element("Price").Value),
                            cookieType,
                            int.Parse(productElement.Element("ExpirationDate").Value),
                            new CalorieContent(
                                int.Parse(productElement.Element("Calories").Element("Protein").Value),
                                int.Parse(productElement.Element("Calories").Element("Fats").Value),
                                int.Parse(productElement.Element("Calories").Element("Carbs").Value)
                            )
                        );
                        break;

                    case "Pie":
                        var fillingType = (Pie<object>.FillingType)Enum.Parse(typeof(Pie<object>.FillingType), productElement.Element("FillingType").Value);
                        product = new Pie<object>(
                            productElement.Element("Name").Value,
                            BakeryProductFactory.ParsePrice(productElement.Element("Price").Value),
                            fillingType,
                            int.Parse(productElement.Element("ExpirationDate").Value),
                            new CalorieContent(
                                int.Parse(productElement.Element("Calories").Element("Protein").Value),
                                int.Parse(productElement.Element("Calories").Element("Fats").Value),
                                int.Parse(productElement.Element("Calories").Element("Carbs").Value)
                            )
                        );
                        break;

                    default:
                        throw new InvalidOperationException($"Unsupported bakery type: {bakeryType}");
                }
                showcase.AddItem((Item)product);
            }
            return showcase;
        }
    }
}
