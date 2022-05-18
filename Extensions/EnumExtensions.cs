using System;
using System.ComponentModel;
using System.Globalization;

namespace Xamariners.Utilities.Extensions
{
  public static class EnumExtensions
  {
    /// <summary>Gets the description.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="e">The e.</param>
    public static string GetDescription<T>(this T e) where T : IConvertible
    {
      string description = (string) null;
      if ((object) e is Enum)
      {
        Type type = e.GetType();
        foreach (int num in Enum.GetValues(type))
        {
          if (num == e.ToInt32((IFormatProvider) CultureInfo.InvariantCulture))
          {
            object[] customAttributes = type.GetMember(type.GetEnumName((object) num))[0].GetCustomAttributes(typeof (DescriptionAttribute), false);
            if (customAttributes.Length != 0)
            {
              description = ((DescriptionAttribute) customAttributes[0]).Description;
              break;
            }
            break;
          }
        }
      }
      return description;
    }
  }
}
