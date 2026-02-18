using JulianaWeb.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulianaWeb.Interfaces.Business.WebServices.ElectronicPayroll
{
  public interface IElectronicPayrollUpdate<TPayload, TPayloadResult>
  {
    TPayloadResult Update(TPayload payload);
  }
}
