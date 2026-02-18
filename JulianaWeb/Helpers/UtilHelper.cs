using JulianaWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Razor.Text;

namespace JulianaWeb.Helpers
{
  public class UtilHelper
  {
    public static DateTime getDate(string unglyDate)
    {
      CultureInfo provider = CultureInfo.CreateSpecificCulture("es-CO");
      DateTime result = DateTime.ParseExact(unglyDate, "yyyyMMdd", provider);
      result = result.AddHours(5.0);
      return result;
    }

    public static string ConcatenarFecha(DateTime? source, string caracterConcatenador = "")
    {
      if (!source.HasValue)
        return "";

      DateTime fecha = source.Value;

      Func<int, string> numerizar = (numero) => $"{(numero < 10 ? "0" : "")}{numero}";

      return $"{fecha.Year}{caracterConcatenador}{numerizar(fecha.Month)}{caracterConcatenador}{numerizar(fecha.Day)}";
    }

    public static string generaateNewPass()
    {
      int length = 8;
      const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890$%&#@";
      StringBuilder res = new StringBuilder();
      Random rnd = new Random();
      while (0 < length--)
      {
        res.Append(valid[rnd.Next(valid.Length)]);
      }
      return res.ToString();
    }

    public static double round(double value)
    {
      //var redondeoSalarioBase = value % 100;
      //if (redondeoSalarioBase > 0 && redondeoSalarioBase <= 500)
      //{
      //    value -= redondeoSalarioBase;
      //}
      value = value / 1000;
      value = Math.Round(value, 0);
      value *= 1000;
      return value;
    }

    internal static string getUnglyDate(DateTime fecha)
    {
      string unglyDate = fecha.Year.ToString();
      int day = fecha.Day;
      int month = fecha.Month;
      if (month > 9)
      {
        unglyDate += month.ToString();
      }
      else
      {
        unglyDate += "0" + month.ToString();
      }
      if (day > 9)
      {
        unglyDate += day.ToString();
      }
      else
      {
        unglyDate += "0" + day.ToString();
      }
      return unglyDate;
    }


    static public string numberToLetter(string num)
    {
      string res, dec = "";
      Int64 entero;
      int decimales;
      double nro;

      try

      {
        nro = Convert.ToDouble(num);
      }
      catch
      {
        return "0";
      }

      entero = Convert.ToInt64(Math.Truncate(nro));
      decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
      if (decimales > 0)
      {
        dec = " CON " + decimales.ToString() + "/100";
      }

      res = doubletoText(Convert.ToDouble(entero)) + dec;
      return res;
    }

    static public string numberToLetter_English(string num)
    {
      string res, dec = "";
      Int64 entero;
      int decimales;
      double nro;

      try

      {
        nro = Convert.ToDouble(num);
      }
      catch
      {
        return "";
      }

      entero = Convert.ToInt64(Math.Truncate(nro));
      decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
      if (decimales > 0)
      {
        dec = " CON " + decimales.ToString() + "/100";
      }

      res = doubletoText_English(Convert.ToDouble(entero)) + dec;
      return res;
    }

