using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Xamariners.Utilities.Helpers
{
  public static class ResourceLoader
  {
    /// <summary>
    /// Attempts to find and return the given resource from within the specified assembly.
    /// </summary>
    /// <returns>The embedded resource stream.</returns>
    /// <param name="assembly">Assembly.</param>
    /// <param name="resourceFileName">Resource file name.</param>
    public static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
    {
      string name = ResourceLoader.FormatResourceName(assembly, resourceFileName);
      return assembly.GetManifestResourceStream(name);
    }

    /// <summary>
    /// Attempts to find and return the given resource from within the specified assembly.
    /// </summary>
    /// <returns>The embedded resource as a byte array.</returns>
    /// <param name="assembly">Assembly.</param>
    /// <param name="resourceFileName">Resource file name.</param>
    public static byte[] GetEmbeddedResourceBytes(Assembly assembly, string resourceFileName)
    {
      using (MemoryStream destination = new MemoryStream())
      {
        ResourceLoader.GetEmbeddedResourceStream(assembly, resourceFileName).CopyTo((Stream) destination);
        return destination.ToArray();
      }
    }

    /// <summary>
    /// Attempts to find and return the given resource from within the specified assembly.
    /// </summary>
    /// <returns>The embedded resource as a string.</returns>
    /// <param name="assembly">Assembly.</param>
    /// <param name="resourceFileName">Resource file name.</param>
    public static string GetEmbeddedResourceString(Assembly assembly, string resourceFileName)
    {
      using (StreamReader streamReader = new StreamReader(ResourceLoader.GetEmbeddedResourceStream(assembly, resourceFileName)))
        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// Gets the string embedded resource from within the specified assembly type.
    /// </summary>
    /// <param name="type">The type of object or classes within the assembly.</param>
    /// <param name="resourceName">Name of the resource.</param>
    public static string GetEmbeddedResourceString(Type type, string resourceName) => ResourceLoader.GetEmbeddedResourceString(type.GetTypeInfo().Assembly, resourceName);

    /// <summary>Formats the name of the resource.</summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="resourceName">Name of the resource.</param>
    private static string FormatResourceName(Assembly assembly, string resourceName)
    {
      resourceName = assembly.GetName().Name + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
      string[] array = ((IEnumerable<string>) assembly.GetManifestResourceNames()).Where<string>((Func<string, bool>) (x => x.EndsWith(resourceName, StringComparison.CurrentCultureIgnoreCase))).ToArray<string>();
      if (!((IEnumerable<string>) array).Any<string>())
        throw new Exception("Resource ending with " + resourceName + " not found.");
      return array.Length <= 1 ? ((IEnumerable<string>) array).Single<string>() : throw new Exception("Multiple resources ending with " + resourceName + " found: \n" + string.Join(Environment.NewLine, array));
    }
  }
}
