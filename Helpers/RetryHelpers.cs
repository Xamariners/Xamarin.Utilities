using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamariners.Utilities.Helpers
{
  public static class RetryHelpers
  {
    public static async Task<bool> Retry(Func<Task<bool>> action, int interval, int retryCount = 3)
    {
      bool flag = false;
      List<Exception> source = new List<Exception>();
      TimeSpan delay = TimeSpan.FromMilliseconds((double) interval);
      for (int index = 0; index < retryCount; ++index)
      {
        try
        {
          flag = await action();
          if (flag)
            return true;
          Task.Delay(delay).Wait();
        }
        catch (Exception ex)
        {
          if (source.All<Exception>((Func<Exception, bool>) (x => x.Message != ex.Message)))
            source.Add(ex);
          Task.Delay(delay).Wait();
        }
      }
      if (!flag && !source.Any<Exception>())
        return false;
      if (source.Count<Exception>() == 1)
        throw source[0];
      if (source.Count<Exception>() > 1)
        throw new AggregateException((IEnumerable<Exception>) source);
      return flag;
    }
  }
}
