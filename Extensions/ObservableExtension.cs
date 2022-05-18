using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xamariners.Utilities.Extensions
{
  public static class ObservableExtension
  {
    /// <summary>Convert IEnumerable(T) to observable collection.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source">The source.</param>
    public static ObservableCollection<T> ToObservableCollection<T>(
      this IEnumerable<T> source)
    {
      ObservableCollection<T> observableCollection = new ObservableCollection<T>();
      if (source != null)
      {
        if (!(source is T[] objArray))
          objArray = source.ToArray<T>();
        T[] source1 = objArray;
        if (((IEnumerable<T>) source1).Any<T>())
        {
          foreach (T obj in source1)
            observableCollection.Add(obj);
        }
      }
      return observableCollection;
    }
  }
}
