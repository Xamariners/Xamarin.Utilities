using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Xamariners.Utilities.Serialization
{
  /// <summary>
  /// JSON contract resolver that ignores read-only properties during serialization.
  /// </summary>
  public class ReadOnlyJsonContractResolver : DefaultContractResolver
  {
    /// <summary>Creates a JsonProperty for the given MemberInfo.</summary>
    /// <param name="member">The member to create a JsonProperty for.</param>
    /// <param name="memberSerialization">The member's parent MemberSerialization.</param>
    /// <returns>A created JsonProperty for the given MemberInfo.</returns>
    protected virtual JsonProperty CreateProperty(
      MemberInfo member,
      MemberSerialization memberSerialization)
    {
      JsonProperty property = base.CreateProperty(member, memberSerialization);
      PropertyInfo propertyInfo = member as PropertyInfo;
      if (propertyInfo != (PropertyInfo) null)
        property.ShouldSerialize = (Predicate<object>) (t =>
        {
          if (propertyInfo.SetMethod != (MethodInfo) null && !propertyInfo.SetMethod.IsPrivate)
            return true;
          return propertyInfo.GetMethod != (MethodInfo) null && propertyInfo.GetMethod.IsStatic;
        });
      return property;
    }
  }
}