    static private string doubletoText(double value)
    {
      string Num2Text = "";
      value = Math.Truncate(value);
      if (value == 0) Num2Text = "cero";
      else if (value == 1) Num2Text = "uno";
      else if (value == 2) Num2Text = "dos";
      else if (value == 3) Num2Text = "tres";
      else if (value == 4) Num2Text = "cuatro";
      else if (value == 5) Num2Text = "cinco";
      else if (value == 6) Num2Text = "seis";
      else if (value == 7) Num2Text = "siete";
      else if (value == 8) Num2Text = "ocho";
      else if (value == 9) Num2Text = "nueve";
      else if (value == 10) Num2Text = "diez";
      else if (value == 11) Num2Text = "once";
      else if (value == 12) Num2Text = "doce";
      else if (value == 13) Num2Text = "trece";
      else if (value == 14) Num2Text = "catorce";
      else if (value == 15) Num2Text = "quince";
      else if (value < 20) Num2Text = "dieci" + doubletoText(value - 10);
      else if (value == 20) Num2Text = "veinte";
      else if (value < 30) Num2Text = "veinti" + doubletoText(value - 20);
      else if (value == 30) Num2Text = "treinta";
      else if (value == 40) Num2Text = "cuarenta";
      else if (value == 50) Num2Text = "cincuenta";
      else if (value == 60) Num2Text = "sesenta";
      else if (value == 70) Num2Text = "setenta";
      else if (value == 80) Num2Text = "ochenta";
      else if (value == 90) Num2Text = "noventa";
      else if (value < 100) Num2Text = doubletoText(Math.Truncate(value / 10) * 10) + " y " + doubletoText(value % 10);
      else if (value == 100) Num2Text = "cien";
      else if (value < 200) Num2Text = "ciento " + doubletoText(value - 100);
      else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = doubletoText(Math.Truncate(value / 100)) + "cientos";
      else if (value == 500) Num2Text = "quinientos";
      else if (value == 700) Num2Text = "setecientos";
      else if (value == 900) Num2Text = "novecientos";
      else if (value < 1000) Num2Text = doubletoText(Math.Truncate(value / 100) * 100) + " " + doubletoText(value % 100);
      else if (value == 1000) Num2Text = "mil";
      else if (value < 2000) Num2Text = "mil " + doubletoText(value % 1000);
      else if (value < 1000000)
      {
        Num2Text = doubletoText(Math.Truncate(value / 1000)) + " mil";
        if ((value % 1000) > 0) Num2Text = Num2Text + " " + doubletoText(value % 1000);
      }

      else if (value == 1000000) Num2Text = "un millon";
      else if (value < 2000000) Num2Text = "un millon " + doubletoText(value % 1000000);
      else if (value < 1000000000000)
      {
        Num2Text = doubletoText(Math.Truncate(value / 1000000)) + " millones ";
        if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + doubletoText(value - Math.Truncate(value / 1000000) * 1000000);
      }

      else if (value == 1000000000000) Num2Text = "un billon";
      else if (value < 2000000000000) Num2Text = "un billon " + doubletoText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

      else
      {
        Num2Text = doubletoText(Math.Truncate(value / 1000000000000)) + " billones";
        if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + doubletoText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
      }
      return Num2Text;

    }

