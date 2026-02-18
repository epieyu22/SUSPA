using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using JulianaWeb.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Drawing;
using JulianaWeb.Helpers;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Data;
using JulianaWeb.Business;
using System.Diagnostics;

namespace JulianaWeb.Controllers
{
  public class ExcelController : Controller
  {
    Dictionary<string, string> Estados_Names = new Dictionary<string, string>() {
      { "A",  "Aprobada" },
      { "A0",  "Aprobada Nivel 1" },
      { "A1",  "Aprobada Nivel 2" },
      { "A2",  "Aprobada Nivel 3" },
      { "A3",  "Aprobada Nivel 4" },
      { "A4",  "Aprobada Nivel 5" },
      { "A5",  "Aprobada Nivel 6" },
      { "A6",  "Aprobada Nivel 7" },
      { "A7",  "Aprobada Nivel 8" },
      { "A8",  "Aprobada Nivel 9" },
      { "A9",  "Aprobada Nivel 10" },
      { "AP", "Pagada" },
      { "P", "Pendiente Aprobación" },
      { "PR", "Pendiente Desaprobación" },
      { "R", "Rechazada" },
      { "D", "Eliminada" },
      { "P1", "Pagada" },
      { "L", "Liquidada" },
      { "E", "Enviado" },
      { "T", "Enviado" },
      { "", "" },
    };

    // GET: Excel
    public ActionResult Index()
    {
      JulianaContext db = new JulianaContext("NOBOSZ");
      List<SOLICITUDES> data = db.SOLICITUDES.ToList();
      WebGrid grid = new WebGrid(source: data, canPage: false, canSort: false);

      string gridData = grid.GetHtml(
          columns: grid.Columns(
                  grid.Column("Fec_Salida", "Fecha Salida"),
                  grid.Column("Fec_Llegada", "Fecha Llagada"),
                  grid.Column("Cantidad", "Días")
                  )
              ).ToString();

      Response.ClearContent();
      Response.AddHeader("content-disposition", "attachment; filename=CustomerInfo.xls");
      Response.ContentType = "application/excel";
      Response.Write(gridData);
      Response.End();
      return View();
    }
    
