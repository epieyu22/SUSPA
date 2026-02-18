using JulianaWeb.Helpers;
using JulianaWeb.Models;
using JulianaWeb.Repositories;
using JW3.Helpers;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace JulianaWeb.Controllers.API
{
  [RoutePrefix("API/HojaVida")]
  public class HojaVidaController : ApiController
  {
    /* Data need to fill some combobox and layout in the hojavida frontend */
    [Route("{Empresa}")]
    [HttpGet]
    public IHttpActionResult GetGeneralData(string Empresa)
    {
      var db = new JulianaContext(Empresa);
      var paises = db.PAISES.ToList();
      var departamentos = db.DEPARTAMENTOS.ToList();
      var ciudades = db.CIUDADES.ToList();
      var idiomas = db.IDIOMAS.OrderBy(i => i.Nom_Idioma).ToList();
      var profesiones = db.PROFESIONES.ToList();
      var insteducativas = db.INSTEDUCATIVAS.ToList();
      var cargos = db.CARGOS.ToList();
      var especialidades = db.ESPECIALIDADES.ToList();
      var data = new
      {
        paises,
        departamentos,
        ciudades,
        idiomas,
        profesiones,
        insteducativas,
        cargos,
        especialidades

      };
      return Ok(data);

    }

    /*METODOS PARA LA OPTENCION DE DATOS*/
    /*METODOS PARA AGREGAR DATOS*/
    /*METODOS PARA ACTUALIZAR DATOS*/
    [Route("{Empresa}/Aspirantes")]
    [HttpGet]
    public IHttpActionResult GetHojavidasData(string Empresa)
    {
      var db = new JulianaContext(Empresa);
      var hojavidas = db.HOJAVIDA.ToList();
      var pendientes = hojavidas.Where(hv => hv.Estado == "P");
      var aprobadas = hojavidas.Where(hv => hv.Estado == "A");
      var empleados = hojavidas.Where(hv => hv.Estado != "P" && hv.Estado != "A" && hv.Estado != "R");
      return Ok(new { pendientes, aprobadas, empleados });

    }


    [Route("{Empresa}/{Cedula}")]
    [HttpGet]
    public IHttpActionResult GetHojaVIda(string Empresa, string Cedula)
    {
      var db = new JulianaContext(Empresa);
      string documento = "";
      var queryalias = db.USUARIOS_WEB.FirstOrDefault(u => u.Clave == Cedula);
      if (queryalias != null)
      {
        documento = queryalias.Usuario.Trim();
      }
      else
      {
        documento = Cedula;
      }
      var hojavida = db.HOJAVIDA.FirstOrDefault(hv => hv.Doc_Identidad == documento);
      var empleado = db.EMPLEADOS.FirstOrDefault(e => e.Cedula == documento);
      if (hojavida != null)
      {

        return Ok(hojavida);
      }
      else
      {
        short id = db.HOJAVIDA.Count() > 0 ? Convert.ToInt16(db.HOJAVIDA.Max(p => p.Cod_HojaVida) + 1) : Convert.ToInt16(1);
        string correo = (empleado.Dir_Elec ?? "").Trim();
        hojavida = new HOJAVIDA()
        {
          Aspirante = empleado.Empleado,
          Doc_Identidad = documento,
          Tipo_Documento = empleado.Tip_Documento,
          PNombre = empleado.PNombre,
          SNombre = empleado.SNombre,
          PApellido = empleado.PApellido,
          SApellido = empleado.SApellido,
          Sexo = empleado.Sexo,
          Fec_Nacimiento = empleado.Fec_Nacimiento,
          Cod_Lugar_Expedicion = empleado.Cod_Lugar_Expedicion.Value,
          Celular = empleado.Celular.Trim(),
          Cod_Cargo_Aspira = 0,
          Cod_Ciudad = 0,
          Cod_Nacionalidad = empleado.Cod_Pais_Nacionalidad,
          Cod_Pais = empleado.Cod_Pais_Nacimiento,
          Cod_Personalidad = 0,
          Cod_Profesion = 0,
          Dir_Electronica = (correo.Length > 40 ? correo.Substring(0, 40) : correo),
          Direccion = empleado.Direccion.Trim(),
          Distrito = empleado.Distrito,
          Anos_Experiencia = 0,
          Estado = "C",
          Estudiante_Practica = empleado.Estudiante,
          Est_Civil = empleado.Est_Civil,
          Fec_Actualizacion = "",
          Fec_Creacion = "",
          Idioma1 = "",
          Idioma2 = "",
          Idioma_Nativo = "",
          Nivel_Idioma1 = "",
          Nivel_Idioma2 = "",
          Lib_Militar = empleado.Lib_Militar,
          Porc_Conocimiento_Idioma1 = 0,
          Porc_Conocimiento_Idioma2 = 0,
          PTraslado = "",
          PViaje = "",
          Sketch = "",
          Tel1 = empleado.Telefono.Trim(),
          Tel2 = empleado.Telefono1.Trim(),
          Unidad_Tiempo = "",
          Asp_Salarial = 0,
          Cod_HojaVida = id
        };

        try
        {
          db.HOJAVIDA.Add(hojavida);
          db.SaveChanges();
        }
        catch (DbEntityValidationException e)
        {
          return Ok(e.EntityValidationErrors);
        }
        return Ok(hojavida);
      }
    }

    [Route("{Empresa}/{Cod_HojaVida}")]
    [HttpPost]
    public IHttpActionResult UpdateHojavida(string Empresa, short Cod_HojaVida, [FromBody] HOJAVIDA data)
    {
      JulianaContext db = new JulianaContext(Empresa);

      var hoja = db.HOJAVIDA.FirstOrDefault(x => x.Cod_HojaVida == Cod_HojaVida);

      try
      {
        if (hoja == null)
        {
          throw new Exception();
        }
        db.HOJAVIDA.Attach(hoja);

        hoja.PNombre = data.PNombre;
        hoja.SNombre = data.SNombre;
        hoja.PApellido = data.PApellido;
        hoja.SApellido = data.SApellido;
        hoja.Aspirante = data.Aspirante;
        hoja.Doc_Identidad = data.Doc_Identidad;
        hoja.Cod_Nacionalidad = data.Cod_Nacionalidad;
        hoja.Fec_Nacimiento = data.Fec_Nacimiento;
        hoja.Sexo = data.Sexo;
        hoja.Lib_Militar = data.Lib_Militar;
        hoja.Distrito = data.Distrito;
        hoja.Est_Civil = data.Est_Civil;
        hoja.Direccion = data.Direccion;
        hoja.Cod_Pais = data.Cod_Pais;
        hoja.Dir_Electronica = data.Dir_Electronica;
        hoja.Cod_Ciudad = data.Cod_Ciudad;
        hoja.Tel1 = data.Tel1;
        hoja.Tel2 = data.Tel2;
        hoja.Celular = data.Celular;
        hoja.Asp_Salarial = data.Asp_Salarial;
        hoja.PViaje = data.PViaje;
        hoja.PTraslado = data.PTraslado;
        hoja.Cod_Profesion = data.Cod_Profesion;
        hoja.Idioma_Nativo = data.Idioma_Nativo;
        hoja.Idioma1 = data.Idioma1;
        hoja.Porc_Conocimiento_Idioma1 = data.Porc_Conocimiento_Idioma1;
        hoja.Idioma2 = data.Idioma2;
        hoja.Porc_Conocimiento_Idioma2 = data.Porc_Conocimiento_Idioma2;
        hoja.Estudiante_Practica = data.Estudiante_Practica;
        hoja.Anos_Experiencia = data.Anos_Experiencia;
        hoja.Fec_Creacion = data.Fec_Creacion;
        hoja.Fec_Actualizacion = data.Fec_Actualizacion;
        hoja.Cod_Cargo_Aspira = data.Cod_Cargo_Aspira;
        hoja.Sketch = data.Sketch;
        hoja.Cod_Personalidad = data.Cod_Personalidad;
        hoja.Tipo_Documento = data.Tipo_Documento;
        hoja.Estado = data.Estado;
        hoja.Cod_Lugar_Expedicion = data.Cod_Lugar_Expedicion;
        hoja.Nivel_Idioma1 = data.Nivel_Idioma1;
        hoja.Nivel_Idioma2 = data.Nivel_Idioma2;
        hoja.Unidad_Tiempo = data.Unidad_Tiempo;

        db.Entry(hoja).State = EntityState.Modified;
        db.SaveChanges();
      }
      catch (Exception)
      {
        return BadRequest("Ocurrio un error inesperado al guardar.");
      }

      string cedula = data.Doc_Identidad;
      if (cedula != null)
      {
        cedula = cedula.Trim();
      }
      else
      {
        cedula = "";
      }

      var empleado = db.EMPLEADOS.AsNoTracking().FirstOrDefault(e => e.Cedula == cedula);

      try
      {
        if (empleado == null)
        {
          throw new Exception();
        }
        db.EMPLEADOS.Attach(empleado);

        empleado.Direccion = data.Direccion?.Trim() ?? "";
        empleado.Telefono = data.Tel1?.Trim() ?? "";
        empleado.Dir_Elec = data.Dir_Electronica?.Trim() ?? "";

        db.Entry(empleado).State = EntityState.Modified;
        db.SaveChanges();
        return Ok();
      }
      catch (DbEntityValidationException e)
      {
        return Ok(e.EntityValidationErrors);
      }
      catch (Exception)
      {
        return BadRequest("Ocurrio un error inesperado al guardar.");
      }
    }

    [Route("{Empresa}/{Cod_HojaVida}/Referencias")]
    [HttpGet]
    public IHttpActionResult getReferenciasHojaVida(string Empresa, short Cod_HojaVida)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var referencias = db.REFERENCIAS_PERSONALES.Where(r => r.Cod_HojaVida == Cod_HojaVida);
      return Ok(referencias);
    }


    //Referencias personales
    [Route("{Empresa}/Referencias/{Cod_HojaVida}")]
    [HttpGet]
    public IHttpActionResult getReferenciasPersonales(string Empresa, short Cod_HojaVida)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var referencias = db.REFERENCIAS_PERSONALES.Where(r => r.Cod_HojaVida == Cod_HojaVida);
      return Ok(referencias);
    }

    [Route("{Empresa}/{Cod_HojaVida}/Referencias/")]
    [HttpPost]
    public IHttpActionResult addReferenciasPersonal(string Empresa, short Cod_HojaVida, [FromBody] REFERENCIAS_PERSONALES data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      data.Cod_HojaVida = Cod_HojaVida;
      data.Verificado = "N";
      data.Observaciones = " ";
      try
      {

        REFERENCIAS_PERSONALES referencia = db.REFERENCIAS_PERSONALES.Add(data);
        db.SaveChanges();
        return Ok(referencia);
      }
      catch (DbEntityValidationException e)
      {
        return Ok(e.EntityValidationErrors);
      }
    }

    [Route("{Empresa}/{Cod_HojaVida}/Referencias/{Cod_Refrencia_Personal}")]
    [HttpPost]
    public IHttpActionResult updateReferenciasPersonal(string Empresa, short Cod_HojaVida, [FromBody] REFERENCIAS_PERSONALES data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      try
      {

        REFERENCIAS_PERSONALES referencia = db.REFERENCIAS_PERSONALES.Attach(data);
        db.Entry(data).State = EntityState.Modified;
        db.SaveChanges();
        return Ok(data);
      }
      catch (DbEntityValidationException e)
      {
        return Ok(e.EntityValidationErrors);
      }
    }


    /*METODOS PARA LA ELIMINACION DE DATOS*/

    [Route("{Empresa}/{Cod_HojaVida}/Referencias/{Cod_Referencia_Personal}/Delete")]
    [HttpPost]

    public IHttpActionResult DeleteReferenciasPersonal(string Empresa, int Cod_Referencia_Personal)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var data = db.REFERENCIAS_PERSONALES.First(d => d.Cod_Referencia_Personal == Cod_Referencia_Personal);
      db.REFERENCIAS_PERSONALES.Remove(data);
      db.SaveChanges();
      return Ok();
    }

    [Route("{Empresa}/FormacionAcademica/{Cod_FormAcademica}/Delete")]
    [HttpPost]
    public IHttpActionResult DeleteFormacionAcademica(string Empresa , short Cod_FormAcademica)
    {
      var db = new JulianaContext(Empresa);
      FORMACADEMICA formacademica = db.FORMACADEMICA.FirstOrDefault(i => i.Cod_FormAcademica == Cod_FormAcademica);
      db.FORMACADEMICA.Remove(formacademica);
      db.SaveChanges();
      return Ok();
    }

    [Route("{Empresa}/{Cod_HojaVida}/IdiomasHojaVida/{Cod_Idioma_Hojavida}/Delete")]
    [HttpPost]
    public IHttpActionResult DeleteIdiomasHojavida(string Empresa , short Cod_Idioma_Hojavida)
    {
      var db = new JulianaContext(Empresa);
      IDIOMAS_HOJAVIDA idioma = db.IDIOMAS_HOJAVIDA.FirstOrDefault(i => i.Cod_Idioma_Hojavida == Cod_Idioma_Hojavida);
      db.IDIOMAS_HOJAVIDA.Remove(idioma);
      db.SaveChanges();
      return Ok();
    }

    [Route("{Empresa}/Experiencia/{Cod_ExpLaboral}/Delete/Exp")]
    [HttpPost]
    public IHttpActionResult EliminarExperienciaLaboral(string Empresa,short Cod_ExpLaboral)
    {
      var db = new JulianaContext(Empresa);
      EXPLABORAL experiencia = db.EXPLABORAL.FirstOrDefault(i => i.Cod_ExpLaboral == Cod_ExpLaboral);
      db.EXPLABORAL.Remove(experiencia);
      db.SaveChanges();
      return Ok();
    }

    [Route("{Empresa}/{Cod_HojaVida}/IdiomasHojaVida")]
    [HttpPost]
    public IHttpActionResult SetIdiomasHojavida(string Empresa, short Cod_HojaVida, [FromBody] IDIOMAS_HOJAVIDA data)
    {
      var db = new JulianaContext(Empresa);
      db.IDIOMAS_HOJAVIDA.Add(data);
      db.SaveChanges();
      return Ok();
    }

    [Route("{Empresa}/{Cod_HojaVida}/IdiomasHojaVida")]
    [HttpGet]
    public IHttpActionResult GetIdiomasHojavida(string Empresa, short Cod_HojaVida)
    {
      var db = new JulianaContext(Empresa);
      var idiomas = db.IDIOMAS_HOJAVIDA.Where(i => i.Cod_HojaVida == Cod_HojaVida);
      return Ok(idiomas);
    }


    //Metodo de formacion academica

    [Route("{Empresa}/{Cod_HojaVida}/GetFormacademica")]
    [HttpGet]
    public IHttpActionResult GetFormacademica(string Empresa, string Cod_HojaVida)
    {
      var db = new JulianaContext(Empresa);
      short Cod = Int16.Parse(Cod_HojaVida);
      var formacademica = db.FORMACADEMICA.Where(f => f.Cod_HojaVida == Cod_HojaVida);
      return Ok(formacademica);
    }

    [Route("{Empresa}/{Cod_HojaVida}/FormaAcademica")]
    [HttpPost]
    public IHttpActionResult AddFormacademica(string Empresa, string Cod_HojaVida)
    {
      string uploadpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Client/uploads/");
      string year = DateTime.Now.ToString("yyyy");
      string month = DateTime.Now.ToString("MM");
      string day = DateTime.Now.ToString("dd");
      string fileUploadPath = uploadpath + "/" + year + "/" + month + "/" + day + "/";
      string fileUrl = "Client/uploads/" + year + "/" + month + "/" + day + "/";
      var data = System.Web.HttpContext.Current.Request.Form;
      JulianaContext db = new JulianaContext(Empresa);

      FORMACADEMICA formaAcademica = new FORMACADEMICA();

      formaAcademica.Cod_HojaVida = Cod_HojaVida /*!= null ? Convert.ToInt16(data["Cod_HojaVida"]) : Convert.ToInt16(0)*/;
      formaAcademica.Otra_Institucion = data["Otra_Institucion"];
      if (formaAcademica.Otra_Institucion != null)
      {
        formaAcademica.Otra_Institucion = data["Otra_Institucion"];

      }
      else
      {
        formaAcademica.Cod_Institucion = data["FormAcademicaVm"] != null ? Convert.ToInt16(data["Cod_Institucion"]) : Convert.ToInt16(0);

      }
      formaAcademica.Otro_Titulo = data["Otro_Titulo"];
      formaAcademica.Estado = data["Estado"];
      formaAcademica.Adjunto = "fileUrl + filename";
      if (formaAcademica.Cod_Titulo != 0)
      {
        formaAcademica.Cod_Titulo = data["Cod_Titulo"] != null ? Convert.ToInt16(data["Cod_Titulo"]) : Convert.ToInt16(0);

      }
      else
      {
        formaAcademica.Cod_Titulo = data["Cod_Titulo"] != null ? Convert.ToInt16(data["Cod_Titulo"]) : Convert.ToInt16(0);

      }
      formaAcademica.Cod_Nivel = Convert.ToInt16(data["Cod_Nivel"]);
      formaAcademica.Inicio = Convert.ToString(data["Inicio"]);
      formaAcademica.Salida = Convert.ToString(data["Salida"]);
      formaAcademica.Cod_Especialidad = 0;
      formaAcademica.Otra_Especialidad = "";


      db.FORMACADEMICA.Add(formaAcademica);
      db.SaveChanges();

      try
      {
        db.SaveChanges();
        return Ok(formaAcademica);
      }
      catch (DbEntityValidationException e)
      {
        return Ok(e.EntityValidationErrors);
      }

    }


    //Experiencia laboral
    [Route("{Empresa}/{Cod_HojaVida}/ExpLaboral")]
    [HttpGet]
    public IHttpActionResult GetExpLaboral(string Empresa, short Cod_HojaVida)
    {
      var db = new JulianaContext(Empresa);
      var explaboral = db.EXPLABORAL.Where(e => e.Cod_HojaVida == Cod_HojaVida.ToString());
      return Ok(explaboral);
    }


    [Route("{Empresa}/{Cod_HojaVida}/ExpLaboral")]
    [HttpPost]
    public IHttpActionResult SetExpLaboral(string Empresa, short Cod_HojaVida, [FromBody] EXPLABORAL data)
    {

      data.Industria = "";
      data.Jefe_Inmediato = "";
      data.Verificado = "N";
      data.Opinion = "";
      data.Funciones = "";


      var db = new JulianaContext(Empresa);
      EXPLABORAL expLaboral = db.EXPLABORAL.Add(data);
      try
      {
        db.SaveChanges();
        return Ok(expLaboral);
      }
      catch (DbEntityValidationException e)
      {
        return Ok(e.EntityValidationErrors);
      }
    }


    [Route("{Empresa}/Aspirante/Aprobar")]
    [HttpPost]
    public IHttpActionResult AprobarAspirante(string Empresa, [FromBody] HOJAVIDA data)
    {
      JulianaContext db = new JulianaContext(Empresa);
      db.HOJAVIDA.Attach(data);
      data.Estado = "A";
      db.Entry(data).State = EntityState.Modified;
      db.SaveChanges();

      AuthContext authDb = new AuthContext();
      AuthManager manager = new AuthManager();
      var exist = authDb.Users.FirstOrDefault(u => u.UserName == data.Doc_Identidad);
      string password = CryptoHelper.generateNewPass();
      if (exist != null)
      {
        authDb.Users.Remove(exist);
        authDb.SaveChanges();
      }
      ApplicationUser newUser = new ApplicationUser
      {
        UserName = data.Doc_Identidad.Trim(),
        Empleado = data.Aspirante,
        DBName = Empresa
      };
      var result = manager.CreateUser(newUser, password);
      newUser = authDb.Users.First(u => u.UserName == data.Doc_Identidad);
      ApplicationGroup empleadoGroup = authDb.Groups.First(g => g.Name == "Aspirante");
      manager.AddUserToGroup(newUser.Id, empleadoGroup.Id);

      MailHelper.sendNewAspiranteMail(data, password);
      return Ok();
    }

    [Route("{Empresa}/Aspirante/{Cod_HojaVida}")]
    [HttpPost]
    public IHttpActionResult DeleteAspirante(string Empresa, short Cod_HojaVida)
    {
      JulianaContext db = new JulianaContext(Empresa);
      HOJAVIDA hojavida = db.HOJAVIDA.FirstOrDefault(hv => hv.Cod_HojaVida == Cod_HojaVida);
      db.HOJAVIDA.Remove(hojavida);
      db.SaveChanges();
      MailHelper.sendDeleteAspiranteMail(hojavida);
      return Ok();
    }


    [Route("{Empresa}/{Cod_HojaVida}/ChangeProfilePicture")]
    [HttpPost]
    public IHttpActionResult ChangeProfilePicture(string Empresa, short Cod_HojaVida)

    {
      string uploadpath = System.Web.Hosting.HostingEnvironment.MapPath("~/Client/uploads/");
      string year = DateTime.Now.ToString("yyyy");
      string month = DateTime.Now.ToString("MM");
      string day = DateTime.Now.ToString("dd");
      string fileUploadPath = uploadpath + "/" + year + "/" + month + "/" + day + "/";
      string fileUrl = "Client/uploads/" + year + "/" + month + "/" + day + "/";
      if (System.Web.HttpContext.Current.Request.Files.Count > 0)
      {
        JulianaContext db = new JulianaContext(Empresa);
        HOJAVIDA hojavida = db.HOJAVIDA.FirstOrDefault(hv => hv.Cod_HojaVida == Cod_HojaVida);
        new FileInfo(fileUploadPath).Directory.Create();
        var file = System.Web.HttpContext.Current.Request.Files[0];
        if (file != null)
        {
          var filename = Empresa + "_" + Cod_HojaVida + "_" + file.FileName;
          file.SaveAs(fileUploadPath + filename);
          hojavida.Imagen_Perfil = fileUrl + filename;
          db.Entry(hojavida).State = EntityState.Modified;
          db.SaveChanges();
        }
      }
      return Ok();
    }

  }
}
