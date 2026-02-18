using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace JulianaWeb.Providers
{
  public class JavascriptResourceHandler : IHttpHandler
  {
    #region IHttpHandler Members

    public bool IsReusable
    {
      // Return false in case your Managed Handler cannot be reused for another request.
      // Usually this would be false in case you have some state information preserved per request.
      get { return true; }
    }

    public void ProcessRequest(HttpContext context)
    {
      var sb = new StringBuilder();
      sb.Append("var SETTINGS = { ");

      var settings = ConfigurationManager.AppSettings.AllKeys.Where(k => k.StartsWith("JS_")).ToList();

      for (int i = 0; i < settings.Count; i++)
      {
        var key = settings[i];
        var name = key.Replace("JS_", string.Empty);
        var value = ConfigurationManager.AppSettings[key];
        sb.Append(name);
        sb.Append(":");
        sb.Append("'");
        sb.Append(HttpUtility.JavaScriptStringEncode(value));
        sb.Append("'");
        if (i != settings.Count - 1)
          sb.Append(",");
      }

      sb.Append("};");

      context.Response.Clear();
      context.Response.ContentType = "text/javascript";
      context.Response.Write(sb.ToString());
    }

    #endregion
  }
}