    static private string doubletoText_English(double value)
    {
      string Num2Text = "";
      value = Math.Truncate(value);
      if (value == 0) Num2Text = "cero";
      else if (value == 1) Num2Text = "one";
      else if (value == 2) Num2Text = "two";
      else if (value == 3) Num2Text = "three";
      else if (value == 4) Num2Text = "four";
      else if (value == 5) Num2Text = "five";
      else if (value == 6) Num2Text = "six";
      else if (value == 7) Num2Text = "seven";
      else if (value == 8) Num2Text = "eight";
      else if (value == 9) Num2Text = "nine";
      else if (value == 10) Num2Text = "ten";
      else if (value == 11) Num2Text = "eleven";
      else if (value == 12) Num2Text = "twelve";
      else if (value == 13) Num2Text = "thirteen";
      else if (value == 14) Num2Text = "fourteen";
      else if (value == 15) Num2Text = "fifteen";
      else if (value == 16) Num2Text = "sixteen";
      else if (value == 17) Num2Text = "seventeen";
      else if (value == 18) Num2Text = "eighteen";
      else if (value == 19) Num2Text = "nineteen";
      else if (value == 20) Num2Text = "twenty";
      else if (value < 30) Num2Text = "twenty" + doubletoText_English(value - 20);
      else if (value == 30) Num2Text = "thirty";
      else if (value == 40) Num2Text = "forty";
      else if (value == 50) Num2Text = "fifty";
      else if (value == 60) Num2Text = "sixty";
      else if (value == 70) Num2Text = "seventy";
      else if (value == 80) Num2Text = "eighty";
      else if (value == 90) Num2Text = "ninety";
      else if (value < 100) Num2Text = doubletoText_English(Math.Truncate(value / 10) * 10) + " " + doubletoText_English(value % 10);
      else if (value == 100) Num2Text = "hundred";
      else if (value < 200) Num2Text = "one hundred" + doubletoText_English(value - 100);
      else if ((value == 200) || (value == 300) || (value == 400) || (value == 500) || (value == 600) || (value == 700) || (value == 800) || (value == 900)) Num2Text = doubletoText_English(Math.Truncate(value / 100)) + " hundred";
      else if (value < 1000) Num2Text = doubletoText_English(Math.Truncate(value / 100) * 100) + " " + doubletoText_English(value % 100);
      else if (value == 1000) Num2Text = "thousand";
      else if (value < 2000) Num2Text = "one thousand " + doubletoText_English(value % 1000);
      else if (value < 1000000)
      {
        Num2Text = doubletoText_English(Math.Truncate(value / 1000)) + " thousand";
        if ((value % 1000) > 0) Num2Text = Num2Text + " " + doubletoText_English(value % 1000);
      }

      else if (value == 1000000) Num2Text = "one million";
      else if (value < 2000000) Num2Text = "one million " + doubletoText_English(value % 1000000);
      else if (value < 1000000000)
      {
        Num2Text = doubletoText_English(Math.Truncate(value / 1000000)) + " millions ";
        if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + doubletoText_English(value - Math.Truncate(value / 1000000) * 1000000);
      }

      else if (value == 1000000000) Num2Text = "one billion";
      else if (value < 2000000000) Num2Text = "one billion " + doubletoText_English(value - Math.Truncate(value / 1000000000) * 1000000000);

      else
      {
        Num2Text = doubletoText_English(Math.Truncate(value / 1000000000)) + " billones";
        if ((value - Math.Truncate(value / 1000000000) * 1000000000) > 0) Num2Text = Num2Text + " " + doubletoText_English(value - Math.Truncate(value / 1000000000) * 1000000000);
      }
      return Num2Text;

    }

    static private DateTime GetEasterDay(int year)
    {
      double day = 0;
      DateTime easter = default(DateTime);
      int M = 24;
      int N = 5;
      double a = year % 19;
      double b = year % 4;
      double c = year % 7;
      double d = ((19 * a) + M) % 30;
      double e = ((2 * b) + (4 * c) + (6 * d) + N) % 7;
      if ((d + e) < 10)
      {
        day = d + e + 22;
        easter = new DateTime(year, 3, Convert.ToInt16(day));
      }
      else
      {
        day = d + e - 9;
        easter = new DateTime(year, 4, Convert.ToInt16(day));
      }

      if (easter.Month == 4 && easter.Day == 26)
      {
        easter = new DateTime(year, 4, 19);
      }
      if (easter.Month == 4 && easter.Day == 25)
      {
        if (d == 28 && e == 5 && a > 10)
        {
          easter = new DateTime(year, 4, 18);
        }
      }
      return easter;
    }

    static public DateTime moveToMonday(DateTime holiday)
    {
      if (holiday.DayOfWeek != DayOfWeek.Monday)
      {
        if (holiday.DayOfWeek == DayOfWeek.Sunday)
        {
          holiday = holiday.AddDays(1);
        }
        else
        {
          int days = 8 - (int)holiday.DayOfWeek;
          holiday = holiday.AddDays(days);
        }
      }
      return holiday;
    }


