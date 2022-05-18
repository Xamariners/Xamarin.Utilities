using System;

namespace Xamariners.Utilities.Extensions
{
  public static class NumberExtensions
  {
    /// <summary>
    /// Type Safe conversion from string to the decimal.
    /// This will return 0m if the string is null or not a number string.
    /// </summary>
    /// <param name="value">The value.</param>
    public static Decimal ToDecimal(this string value)
    {
      Decimal result;
      Decimal.TryParse(value, out result);
      return result;
    }

    /// <summary>
    /// Type Safe conversion from string to the double.
    /// This will return 0.0d if the string is null or not a number string.
    /// </summary>
    /// <param name="value">The value.</param>
    public static double ToDouble(this string value)
    {
      double result;
      double.TryParse(value, out result);
      return result;
    }

    /// <summary>To the ordinal.</summary>
    /// <param name="value">The value.</param>
    public static string ToOrdinal(this int value)
    {
      string str = "th";
      int num = value % 100;
      if (num < 11 || num > 13)
      {
        switch (num % 10)
        {
          case 1:
            str = "st";
            break;
          case 2:
            str = "nd";
            break;
          case 3:
            str = "rd";
            break;
        }
      }
      return string.Format("{0}{1}", (object) value, (object) str);
    }
  }
}
