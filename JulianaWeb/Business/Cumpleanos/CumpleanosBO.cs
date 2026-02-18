using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JulianaWeb.Models.Juliana;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Business
{
  public class CumpleanosBO: ApiController
  {

    public dynamic GetDataCumpleanos(string Empresa)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var fechaHoy =DateTime.Now.ToString("MMdd");
      var fechaMes = DateTime.Now.ToString("MM");

      var cumpleañosHoy = db.EMPLEADOS.Where(x => x.Fec_Nacimiento == fechaHoy && x.Estado != "R");

      List<String> columnData = new List<String>();

      var p1 = new[] {
                  new SqlParameter("@fechaMes", fechaMes)
              };
      string q1 = String.Format("SELECT * from {0}.dbo.EMPLEADOS " +
        "WHERE SUBSTRING(Fec_Nacimiento,5,2)= @fechaMes And Estado<>'R' ORDER BY  SUBSTRING(Fec_Nacimiento,5,4)", Empresa);
      using (var conn = new SqlConnection(SqlHelper.GetConnectionString()))
      {
        using (SqlDataReader cumpleanosReader = SqlHelper.ExecuteReader(conn, CommandType.Text, q1, p1))
        {
          var NombreEmpleado = "";
          var FechaNacimiento = "";
          List<CUMPLEANOS> items = new List<CUMPLEANOS>();
          

          while (cumpleanosReader.Read())
          {
            
              string Empleado = cumpleanosReader.GetString(cumpleanosReader.GetOrdinal("Empleado"));
              string getCumpleanos = cumpleanosReader.GetString(cumpleanosReader.GetOrdinal("Fec_Nacimiento"));
              DateTime Cumpleanos = DateTime.ParseExact(getCumpleanos,
                                                  "yyyyMMdd",
                                                  CultureInfo.InvariantCulture);
              items.Add(new CUMPLEANOS(Empleado, Cumpleanos));        
          }

          return new { items , cumpleañosHoy };

        }
      }
    }
  }
}