    public ActionResult Solicitudes(ReporteVacacionesVM reporte)
    {
      using (ExcelPackage pck = new ExcelPackage())
      {
        JulianaContext db = new JulianaContext(reporte.Empresa);
        List<SOLICITUDES> data = new List<SOLICITUDES>();

        if (reporte.Filtro == "Proyeccion")
        {
          reporte.Desde = new DateTime(reporte.Desde.Value.Year, reporte.Desde.Value.Month, 1);
          reporte.Hasta = reporte.Desde.Value.AddMonths(1).AddDays(-1);
          reporte.Filtro = "Fec_Salida";
        }

        switch (reporte.Filtro)
        {
          case "None":
            data = db.SOLICITUDES.ToList();
            break;
          case "Fec_Salida":
            if (reporte.Desde != null && reporte.Hasta != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Salida >= reporte.Desde.Value && s.Fec_Salida <= reporte.Hasta.Value).ToList();
            }
            else if (reporte.Desde != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Salida >= reporte.Desde.Value).ToList();
            }
            else if (reporte.Hasta != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Salida <= reporte.Hasta.Value).ToList();
            }
            else
            {
              data = db.SOLICITUDES.ToList();
            }
            break;
          case "Fec_Llegada":
            if (reporte.Desde != null && reporte.Hasta != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Llegada >= reporte.Desde.Value && s.Fec_Llegada <= reporte.Hasta.Value).ToList();
            }
            else if (reporte.Desde != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Llegada >= reporte.Desde.Value).ToList();
            }
            else if (reporte.Hasta != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Llegada <= reporte.Hasta.Value).ToList();
            }
            else
            {
              data = db.SOLICITUDES.ToList();
            }

            break;
          case "Fec_Solicitud":
            if (reporte.Desde != null && reporte.Hasta != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Solicitud >= reporte.Desde.Value && s.Fec_Solicitud <= reporte.Hasta.Value).ToList();
            }
            else if (reporte.Desde != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Solicitud >= reporte.Desde.Value).ToList();
            }
            else if (reporte.Hasta != null)
            {
              data = db.SOLICITUDES.Where(s => s.Fec_Solicitud <= reporte.Hasta.Value).ToList();
            }
            else
            {
              data = db.SOLICITUDES.ToList();
            }

            break;
        }

          var query = from nov in db.NOVAUT
                       join con in db.CONCEPTOS on nov.Cod_Concepto equals con.Cod_Concepto
                       join emp in db.EMPLEADOS on nov.Cod_Empleado equals emp.Cod_Empleado
                       where con.Tipo_Concepto.Trim() == "I"
                       select new
                       {
                         Cod_Concepto = nov.Cod_Concepto,
                         Cod_Empleado = nov.Cod_Empleado,
                         Cantidad = nov.Dias,
                         Tipo_Solicitud = con.Tipo_Concepto,
                         Descripcion = con.Nom_Concepto,
                         Fec_Solicitud = nov.Desde,
                         Fec_Salida = nov.Desde,
                         Fec_Llegada = nov.Hasta,
                         Estado = nov.Estado == "P" ? nov.Estado + "1":nov.Estado,
                         Empleado = emp,
                         Adjunto = nov.Adjunto
                       };

        if (reporte.Desde.HasValue && reporte.Hasta.HasValue)
        {
          string fechaInicio = UtilHelper.getUnglyDate(reporte.Desde.Value);
          string fechaFin = UtilHelper.getUnglyDate(reporte.Hasta.Value);

          query = query.Where(q => String.Compare(q.Fec_Solicitud, fechaInicio) >= 0 &&
                                   String.Compare(q.Fec_Solicitud, fechaFin) <= 0);
        }
        else if (reporte.Desde.HasValue)
        {
          string fechaInicio = UtilHelper.getUnglyDate(reporte.Desde.Value);
          query = query.Where(q => String.Compare(q.Fec_Solicitud, fechaInicio) >= 0);
        }
        else if (reporte.Hasta.HasValue)
        {
          string fechaFin = UtilHelper.getUnglyDate(reporte.Hasta.Value);
          query = query.Where(q => String.Compare(q.Fec_Solicitud, fechaFin) <= 0);
        }

        var queryResult = query.ToList();

        data = data.Concat(queryResult.Select(x => new SOLICITUDES()
          {
            Cod_Concepto = x.Cod_Concepto,
            Cod_Empleado = x.Cod_Empleado,
            Cantidad = x.Cantidad,
            Tipo_Solicitud = x.Tipo_Solicitud,
            Descripcion = x.Descripcion,
            Fec_Solicitud = UtilHelper.getDate(x.Fec_Solicitud),
            Fec_Salida = UtilHelper.getDate(x.Fec_Salida),
            Fec_Llegada = UtilHelper.getDate(x.Fec_Llegada),
            Estado = x.Estado.Trim(),
            empleado = x.Empleado,
            Adjunto = x.Adjunto
          })).ToList();

        List<short> cod_conceptos = data.Where(x => x.Cod_Concepto > 0).Select(x => x.Cod_Concepto).Distinct().ToList();
        Dictionary<short, CONCEPTOS> dataConceptos = db.CONCEPTOS.Where(x => cod_conceptos.Contains(x.Cod_Concepto)).ToDictionary(x => x.Cod_Concepto);

        //Create the worksheet 
        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Solicitudes");
        ws.Cells[1, 1].Value = "Cod";
        ws.Cells[1, 2].Value = "Cedula";
        ws.Cells[1, 3].Value = "Empleado";
        ws.Cells[1, 4].Value = "Fecha Solicitud";
        ws.Cells[1, 5].Value = "Fecha Salida";
        ws.Cells[1, 6].Value = "Fecha Llegada";
        ws.Cells[1, 7].Value = "Días";
        ws.Cells[1, 8].Value = "Aprueba";
        ws.Cells[1, 9].Value = "Estado";
        ws.Cells[1, 10].Value = "Tipo Solicitud";

        for (int i = 0; i < data.Count(); i++)
        {
          ws.Cells[i + 2, 1].Value = data.ElementAt(i).empleado.Cod_Empleado;
          ws.Cells[i + 2, 2].Value = Convert.ToInt64(data.ElementAt(i).empleado.Cedula.Trim());
          ws.Cells[i + 2, 3].Value = data.ElementAt(i).empleado.Empleado.Trim();
          ws.Cells[i + 2, 4].Value = data.ElementAt(i).Fec_Solicitud;
          ws.Cells[i + 2, 5].Value = data.ElementAt(i).Fec_Salida.Value;
          ws.Cells[i + 2, 6].Value = data.ElementAt(i).Fec_Llegada.Value;
          ws.Cells[i + 2, 7].Value = data.ElementAt(i).Cantidad;
          ws.Cells[i + 2, 8].Value = (data.ElementAt(i).aprobador?.Tercero ?? "").Trim();
          ws.Cells[i + 2, 9].Value = Estados_Names[data.ElementAt(i).Estado];
          ws.Cells[i + 2, 10].Value = data.ElementAt(i).Tipo_Solicitud == "V" ? "Vacaciones" : (dataConceptos.ContainsKey(data.ElementAt(i).Cod_Concepto) ?
                                                                                                dataConceptos[data.ElementAt(i).Cod_Concepto].Nom_Concepto :
                                                                                                "Licencia");
          ws.Cells[i + 2, 10].Value = ws.Cells[i + 2, 10].Value.ToString().ToUpper();
        }
        for (int i = 1; i <= ws.Dimension.End.Column; i++) { ws.Column(i).AutoFit(); }
        ws.Column(4).Style.Numberformat.Format = "dd-MM-yyyy";
        ws.Column(5).Style.Numberformat.Format = "dd-MM-yyyy";
        ws.Column(6).Style.Numberformat.Format = "dd-MM-yyyy";

        using (ExcelRange rng = ws.Cells["A1:J1"])
        {
          rng.Style.Font.Bold = true;
          rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
          rng.Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
          rng.Style.Font.Color.SetColor(Color.White);
        }
        using (ExcelRange rng = ws.Cells["E1:G1"])
        {
          rng.Style.Fill.BackgroundColor.SetColor(Color.MediumSeaGreen);
        }

        var fileStream = new MemoryStream();
        pck.SaveAs(fileStream);
        fileStream.Position = 0;

        var fileDownloadName = "Solicitudes.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        var fsr = new FileStreamResult(fileStream, contentType)
        {
          FileDownloadName = fileDownloadName
        };
        return fsr;

      }
    }

    public ActionResult ReporteVacaciones(ReporteVacacionesVM reporte)
    {
      using (ExcelPackage pck = new ExcelPackage())
      {
        JulianaContext db = new JulianaContext(reporte.Empresa);
        var fecNomina = db.HISTORICO.Where(h => h.Cod_Concepto == 40).Max(h => h.Fec_Nomina);
        DateTime fecCorte = UtilHelper.getDate(fecNomina);

        string diasFuturosCausados = reporte.Desde.Value.ToString("yyyyMMdd");
        SqlParameter fechaCorte = new SqlParameter()
        {
          ParameterName = "@Fecha_Corte",
          DbType = DbType.String,
          Value = diasFuturosCausados
        };
        object[] parametros = new object[] { fechaCorte };
        var data = db.Database.SqlQuery<SPReporteVacaciones>("exec SP_ReporteVacaciones @Fecha_Corte", parametros).ToList();
        //Cantidad de dias solicitados
        List <SOLICITUDES> solicitudesDias = new List<SOLICITUDES>();
        solicitudesDias = null;

        //Create the worksheet 
        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Vacaciones");

        ws.Cells[1, 1].Value = "Cod";
        ws.Cells[1, 2].Value = "Doc Identificación";
        ws.Cells[1, 3].Value = "Empleado";
        ws.Cells[1, 4].Value = "Días Causados";
        ws.Cells[1, 5].Value = "Días Tiempo";
        ws.Cells[1, 6].Value = "Días Dinero";
        ws.Cells[1, 7].Value = "Días Disponibles (Corte)";
        ws.Cells[1, 8].Value = "Días Solicitudes Pendientes";
        ws.Cells[1, 9].Value = "Días  Solicitudes Aprobados";
        ws.Cells[1, 10].Value = "Total Solicitados";

        ws.Cells[1, 11].Value = "Solicitados Proyectado";
        ws.Cells[1, 12].Value = "Causado Proyectado";
        ws.Cells[1, 13].Value = "Disponibles Proyectado";
        
        if (reporte.Desde == null)
        {
          reporte.Desde = fecCorte;
        }

        var difMeses = (12)-(fecCorte.Month);

        for (int i = 0; i < data.Count(); i++)
        {

          var Dias_Tiempo = data.ElementAt(i).Dias_Tiempo;
          var Dias_Dinero = data.ElementAt(i).Dias_Dinero;
          var Dias_Pendientes = data.ElementAt(i).Dias_Pendientes;
          var Dias_Aprobados = data.ElementAt(i).Dias_Aprobados;
          var Dias_Aprobados_Corte = data.ElementAt(i).Dias_Aprobados_Corte;

          if (Dias_Pendientes == null)
          {
            Dias_Pendientes = 0;
          }
          if (Dias_Aprobados == null)
          {
            Dias_Aprobados = 0;
          }
        var Dias_Causados = data.ElementAt(i).Dias_Causados;

          Dias_Tiempo = Dias_Tiempo - Dias_Aprobados.Value;

          ws.Cells[i + 2, 1].Value = data.ElementAt(i).Cod_Empleado;
          ws.Cells[i + 2, 2].Value = data.ElementAt(i).Cedula;
          ws.Cells[i + 2, 3].Value = data.ElementAt(i).Empleado.Trim();
          ws.Cells[i + 2, 4].Value = Dias_Causados;
          ws.Cells[i + 2, 5].Value = Dias_Tiempo;
          ws.Cells[i + 2, 6].Value = data.ElementAt(i).Dias_Dinero;
        
          ws.Cells[i + 2, 8].Value = data.ElementAt(i).Dias_Pendientes;
          ws.Cells[i + 2, 9].Value = data.ElementAt(i).Dias_Aprobados;
          ws.Cells[i + 2, 10].Value = data.ElementAt(i).Dias_Pendientes + data.ElementAt(i).Dias_Aprobados;

          var cod_empelado = data.ElementAt(i).Cod_Empleado;
          var empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cod_Empleado == cod_empelado);
          var DiasxAños = Math.Round(Convert.ToDouble(empleado.DiasVacAno) / 12, 2);
          var diasVacAno = empleado.DiasVacAno;
          double diasVacMes = DiasxAños;
          double diasFuturos = diasVacMes * difMeses;        


          var Dias_Disponibles = Dias_Causados - Dias_Tiempo - Dias_Dinero - Dias_Pendientes.Value - Dias_Aprobados.Value;
          double Dias_Anticipados = 0;
          if (Dias_Disponibles < 0)
          {
            Dias_Anticipados = Dias_Disponibles * -1;
          }
          var Dias_Causados_Futuro =  Dias_Causados + diasFuturos;
          
          var Dias_Disponibles_Futuro = Dias_Causados_Futuro - Dias_Tiempo - Dias_Dinero - Dias_Pendientes.Value - Dias_Aprobados_Corte;

          ws.Cells[i + 2, 11].Value = Dias_Anticipados;
          ws.Cells[i + 2, 12].Value = Dias_Causados_Futuro;
          ws.Cells[i + 2, 13].Value = Dias_Disponibles_Futuro;
          ws.Cells[i + 2, 7].Value = Dias_Disponibles > 0 ? Dias_Disponibles : 0;

        }
        for (int i = 1; i <= ws.Dimension.End.Column; i++) { ws.Column(i).AutoFit(); }

        for(int i = 2; i <= 20; i++) { ws.Column(i).Style.Numberformat.Format = "0.00"; }

        using (ExcelRange rng = ws.Cells["A1:M1"])
        {
          rng.Style.Font.Bold = true;
          rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
          rng.Style.Fill.BackgroundColor.SetColor(Color.CornflowerBlue);
          rng.Style.Font.Color.SetColor(Color.White);
        }
        using (ExcelRange rng = ws.Cells["H1:J1"])
        {
          rng.Style.Fill.BackgroundColor.SetColor(Color.MediumSeaGreen);
        }
        using (ExcelRange rng = ws.Cells["K1:M1"])
        {
          rng.Style.Fill.BackgroundColor.SetColor(Color.Indigo);
        }


        var fileStream = new MemoryStream();
        pck.SaveAs(fileStream);
        fileStream.Position = 0;

        var fileDownloadName = "ReporteVacaciones.xlsx";
        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        var fsr = new FileStreamResult(fileStream, contentType)
        {
          FileDownloadName = fileDownloadName
        };
        return fsr;
      }
    }

    public ActionResult Aprobadores(ReporteVacacionesVM reporte)
    {
      using (ExcelPackage pck = new ExcelPackage())
      {
        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Aprobadores");
        ReportesBO reportesBO = new ReportesBO();
        List<AprobadoresViewModel> list = reportesBO.GetReporteAprobadores(reporte.Empresa).Items;

        //Encabezado
        ws.Cells[1, 1].Value = "Cod. Empleado";
        ws.Cells[1, 2].Value = "Doc. Empleado";
        ws.Cells[1, 3].Value = "Car. ID Empleado";
        ws.Cells[1, 4].Value = "Nombre Empleado";
        ws.Cells[1, 5].Value = "Doc. Aprobador";
        ws.Cells[1, 6].Value = "Car. ID Aprobador";
        ws.Cells[1, 7].Value = "Nombre Aprobador";
        ws.Cells[1, 8].Value = "Tipo Aprobacion";
        for (int i = 0; i < list.Count; i++)
        {
          ws.Cells[i + 2, 1].Value = list[i].Cod_Empleado;
          ws.Cells[i + 2, 2].Value = list[i].Documento_Empleado;
          ws.Cells[i + 2, 3].Value = list[i].Cod_Colaborador_Empleado;
          ws.Cells[i + 2, 4].Value = list[i].Detalle_Filtro;
          ws.Cells[i + 2, 5].Value = list[i].Documento_Aprobador;
          ws.Cells[i + 2, 6].Value = list[i].CareerID_Aprobador;
          ws.Cells[i + 2, 7].Value = list[i].Aprobador;
          ws.Cells[i + 2, 8].Value = list[i].Detalle_Tipo_Aprobacion;
        }

        using (ExcelRange rng = ws.Cells["A1:H1"])
        {
          rng.Style.Font.Bold = true;
        }
        for (int col = 1; col <= 8; col++)
        {
          ws.Column(col).AutoFit();
        }
        var fileStream = new MemoryStream();
        pck.SaveAs(fileStream);
        fileStream.Position = 0;

        string fileDownloadName = "ReporteAprobadores.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        var fsr = new FileStreamResult(fileStream, contentType)
        {
          FileDownloadName = fileDownloadName
        };

        return fsr;
      }
    }


    [Route("Excel/Upload/OtrasNov")]
    public ActionResult Upload_OtrasNov()
    {
      if (Request != null)
      {
        string Empresa = Request.Form["Empresa"].Trim();
        JulianaContext db = new JulianaContext(Empresa);
        HttpPostedFileBase file = Request.Files["OtrasNov"];
        if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
        {

          MemoryStream mem = new MemoryStream();
          mem.SetLength((int)file.ContentLength);
          file.InputStream.Read(mem.GetBuffer(), 0, (int) file.ContentLength);

          using (var package = new ExcelPackage(file.InputStream))
          {
            var currentSheet = package.Workbook.Worksheets;
            var ws = currentSheet.First();
            var cols = ws.Dimension.End.Column;
            var rows = ws.Dimension.End.Row;

            List<OtrasnovsViewModel> aa = new List<OtrasnovsViewModel>();
            for(int i = 2; i < rows; i++)
            {
              int Cod_Empleado = Convert.ToInt16(ws.Cells[i, 1].Value.ToString());
              var Empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cod_Empleado == Cod_Empleado);
              if(Empleado == null)
              {
                Empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == Cod_Empleado.ToString() && e.Estado != "R");
              }
              if(Empleado != null)
              {
                short Cuotas = Convert.ToInt16(ws.Cells[i, 5].Value.ToString());
                //OTRASNOV data = new OTRASNOV
                //{
                //  Cod_Empleado = Empleado.Cod_Empleado,
                //  Cod_Zona = Empleado.Cod_Zona,
                //  Cod_Sucursal = Empleado.Cod_Sucursal,
                //  Cod_Ccosto = Empleado.Cod_Ccostos,
                //  Cod_Concepto = Convert.ToInt16(ws.Cells[i, 2].Value.ToString()),
                //  Prioridad = ws.Cells[i, 6].Value.ToString(),
                //  Porc_OtrasNov = 0,
                //  Val_OtrasNov = Convert.ToDouble(ws.Cells[i, 3].Value.ToString()),
                //  Dias = 0,
                //  Tipo = Cuotas > 1 ? "P" : "O",
                //  Estado = "",
                //  Cuotas = 1,
                //  Vigencia = (string) ws.Cells[i, 4].Value,
                //  Cod_Usuario = "999",
                //  Fec_Ing_Novedad = UtilHelper.getUnglyDate(DateTime.Now),
                //  NumCtaVoluntarios = "",
                //  Tipo_Especial = 1,
                //  VigenciaDesde = "",
                //  Fuente = "",
                //  Fec_Aprobado = "",
                //  Fec_Desaprobado = "",
                //  Aplica = ""
                //};
                //db.OTRASNOV.Add(data);

                short Cod_Concepto = Convert.ToInt16(ws.Cells[i, 2].Value.ToString());
                var Concepto = db.CONCEPTOS.Where(c => c.Cod_Concepto == Cod_Concepto).ToList().First();
                string ddd = ws.Cells[i, 4].Value.ToString();
                double Val_OtrasNov = Convert.ToDouble(ws.Cells[i, 3].Value.ToString());
                double Porc_OtrasNov = 0;
                if (Empleado.Salario > 0)
                {
                  Porc_OtrasNov = (Val_OtrasNov * 100)/ Empleado.Salario;
                }

                var Coutas = Convert.ToInt16(ws.Cells[i, 5].Value.ToString());

                var Nom_Ccosto = db.CCOSTOS.Where(cc => cc.Cod_Ccosto == Empleado.Cod_Ccostos).Select(cc => cc.Nom_Ccosto).First();
                var Nom_Sucursal = db.SUCURSALES.Where(s => s.Cod_Sucursal == Empleado.Cod_Sucursal).Select(s => s.Nom_Sucursal).First();

                OtrasnovsViewModel d = new OtrasnovsViewModel
                {
                  Empleado = Empleado.Empleado,
                  Concepto = Concepto.Nom_Concepto,
                  Fecha = ws.Cells[i, 4].Value.ToString(),
                  Val_OtrasNov = Val_OtrasNov,
                  Nom_Sucursal = Nom_Sucursal,
                  Nom_Ccosto = Nom_Ccosto,
                  Coutas = Coutas,
                  Tipo = Coutas > 1 ? "Permanente": "ocasional",
                  Devengo = Concepto.Devengo == "S" ?  "Si" : "No",
                  Prioridad = ws.Cells[i, 6].Value.ToString(),
                  Cod_Concepto = Convert.ToInt16(ws.Cells[i, 2].Value.ToString()),
                  Cod_Empleado = Empleado.Cod_Empleado,
                  Documento = Empleado.Cedula,
                  Porc_OtrasNov = Porc_OtrasNov.ToString("F2")
                };
                aa.Add(d);
              }
            }
            try
            {
              //db.SaveChanges();
              //return Redirect("/Novedades/Result");
              aa = aa.OrderBy(a => a.Cod_Empleado).ToList();
              return Json(aa);
            }
            catch(DbEntityValidationException e)
            {
              return Json(e.EntityValidationErrors);
            }
          }
        }        
       }
      return Json(new {});
    }
  }
}