    static public List<DateTime> getHolidays(int year, JulianaContext db = null, string empresa = "")
    {
      List<DateTime> H = new List<DateTime>();
      H.Add(new DateTime(year, 1, 1)); //Año Nuevo
      H.Add(moveToMonday(new DateTime(year, 1, 6))); //Día de los Reyes Magos
      H.Add(moveToMonday(new DateTime(year, 3, 19))); //Día de San José
      DateTime easter = GetEasterDay(year); // Domingo de resurreción
      H.Add(easter.AddDays(-3)); //Jueves Santo
      H.Add(easter.AddDays(-2));//Viernes Santo
      H.Add(new DateTime(year, 5, 1));//Día del Trabajo
      H.Add(moveToMonday(easter.AddDays(40)));//Día de la Ascensión
      H.Add(moveToMonday(easter.AddDays(60)));//Corpus Christi
      H.Add(moveToMonday(easter.AddDays(69)));//Sagrado Corazón
      H.Add(moveToMonday(new DateTime(year, 6, 29)));//San Pedro y San Pablo
      H.Add(new DateTime(year, 7, 20));//Día de la Independencia
      H.Add(new DateTime(year, 8, 7));//Batalla de Boyaca
      H.Add(moveToMonday(new DateTime(year, 8, 15)));//La asunción de la Virgen
      H.Add(moveToMonday(new DateTime(year, 10, 12)));//Día de la raza
      H.Add(moveToMonday(new DateTime(year, 11, 1)));//Día de Todos los Santos
      H.Add(moveToMonday(new DateTime(year, 11, 11)));//Independencia de Cartagena
      H.Add(new DateTime(year, 12, 8));//Día de la Inmaculada Concepción
      H.Add(new DateTime(year, 12, 25));//Navidad

      if (db == null)
      {
        if(string.IsNullOrWhiteSpace(empresa))
        {
          db = new JulianaContext();
        }
        else
        {
          db = new JulianaContext(empresa);
        }
      }

      var holidays = db.FERIADOS.Where(f => f.Fecha.StartsWith(year.ToString())).Select(f => f.Fecha).ToList();
      foreach (var item in holidays)
      {
        try
        {
          DateTime tmp = getDate(item);
          if (!H.Contains(tmp)) H.Add(tmp);
        }
        catch (Exception)
        {
        }
      }

      //SOLO PARA DDB
      //H.Add(new DateTime(year, 01, 02));//Bloqueo Solicitado 2024 DDB
      //H.Add(new DateTime(year, 12, 31));//Bloqueo Solicitado 2024 DDB

      List<DateTime> HUTC = new List<DateTime>();
      H.ForEach(h => HUTC.Add(DateTime.SpecifyKind(h, DateTimeKind.Utc)));
      return HUTC;
    }

    internal static DateTime getDate2(string v)
    {
      CultureInfo provider = CultureInfo.CreateSpecificCulture("es-co");
      DateTime result = DateTime.ParseExact(v, "yyyy/MM/dd", provider);
      result = result.AddHours(5.0);
      return result;
    }

    static public double GetYearsAhead(DateTime a)
    {
      DateTime zeroTime = new DateTime(1, 1, 1);
      DateTime b = DateTime.Now;
      double days = (b - a).TotalDays;
      return days / 365;
    }

    public static string ROOTURL
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
          appPath += "/";

        return appPath;
      }
    }


    public static Boolean ExisteVariable(JulianaContext db, int Uso)
    {
      var variable = db.VARIABLES.SingleOrDefault(v => v.Uso == Uso);
      if (variable != null) return true;
      return false;
    }

    public static bool EsNull(object objeto)
    {
      return objeto == null;
    }

    public static bool SonNull(object objeto, params string[] propiedades)
    {
      var type = objeto.GetType();

      var resultado = propiedades.Aggregate(true, (aggr, item) => {

        object valor;

        try
        {
          valor = type.GetProperty(item).GetValue(objeto, null);
        }
        catch { valor = null; }

        return aggr && valor == null;
      });

      return resultado;
    }

    public static bool EsIgualA(object objeto, params object[] valores)
    {
      if (valores == null)
      {
        return objeto == null;
      }

      bool resultado = valores.Aggregate(false, (aggr, item) => {
        return aggr || Equals(item, objeto);
      });

      return resultado;
    }

    public static bool EsLista(object objeto)
    {
      var oType = objeto.GetType();
      return (oType.IsGenericType && (oType.GetGenericTypeDefinition() == typeof(List<>)));
    }

  }
}
