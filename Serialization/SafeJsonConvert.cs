using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Xamariners.Utilities.Serialization
{
  /// <summary>
  /// Provides an alternative to JSON.NET's JsonConvert that does not inherit any settings from
  /// JsonConvert.DefaultSettings.
  /// </summary>
  public static class SafeJsonConvert
  {
    /// <summary>Gets or sets the default serialization settings.</summary>
    public static JsonSerializerSettings SerializationSettings { get; set; }

    /// <summary>Gets or sets the default deserialization settings.</summary>
    public static JsonSerializerSettings DeserializationSettings { get; set; }

    /// <summary>
    /// Initializes the safe json convert if you want to use [this] default serialization settings.
    /// </summary>
    public static void InitializeSafeJsonConvert()
    {
      JsonSerializerSettings serializerSettings1 = new JsonSerializerSettings();
      serializerSettings1.Formatting = (Formatting) 1;
      serializerSettings1.DateFormatHandling = (DateFormatHandling) 0;
      serializerSettings1.DateTimeZoneHandling = (DateTimeZoneHandling) 1;
      serializerSettings1.NullValueHandling = (NullValueHandling) 1;
      serializerSettings1.ReferenceLoopHandling = (ReferenceLoopHandling) 1;
      serializerSettings1.ContractResolver = (IContractResolver) new ReadOnlyJsonContractResolver();
      List<JsonConverter> jsonConverterList1 = new List<JsonConverter>();
      jsonConverterList1.Add((JsonConverter) new Iso8601TimeSpanConverter());
      serializerSettings1.Converters = (IList<JsonConverter>) jsonConverterList1;
      SafeJsonConvert.SerializationSettings = serializerSettings1;
      JsonSerializerSettings serializerSettings2 = new JsonSerializerSettings();
      serializerSettings2.DateFormatHandling = (DateFormatHandling) 0;
      serializerSettings2.DateTimeZoneHandling = (DateTimeZoneHandling) 0;
      serializerSettings2.NullValueHandling = (NullValueHandling) 1;
      serializerSettings2.ReferenceLoopHandling = (ReferenceLoopHandling) 1;
      serializerSettings2.ContractResolver = (IContractResolver) new ReadOnlyJsonContractResolver();
      List<JsonConverter> jsonConverterList2 = new List<JsonConverter>();
      jsonConverterList2.Add((JsonConverter) new Iso8601TimeSpanConverter());
      serializerSettings2.Converters = (IList<JsonConverter>) jsonConverterList2;
      SafeJsonConvert.DeserializationSettings = serializerSettings2;
    }

    /// <summary>
    /// Deserializes the given JSON into an instance of type T.
    /// Notes: If you executed InitializeSafeJsonConvert(), it will use [this] default settings,
    /// otherwise it will use the JsonConvert default settings. Make sure the settings is properly
    /// setup or look at the code of InitializeSafeJsonConvert() to see what is set and unset.
    /// </summary>
    /// <typeparam name="T">The type to which to deserialize.</typeparam>
    /// <param name="json">The JSON to deserialize.</param>
    /// <param name="settings">JsonSerializerSettings to control deserialization.</param>
    /// <returns>An instance of type T deserialized from the given JSON.</returns>
    public static T DeserializeObject<T>(string json)
    {
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      JsonSerializer jsonSerializer = SafeJsonConvert.DeserializationSettings != null ? JsonSerializer.CreateDefault(SafeJsonConvert.DeserializationSettings) : JsonSerializer.CreateDefault();
      jsonSerializer.CheckAdditionalContent = true;
      using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) new StringReader(json)))
        return (T) jsonSerializer.Deserialize((JsonReader) jsonTextReader, typeof (T));
    }

    /// <summary>
    /// Deserializes the given JSON into an instance of type T.
    /// </summary>
    /// <typeparam name="T">The type to which to deserialize.</typeparam>
    /// <param name="json">The JSON to deserialize.</param>
    /// <param name="settings">JsonSerializerSettings to control deserialization.</param>
    /// <returns>
    /// An instance of type T deserialized from the given JSON.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">json</exception>
    public static T DeserializeObject<T>(string json, JsonSerializerSettings settings)
    {
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
      jsonSerializer.CheckAdditionalContent = true;
      using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) new StringReader(json)))
        return (T) jsonSerializer.Deserialize((JsonReader) jsonTextReader, typeof (T));
    }

    /// <summary>
    /// Deserializes the given JSON into an instance of type T using the given JsonConverters.
    /// </summary>
    /// <typeparam name="T">The type to which to deserialize.</typeparam>
    /// <param name="json">The JSON to deserialize.</param>
    /// <param name="converters">A collection of JsonConverters to control deserialization.</param>
    /// <returns>
    /// An instance of type T deserialized from the given JSON.
    /// </returns>
    public static T DeserializeObject<T>(string json, params JsonConverter[] converters) => SafeJsonConvert.DeserializeObject<T>(json, SafeJsonConvert.SettingsFromConverters(converters));

    /// <summary>Serializes the given object into JSON.</summary>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>
    /// A string containing the JSON representation of the given object.
    /// </returns>
    public static string SerializeObject(object obj)
    {
      JsonSerializer jsonSerializer = SafeJsonConvert.SerializationSettings != null ? JsonSerializer.CreateDefault(SafeJsonConvert.SerializationSettings) : JsonSerializer.CreateDefault();
      StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      JsonTextWriter jsonTextWriter1 = new JsonTextWriter((TextWriter) stringWriter);
      ((JsonWriter) jsonTextWriter1).Formatting = jsonSerializer.Formatting;
      using (JsonTextWriter jsonTextWriter2 = jsonTextWriter1)
        jsonSerializer.Serialize((JsonWriter) jsonTextWriter2, obj);
      return stringWriter.ToString();
    }

    /// <summary>Serializes the given object into JSON.</summary>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="settings">JsonSerializerSettings to control serialization.</param>
    /// <returns>
    /// A string containing the JSON representation of the given object.
    /// </returns>
    public static string SerializeObject(object obj, JsonSerializerSettings settings)
    {
      JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
      StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture);
      JsonTextWriter jsonTextWriter1 = new JsonTextWriter((TextWriter) stringWriter);
      ((JsonWriter) jsonTextWriter1).Formatting = jsonSerializer.Formatting;
      using (JsonTextWriter jsonTextWriter2 = jsonTextWriter1)
        jsonSerializer.Serialize((JsonWriter) jsonTextWriter2, obj);
      return stringWriter.ToString();
    }

    /// <summary>
    /// Serializes the given object into JSON using the given JsonConverters.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="converters">A collection of JsonConverters to control serialization.</param>
    /// <returns>
    /// A string containing the JSON representation of the given object.
    /// </returns>
    public static string SerializeObject(object obj, params JsonConverter[] converters) => SafeJsonConvert.SerializeObject(obj, SafeJsonConvert.SettingsFromConverters(converters));

    private static JsonSerializerSettings SettingsFromConverters(
      JsonConverter[] converters)
    {
      if (converters == null || converters.Length == 0)
        return (JsonSerializerSettings) null;
      return new JsonSerializerSettings()
      {
        Converters = (IList<JsonConverter>) converters
      };
    }
  }
}
