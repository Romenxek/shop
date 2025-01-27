using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Lab7
{
    internal partial class Program
    {
        class BakeryProductJsonConverter : JsonConverter<BakeryProduct>
        {
            public override BakeryProduct Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
                var typeProperty = jsonObject.GetProperty("ClassType").GetString();
                var type = Type.GetType(typeProperty);

                if (type == null)
                {
                    throw new JsonException($"Unknown type: {typeProperty}");
                }

                BakeryProduct product = (BakeryProduct)Activator.CreateInstance(type);

                foreach (var property in jsonObject.EnumerateObject())
                {
                    if (property.Name != "ClassType")  // Исключаем "ClassType", так как это не свойство
                    {
                        var propertyInfo = type.GetProperty(property.Name);
                        if (propertyInfo != null && propertyInfo.CanWrite)
                        {
                            var value = JsonSerializer.Deserialize(property.Value.GetRawText(), propertyInfo.PropertyType, options);
                            propertyInfo.SetValue(product, value);
                        }
                    }
                }
                return product;
            }

            public override void Write(Utf8JsonWriter writer, BakeryProduct value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteString("ClassType", value.GetType().FullName);
                foreach (var property in value.GetType().GetProperties())
                {
                    var propValue = property.GetValue(value);
                    writer.WritePropertyName(property.Name);
                    JsonSerializer.Serialize(writer, propValue, options);
                }

                writer.WriteEndObject();
            }
        }
    }
}