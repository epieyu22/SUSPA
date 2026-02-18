using System;

namespace JulianaWeb.Business
{
  public class JulianaException : Exception
  {
    public string Code { get; private set; }

    public JulianaException(string Code, string Message) : base(Message) {
      this.Code = Code;
    }
  }
}
