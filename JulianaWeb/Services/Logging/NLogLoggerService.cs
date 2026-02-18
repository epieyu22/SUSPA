using JulianaWeb.Interfaces.Aspects;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulianaWeb.Services.Logging
{
  public class NLogLoggerService<T> : ILoggerService<T> where T : class
  {
    private readonly Type _type;
    private readonly Logger _logger;

    public NLogLoggerService()
    {
      _type = typeof(T);
      _logger = LogManager.GetLogger("logger");
    }

    public void LogInformation(string message)
    {
      _logger.Info(WrapMessageType(message));
    }

    public void LogWarning(string message)
    {
      _logger.Warn(WrapMessageType(message));
    }

    public void LogError(string message)
    {
      _logger.Error(WrapMessageType(message));
    }

    public void LogError(string message, Exception exception)
    {
      _logger.Error(exception, WrapMessageType(message));
    }

    public void LogDebug(string message)
    {
      var wrappedMessage = WrapMessageType(message);

      _logger.Debug(wrappedMessage);
    }

    private string WrapMessageType(string message)
    {
      return $"{_type.Name} - {message}";
    }
  }
}
