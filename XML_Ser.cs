using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lab7
{
    internal partial class Program
    {
        public static void CreateXmlShowcase<Item>(string filepath, Showcase<Item> showcase)
            where Item : BakeryProduct
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmldecl);

            XmlElement root = xmlDoc.CreateElement("Showcase");
            xmlDoc.AppendChild(root);

            XmlElement height = xmlDoc.CreateElement("height");
            height.InnerText = showcase.Height.ToString();
            root.AppendChild(height);

            XmlElement width = xmlDoc.CreateElement("width");
            width.InnerText = showcase.Width.ToString();
            root.AppendChild(width);

            XmlElement products = xmlDoc.CreateElement("Products");
            root.AppendChild(products);

            for (int i = 0; i < showcase.Height; i++)
            {
                for (int j = 0; j < showcase.Width; j++)
                {
                    Item product = showcase.Products[i, j];
                    if (product != null)
                    {
                        XmlElement productElement = xmlDoc.CreateElement("Product");

                        XmlElement productType = xmlDoc.CreateElement("BakeryType");
                        productType.InnerText = GetReadableTypeName(product.GetType());
                        productElement.AppendChild(productType);

                        if (product is Cake<object> cake)
                        {
                            XmlElement cakeType = xmlDoc.CreateElement("CakeType");
                            cakeType.InnerText = cake.Type.ToString();
                            productElement.AppendChild(cakeType);
                        }
                        else if (product is Bread<object> bread)
                        {
                            XmlElement breadType = xmlDoc.CreateElement("BreadType");
                            breadType.InnerText = bread.Type.ToString();
                            productElement.AppendChild(breadType);
                        }
                        else if (product is Samsa<object> samsa)
                        {
                            XmlElement meatType = xmlDoc.CreateElement("MeatType");
                            meatType.InnerText = samsa.Type.ToString();
                            productElement.AppendChild(meatType);
                        }
                        else if (product is Cookie<object> cookie)
                        {
                            XmlElement cookieType = xmlDoc.CreateElement("CookieType");
                            cookieType.InnerText = cookie.Type.ToString();
                            productElement.AppendChild(cookieType);
                        }
                        else if (product is Pie<object> pie)
                        {
                            XmlElement fillingType = xmlDoc.CreateElement("FillingType");
                            fillingType.InnerText = pie.Type.ToString();
                            productElement.AppendChild(fillingType);
                        }

                        XmlElement name = xmlDoc.CreateElement("Name");
                        name.InnerText = product.Name;
                        productElement.AppendChild(name);

                        XmlElement expDate = xmlDoc.CreateElement("ExpirationDate");
                        expDate.InnerText = product.ExpirationDateInt.ToString();
                        productElement.AppendChild(expDate);

                        XmlElement price = xmlDoc.CreateElement("Price");
                        if (showcase.TryGetPrice(product, out decimal productPrice))
                        {
                            string pPrice = productPrice.ToString().Replace(",",".");
                            price.InnerText = pPrice;
                        }
                        productElement.AppendChild(price);

                        XmlElement calorie = xmlDoc.CreateElement("Calories");

                            XmlElement protein = xmlDoc.CreateElement("Protein");
                            protein.InnerText = product.Calorie.Protein.ToString();
                            calorie.AppendChild(protein);
                            XmlElement fats = xmlDoc.CreateElement("Fats");
                            fats.InnerText = product.Calorie.Fats.ToString();
                            calorie.AppendChild(fats);
                            XmlElement carbs = xmlDoc.CreateElement("Carbs");
                            carbs.InnerText = product.Calorie.Carbs.ToString();
                            calorie.AppendChild(carbs);

                        productElement.AppendChild(calorie);

                        products.AppendChild(productElement);
                    }
                }
            }
            xmlDoc.Save(filepath);
        }

        public static string GetReadableTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                var typeName = type.Name;
                var backtickIndex = typeName.IndexOf('`');
                if (backtickIndex > 0)
                {
                    typeName = typeName.Substring(0, backtickIndex);
                }
                return typeName;
            }
            return type.Name;
        }
    }
}
