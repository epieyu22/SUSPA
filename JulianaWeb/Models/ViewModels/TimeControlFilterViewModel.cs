using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JulianaWeb.Models.ViewModels
{
  public class TimeControlFilterViewModel
  {
    #region Properties

    public string Business { get; set; }

    public short EmployeeId { get; set; }
        
    public DateTime Month { get; set; }

    public string PunchType { get; set; }

    #endregion
  }
}
