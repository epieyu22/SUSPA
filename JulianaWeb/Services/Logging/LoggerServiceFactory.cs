using JulianaWeb.Interfaces.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulianaWeb.Services.Logging
{
  public static class LoggerServiceFactory
  {
    public static ILoggerService<T> Get<T>() where T : class
    {
      return new NLogLoggerService<T>();
    }
  }
}
