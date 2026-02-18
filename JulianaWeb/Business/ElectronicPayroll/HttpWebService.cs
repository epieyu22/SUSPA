using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace JulianaWeb.Business.ElectronicPayroll
{
  public class HttpWebService
  {
    public enum HttpType
    {
      POST = 0,
      GET
    }

    private string _Url { get; set; }
    public Dictionary<string, string> Headers { get; set; }

    public HttpWebService(string url = "")
    {
      _Url = url;
      Headers = new Dictionary<string, string>();
    }

    public bool GetRequest(string endpoint, string json, out string response)
    {
      response = Request(endpoint, HttpType.GET, json);
      return !string.IsNullOrEmpty(response);
    }

    public bool PostRequest(string endpoint, string json, out string response)
    {
      response = Request(endpoint, HttpType.POST, json);
      return !string.IsNullOrEmpty(response);
    }

    private string Request(string endpoint, HttpType type, string json)
    {
      string RSP = "";
      try
      {
        HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(_Url + endpoint);
        Request.AllowAutoRedirect = true;
        Request.UserAgent = "JulianaWebClient";
        Request.Method = $"{type}";
        Request.ContentType = "application/json";

        foreach (var Header in Headers)
        {
          Request.Headers.Add(Header.Key, Header.Value);
        }

        byte[] Buffer = Encoding.UTF8.GetBytes(json);

        Request.ContentLength = Buffer.Length;
        using (Stream DataRequest = Request.GetRequestStream())
        {
          DataRequest.Write(Buffer, 0, Buffer.Length);
          DataRequest.Flush();
        }

        HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
        Stream DataResponse = Response.GetResponseStream();
        using (StreamReader Reader = new StreamReader(DataResponse))
        {
          RSP = Reader.ReadToEnd();
        }
        DataResponse.Close();
        Response.Close();
      }
      catch (WebException ex)
      {
        if (!ex.Message.Contains("401"))
        {
          RSP = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
        }
      }

      return RSP;
    }
  }
}
