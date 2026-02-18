using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulianaWeb.Interfaces.Aspects
{
  public interface ILoggerService<T> where T : class
  {
    void LogDebug(string message);
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message);

    void LogError(string message, Exception exception);
  }
}
