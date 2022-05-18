using Newtonsoft.Json;
using System;
using System.Xml;

namespace Xamariners.Utilities.Serialization
{
  /// <summary>Converter used to convert timespan to ISO8601 format</summary>
  public class Iso8601TimeSpanConverter : JsonConverter
  {
        /// <summary>Writes the specified object to JSON.</summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The value to serialize.</param>
        /// <param name="serializer">The JSON serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
          if (serializer == null)
            throw new ArgumentNullException(nameof (serializer));
          string str = XmlConvert.ToString((TimeSpan) value);
          serializer.Serialize(writer, (object) str);
         }

        /// <summary>Reads the JSON token.</summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The JSON serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
          if (reader == null)
            throw new ArgumentNullException(nameof (reader));
          if (serializer == null)
            throw new ArgumentNullException(nameof (serializer));
          return reader.TokenType == (JsonToken)11 ? (object) null : (object) XmlConvert.ToTimeSpan(serializer.Deserialize<string>(reader));
        }
        
      
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }
    }
}
