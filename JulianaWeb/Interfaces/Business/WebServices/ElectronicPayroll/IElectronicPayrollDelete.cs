using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JulianaWeb.Interfaces.Business.WebServices.ElectronicPayroll
{
  public interface IElectronicPayrollDelete<TPayload>
  {
    void Delete(TPayload payload);
  }
}
