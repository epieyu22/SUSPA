using JulianaWeb.Helpers;
using JulianaWeb.Models;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace JW3.Helpers
{
  public class MailHelper
  {
    //private static string adminMail = "soporte@systemsltda.com";
    public static void EnviarCorreo(string nombre, string dir_elec, string asunto, string mensaje, string Dir_Copia = "")
    {
      JulianaContext db = new JulianaContext();
      PARAMETROS parametros = db.PARAMETROS.ToList().First();

      string remitente = parametros.Remitente.Trim();
      string servidor = parametros.Servidor.Trim();
      int puerto = Convert.ToInt32(parametros.Puerto.Trim());
      string usuario = parametros.Usuario.Trim();
      string clave = parametros.Clave.Trim();
      bool ssl = Convert.ToBoolean(parametros.SSL);


      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("Notificaciones Juliana", remitente));
      var correos = dir_elec.Trim().Split(';');      
      foreach (string c in correos)
      {
        message.To.Add(new MailboxAddress(nombre.Trim(), c.Trim()));
      }

      if (Dir_Copia != "")
      {
        message.Cc.Add(new MailboxAddress(Dir_Copia.Trim(), Dir_Copia.Trim()));
      }

      message.Subject = asunto;
      var builder = new BodyBuilder();
      string logoJulianaPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/juliana_logo_xs.png");
      string logoEmpresaPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/empresa_logo.png");
      var julianaLogo = builder.LinkedResources.Add(logoJulianaPath);
      var empresaLogo = builder.LinkedResources.Add(logoEmpresaPath);
      julianaLogo.ContentId = "logo_juliana";
      empresaLogo.ContentId = "logo_empresa";
      mensaje = mensaje.Replace("@ano", DateTime.Now.Year.ToString());
      builder.HtmlBody = mensaje;
      message.Body = builder.ToMessageBody();

      using (var client = new SmtpClient())
      {
        var secureOption = ssl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
        client.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
        {
          return true;
        };
        client.Connect(servidor, puerto, secureOption);
        if (client.Capabilities.HasFlag(SmtpCapabilities.Authentication))
        {
          client.Authenticate(usuario, clave);
        }
        client.Send(message);
        client.Disconnect(true);
      }
    }

    public static void sendWellcomeMail(EMPLEADOS Empleado, string newPass, string username)
    {
      JulianaContext db = new JulianaContext();
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/welcome.html");

      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", Empleado.PNombre);
      template = template.Replace("@id", username);
      template = template.Replace("@newPass", newPass);
      var ROOTURL = FullyQualifiedApplicationPath;
      template = template.Replace("@callbackUrl", ROOTURL);
      EnviarCorreo(Empleado.Empleado, Empleado.Dir_Elec, "Juliana - Bienvenido a Juliana Web", template);
      string mensaje = "Codigo Empleado: {0} Genero un nuevo Usuario en Juliana Web";
      mensaje = String.Format(mensaje, Empleado.Cod_Empleado);
      AuditoriaHelper.Log(db, "USUARIOS_WEB", "N", mensaje);
    }

    public static void sendRestorePasswordMail(EMPLEADOS Empleado, string newPass)
    {
      JulianaContext db = new JulianaContext();
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/welcome.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", Empleado.PNombre);
      template = template.Replace("@id", Empleado.Cedula);
      template = template.Replace("@newPass", newPass);
      var ROOTURL = FullyQualifiedApplicationPath;
      template = template.Replace("@callbackUrl", ROOTURL);
      EnviarCorreo(Empleado.Empleado, Empleado.Dir_Elec, "Juliana - Bienvenido a Juliana Web", template);


      string mensaje = "Codigo Empleado: {0}, Restauro su contraseña de usuario en Juliana Web";
      mensaje = String.Format(mensaje, Empleado.Cod_Empleado);
      AuditoriaHelper.Log(db, "USUARIOS_WEB", "M", mensaje);
    }

    public static void Nueva_Solicitud_Vacaciones(string NomBaseDatos, TERCEROS aprobador, EMPLEADOS Empleado, SOLICITUDES data, DateTime fechaLlegada)
    {
      JulianaContext db = new JulianaContext();
      string NomEmpresa = (db.EMPRESAS.Where(x => x.BaseDatos.Trim() == NomBaseDatos).FirstOrDefault()?.Nombre_Empresa) ?? NomBaseDatos;
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/nueva_solicitud_vacaciones.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@Model.username", aprobador.PNombre.Trim());
      template = template.Replace("@Model.empleado", Empleado.Empleado.Trim());
      template = template.Replace("@Model.empresa", !string.IsNullOrWhiteSpace(NomEmpresa) ? NomEmpresa.Trim() : "(NO IDENTIFICA)");
      template = template.Replace("@Model.fechaSalida", data.Fec_Salida.Value.ToString("D"));
      template = template.Replace("@﻿Model.fechaLlegada", fechaLlegada.ToString("D"));
      string modoVacaciones = data.Modo_Vacaciones == "T" ? "Tiempo" : "Dinero";
      template = template.Replace("@Model.modoVacaciones", modoVacaciones);
      template = template.Replace("@Model.cantidadDias", data.Cantidad.ToString());
      var ROOTURL = FullyQualifiedApplicationPath;
      string callbackURL = ROOTURL + "Solicitud/" + NomBaseDatos.Trim() + "/" + data.Cod_Solicitud;
      template = template.Replace("@Model.callbackURL", callbackURL);

      EnviarCorreo(aprobador.Tercero, aprobador.Dir_Elec, "Juliana - Nueva Solicitud de Vacaciones", template, data.empleado.Dir_Elec);
      string mensaje = "Se envio correo para aprobacion de solicitud de vacaciones. Cod Empleado: {0}, Cod Aprobador: {1}";
      mensaje = String.Format(mensaje, Empleado.Cod_Empleado, aprobador.Cod_Tercero);
      AuditoriaHelper.Log(db, "SOLICITUDES", "N", mensaje);
    }

    public static void Solicitud_Vacaciones_Aprobada(EMPLEADOS empleado, SOLICITUDES data)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/aprobar_solicitud_vacaciones.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@Model.username", empleado.PNombre.Trim());
      template = template.Replace("@Model.fechaSalida", data.Fec_Salida.Value.ToString("D"));
      template = template.Replace("@Model.fechaLlegada", data.Fec_Llegada.Value.ToString("D"));
      new Thread(delegate ()
      {
        EnviarCorreo(empleado.Empleado, empleado.Dir_Elec, "Juliana - Solicitud de vacaciones Aprobada", template);
      }).Start();
    }


    public static void EnviarSolicitudLicencia(EMPLEADOS empleado, TERCEROS aprobador, SOLICITUDES data, string Concepto)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/solicitud_licencias.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", "Empleado Prueba");
      template = template.Replace("@empleado", empleado.Empleado.Trim());
      template = template.Replace("@desde", data.Fec_Salida.Value.ToString("D"));
      template = template.Replace("@hasta", data.Fec_Llegada.Value.ToString("D"));
      template = template.Replace("@concepto", Concepto);
      var ROOTURL = FullyQualifiedApplicationPath;
      template = template.Replace("@callbackurl", ROOTURL);
      template = template.Replace("@cantidad", data.Cantidad.ToString());
      EnviarCorreo(empleado.Empleado, empleado.Dir_Elec, "Juliana - Solicitud de Licencia", template);
      if (aprobador != null)
      {
        EnviarCorreo(aprobador.Tercero, aprobador.Dir_Elec, "Juliana - Solicitud de Licencia", template);
      }
      
    }

    public static void mailRechzarSolicitudVacaciones(EMPLEADOS user, SOLICITUDES data)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/rechzar_solicitud_vacaciones.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@Model.username", user.PNombre.Trim());
      template = template.Replace("@Model.fechaSalida", data.Fec_Salida.Value.ToString("D"));
      template = template.Replace("@Model.motivo", data.Observacion);
      new Thread(delegate ()
      {
        EnviarCorreo(user.Empleado, user.Dir_Elec, "Juliana - Solicitud de vacaciones rechazada", template);
      }).Start();
    }

    public static void mailRechzarSolicitudLicencias(EMPLEADOS user, SOLICITUDES data)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/rechazar_solicitud_licencias.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@Model.username", user.PNombre.Trim());
      template = template.Replace("@Model.fechaSalida", data.Fec_Salida.Value.ToString("D"));
      template = template.Replace("@Model.descripcion", data.Observacion);
      new Thread(delegate ()
      {
        EnviarCorreo(user.Empleado, user.Dir_Elec, "Juliana - Solicitud de licencia rechazada", template);
      }).Start();
    }

    public static void mailPedidoDesaprobacion(SOLICITUDES solicitud)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/desaprobar_solicitud_vacaciones.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@user", solicitud.aprobador.PNombre.Trim());
      template = template.Replace("@empleado", solicitud.empleado.Empleado.Trim());
      template = template.Replace("@fechaSalida", solicitud.Fec_Salida.Value.ToString("D"));
      var ROOTURL = FullyQualifiedApplicationPath;
      string callbackURL = ROOTURL + "Solicitudes/Aprobar/Vacaciones/" + solicitud.Cod_Solicitud;
      template = template.Replace("@callbackUrl", callbackURL);
      new Thread(delegate ()
      {
        EnviarCorreo(solicitud.empleado.Empleado, solicitud.aprobador.Dir_Elec, "Pedido de desaprobación", template);
      }).Start();
    }

    internal static void Solicitud_Certificado_Laboral(EMPLEADOS Empleado, string info)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/solicitud_certificado_laboral.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@Empleado", Empleado.Empleado.Trim());
      template = template.Replace("@info", info);
      new Thread(delegate ()
      {
        EnviarCorreo(Empleado.Empleado, "and7702@gmail.com", "Juliana - Solicitud Certificado Laboral", template);
      }).Start();
    }

    public static string FullyQualifiedApplicationPath
    {
      get
      {
        //Return variable declaration
        var appPath = string.Empty;

        //Getting the current context of HTTP request
        var context = HttpContext.Current;

        //Checking the current context content
        if (context != null)
        {
          //Formatting the fully qualified website url/name
          appPath = string.Format("{0}://{1}{2}{3}",
                                  context.Request.Url.Scheme,
                                  context.Request.Url.Host,
                                  context.Request.Url.Port == 80
                                      ? string.Empty
                                      : ":" + context.Request.Url.Port,
                                  context.Request.ApplicationPath);
        }
        if (!appPath.EndsWith("/"))
        {
          appPath += "/";
        }

        return appPath;
      }
    }


    public static bool sendAttachmentMail(string addressName, string address, string subject, string body, byte[] attachFile, string filename)
    {
      JulianaContext db = new JulianaContext();
      PARAMETROS parametros = db.PARAMETROS.ToList().First();

      string remitente = parametros.Remitente.Trim();
      string servidor = parametros.Servidor.Trim();
      int puerto = Convert.ToInt32(parametros.Puerto.Trim());
      string usuario = parametros.Usuario.Trim();
      string clave = parametros.Clave.Trim();
      bool ssl = parametros.SSL;

      var message = new MimeMessage();
      message.From.Add(new MailboxAddress("Notificaciones Juliana Web", remitente));
      //message.Cc.Add(new MailboxAddress("Copia Soporte", "julianaweb@systemsltda.com"));

      var correos = address.Split(';');
      foreach (string c in correos)
      {
        message.To.Add(new MailboxAddress(addressName.Trim(), c.Trim()));
      }


      //message.To.Add(new MailboxAddress(addressName, adminMail)); // test change for line below in prod
      //message.To.Add(new MailboxAddress(addressName, address.Trim()));
      message.Subject = subject;
      var builder = new BodyBuilder();
      string logoJulianaPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/juliana_logo_xs.png");
      string logoEmpresaPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/images/empresa_logo.png");
      var julianaLogo = builder.LinkedResources.Add(logoJulianaPath);
      var empresaLogo = builder.LinkedResources.Add(logoEmpresaPath);
      julianaLogo.ContentId = "logo_juliana";
      empresaLogo.ContentId = "logo_empresa";

      builder.HtmlBody = body;
      builder.Attachments.Add(filename, attachFile);

      message.Body = builder.ToMessageBody();

      //try
      //{
      using (var client = new SmtpClient())
      {
        var secureOption = ssl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;
        client.Connect(servidor, puerto, secureOption);
        client.Authenticate(usuario, clave);
        client.Send(message);
        client.Disconnect(true);
      }
      return true;
      //}
      //catch (Exception e)
      //{
      //  return false;
      //}

    }

    public static void SendCertlabCopyMail(string addressName, string address, string empleado, string token, byte[] attachFile)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/certlab.html");
      string template = File.ReadAllText(templatePath);

      //var attachFile = System.IO.File.ReadAllBytes(path);


      template = template.Replace("@username", addressName);
      template = template.Replace("@empleado", empleado);
      template = template.Replace("@token", token);
      sendAttachmentMail(
          addressName,
          address,
          "Juliana - Certificado laboral Generado desde Juliana",
          template,
          attachFile,
          "Certificado laboral-" + empleado + "-" + token + ".pdf"
          );
    }


    public static void EnviarSolicitudCertlaboral(string addressName, string address, string empleado, string token, byte[] attachFile)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/solicitud_certlaboral.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@empleado", empleado);
      template = template.Replace("@token", token);
      template = template.Replace("@ano", DateTime.Now.Year.ToString());
      var ROOTURL = FullyQualifiedApplicationPath;
      template = template.Replace("@callbackurl", ROOTURL);
      sendAttachmentMail(
          addressName,
          address,
          "Juliana - Solicitud Certificado laboral",
          template,
          attachFile,
          "Certificado laboral - " + empleado + " - " + token + ".pdf"
          );
    }

    internal static void sendDeleteAspiranteMail(HOJAVIDA hojavida)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/delete-aspirante.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", hojavida.Aspirante);
      EnviarCorreo(hojavida.Aspirante, hojavida.Dir_Electronica, "Ingreso rechazado", template);
    }

    internal static void EnviarReporteIncapacidad(EMPLEADOS empleado, string Dir_Elec, NOVAUT New_Novaut, string nom_Concepto, string nom_diagnostico = "")
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/informe_incapacidad.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", "Empleado Prueba");
      template = template.Replace("@empleado", empleado.Empleado.Trim());
      template = template.Replace("@desde", UtilHelper.getDate(New_Novaut.Desde).ToString("D"));
      template = template.Replace("@hasta", UtilHelper.getDate(New_Novaut.Hasta).ToString("D"));
      template = template.Replace("@concepto", nom_Concepto);
      template = template.Replace("@cantidad", New_Novaut.Dias.ToString());
      template = template.Replace("@diagnostico", !string.IsNullOrEmpty(nom_diagnostico) ? nom_diagnostico.Trim() : "-");
      EnviarCorreo(Dir_Elec, Dir_Elec, "Juliana - Reporte Incapacidad", template);
    }


    internal static void EnviarAlertaBlockLeave(EMPLEADOS empleado)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/alerta_vacaciones_vencidas.html");
      string template = File.ReadAllText(templatePath);
      EnviarCorreo(empleado.Dir_Elec, empleado.Dir_Elec, "Juliana - Alerta Block Leave", template);
    }



    internal static void sendNewAspiranteMail(HOJAVIDA data, string password)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/welcome-aspirante.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", data.PNombre);
      template = template.Replace("@id", data.Doc_Identidad);
      template = template.Replace("@newPass", password);
      var ROOTURL = FullyQualifiedApplicationPath;
      template = template.Replace("@callbackUrl", ROOTURL);
      EnviarCorreo(data.Aspirante, data.Dir_Electronica, "Juliana - Bienvenido a Juliana Web", template);
    }

    internal static void sendNewAspiranteAproMail(EMPLEADOS empleado, HOJAVIDA h)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/aprobar-hojavida.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@user", empleado.PNombre.Trim());
      template = template.Replace("@data.Nombres", h.Aspirante);
      template = template.Replace("@data.Num_Documento", h.Doc_Identidad);
      template = template.Replace("@data.Dir_Electronica", h.Dir_Electronica);
      var ROOTURL = FullyQualifiedApplicationPath;
      template = template.Replace("@callbackUrl", ROOTURL);
      new Thread(delegate ()
      {
        EnviarCorreo(empleado.Empleado, empleado.Dir_Elec, "Nuevo ingreso de Hoja de vida", template);
      }).Start();
    }

    internal static void sendConfirmarAspiranteMail(AspiranteViewModel data)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/nuevo-aspirante.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", data.PNombre);
      EnviarCorreo(data.PNombre, data.Dir_Electronica, "Juliana - Registro Juliana Web", template);
    }


    public static void SendMailCompropago(string Empresa, string Fec_Nomina)
    {
      JulianaContext db = new JulianaContext(Empresa);
      var query = from e in db.EMPLEADOS
                  join h in db.HISTORICO on e.Cod_Empleado equals h.Cod_Empleado
                  where h.Fec_Nomina == Fec_Nomina
                     && e.Estado != "R"
                     && e.Dir_Elec != ""
                  select new
                  {
                    e.Cod_Empleado,
                    e.Empleado,
                    e.PNombre,
                    e.Dir_Elec
                  } into g
                  group g by new { g.Cod_Empleado, g.Empleado, g.PNombre, g.Dir_Elec } into x
                  select new
                  {
                    x.Key.Cod_Empleado,
                    x.Key.Empleado,
                    x.Key.Dir_Elec,
                    x.Key.PNombre
                  };
      var empleados = query.ToList();
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/compropago_info.html");
      var f = UtilHelper.getDate(Fec_Nomina);
      foreach (var e in empleados)
      {
        string template = File.ReadAllText(templatePath);
        template = template.Replace("@username", e.PNombre.Trim());
        template = template.Replace("@Fec_Nomina", f.ToString("yyyy/MM/dd"));
        template = template.Replace("@callbackUrl", FullyQualifiedApplicationPath);
        EnviarCorreo(e.PNombre, e.Dir_Elec, "Juliana - Comprobante de pago", template);
      }
    }

    internal static void Mail_Solicitudes_Pendientes(SOLICITUDES s)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/solicitud_vacaciones_pendiete.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@Model.username", s.aprobador.PNombre.Trim());
      template = template.Replace("@Model.empleado", s.empleado.Empleado.Trim());
      template = template.Replace("@Model.fechaSalida", s.Fec_Salida.Value.ToString("D"));
      template = template.Replace("@﻿Model.fechaLlegada", s.Fec_Llegada.Value.ToString("D"));
      string modoVacaciones = s.Modo_Vacaciones == "T" ? "Tiempo" : "Dinero";
      template = template.Replace("@Model.modoVacaciones", modoVacaciones);
      template = template.Replace("@Model.cantidadDias", s.Cantidad.ToString());
      var ROOTURL = FullyQualifiedApplicationPath;
      string callbackURL = ROOTURL + "Solicitud/" + s.Cod_Solicitud;
      template = template.Replace("@Model.callbackURL", callbackURL);
      EnviarCorreo(s.empleado.Empleado, s.aprobador.Dir_Elec, "Juliana - Solicitud de Vaciones Pendiente de Aprobación", template);
    }

    public static bool EnviarCorreoCompropago(EMPLEADOS empleado, byte[] compropago, string Fec_Nomina)
    {
      if (empleado.Dir_Elec.Trim() == String.Empty)
      {
        return false;
      }

      var f = UtilHelper.getDate(Fec_Nomina);
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/compropago_info.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", empleado.PNombre.Trim());
      template = template.Replace("@Fec_Nomina", f.ToString("yyyy/MM/dd"));
      template = template.Replace("@callbackUrl", FullyQualifiedApplicationPath);
      string asunto = "Comprobante de Pago - Nómina " + f.ToString("yyyy/MM/dd");
      return sendAttachmentMail(empleado.Empleado.Trim(), empleado.Dir_Elec.Trim(), asunto, template, compropago, "compropago.pdf");
    }


    public static void EnviarCorreoEstadoEnvios(List<dynamic> estados, string Dir_Elec)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Static/templates/emails/delete-aspirante.html");
      string template = File.ReadAllText(templatePath);
      EnviarCorreo(Dir_Elec, Dir_Elec, "Juliana - Confirmación correos", template);
    }


    public static void EnviarSolicitudLicenciaAprobada(EMPLEADOS empleado, SOLICITUDES data, string Concepto)
    {
      string templatePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Static/templates/emails/solicitud_licencias_aprobada.html");
      string template = File.ReadAllText(templatePath);
      template = template.Replace("@username", "Empleado Prueba");
      template = template.Replace("@Cod_Solicitud", data.Cod_Solicitud.ToString());
      template = template.Replace("@empleado", empleado.Empleado.Trim());
      template = template.Replace("@desde", data.Fec_Salida.Value.ToString("D"));
      template = template.Replace("@hasta", data.Fec_Llegada.Value.ToString("D"));
      template = template.Replace("@concepto", Concepto);
      template = template.Replace("@cantidad", data.Cantidad.ToString());
      EnviarCorreo(empleado.Empleado, empleado.Dir_Elec, "Juliana - Solicitud de Licencia", template);
    }


  }


  class DSNSmtpClient : SmtpClient
  {
    protected override DeliveryStatusNotification? GetDeliveryStatusNotifications(MimeMessage message, MailboxAddress mailbox)
    {
      //if (/* some criteria for deciding whether to get DSN's... */)
      return DeliveryStatusNotification.Failure;
      return null;
    }
  }

}
