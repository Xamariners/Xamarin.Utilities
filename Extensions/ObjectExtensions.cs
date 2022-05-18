using System.Reflection;

namespace Xamariners.Utilities.Extensions
{
  public static class ObjectExtensions
  {
    /// <summary>Gets the property value.</summary>
    /// <param name="o">The o.</param>
    /// <param name="propertyName">Name of the property.</param>
    public static object GetPropertyValue(this object o, string propertyName)
    {
      object empty = (object) string.Empty;
      PropertyInfo property = o?.GetType().GetProperty(propertyName);
      if (property != (PropertyInfo) null)
        empty = property.GetValue(o, (object[]) null);
      return empty;
    }
  }
}
