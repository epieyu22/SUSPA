using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JulianaWeb.Helpers
{
  public class ErrorHelper
  {
    public static void ErrorLogging(string name, Exception ex)
    {
      string filename = DateTime.Now.ToString() + "_" + name + ".log";
      string strPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/" + filename);
      if (!File.Exists(strPath))
      {
        File.Create(strPath).Dispose();
      }
      using (StreamWriter sw = File.AppendText(strPath))
      {
        sw.WriteLine("=============Error Logging ===========");
        sw.WriteLine("===========Start============= " + DateTime.Now);
        sw.WriteLine("Error Message: " + ex.Message);
        sw.WriteLine("Stack Trace: " + ex.StackTrace);
        sw.WriteLine("===========End============= " + DateTime.Now);

      }
    }


  }
}
