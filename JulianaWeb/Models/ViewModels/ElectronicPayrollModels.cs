using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace JulianaWeb.Models.ViewModels
{


  // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
  public class Note
  {
    [JsonProperty("text")]
    public string Text { get; set; }
  }

  public class Entries
  {
    [JsonProperty("entires")]
    public List<ElectronicPayrollInfo> Entrie { get; set; }
  }

  public class Accrual
  {
    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("amount")]
    public double? Amount { get; set; }

    [JsonProperty("amount-ns")]
    public double? AmountNs { get; set; }

    [JsonProperty("days")]
    public double? Days { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("hours")]
    public double? Hours { get; set; }

    [JsonProperty("percentage")]
    public double? Percentage { get; set; }

    [JsonProperty("medical-leave-type")]
    public string MedicalLeaveType { get; set; }

    [JsonProperty("cesantias-interest")]
    public double? CesantiasInterest { get; set; }

  }

  public class Deduction
  {
    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("amount")]
    public double? Amount { get; set; }

    [JsonProperty("percentage")]
    public double? Percentage { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
  }





  public class Address
  {
    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("line")]
    public string Line { get; set; }

    [JsonProperty("department")]
    public string Department { get; set; }
  }

  public class Employee
  {
    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("payment-means")]
    public string PaymentMeans { get; set; }

    [JsonProperty("worker-type")]
    public string WorkerType { get; set; }

    [JsonProperty("sub-code")]
    public string SubCode { get; set; }

    [JsonProperty("start-date")]
    public string StartDate { get; set; }

    [JsonProperty("fire-date")]
    public string FireDate { get; set; }

    [JsonProperty("high-risk")]
    public bool HighRisk { get; set; }

    [JsonProperty("integral-salary")]
    public bool IntegralSalary { get; set; }

    [JsonProperty("contract-type")]
    public string ContractType { get; set; }

    [JsonProperty("identification-type")]
    public string IdentificationType { get; set; }

    [JsonProperty("identification")]
    public string Identification { get; set; }

    [JsonProperty("first-name")]
    public string FirstName { get; set; }

    [JsonProperty("other-names")]
    public string OtherNames { get; set; }

    [JsonProperty("last-name")]
    public string LastName { get; set; }

    [JsonProperty("second-last-name")]
    public string SecondLastName { get; set; }

    [JsonProperty("bank")]
    public string Bank { get; set; }

    [JsonProperty("account-type-kw")]
    public string AccountTypeKw { get; set; }

    [JsonProperty("account-number")]
    public string AccountNumber { get; set; }

    [JsonProperty("address")]
    public Address Address { get; set; }
  }

  public class Software
  {
    [JsonProperty("pin")]
    public string Pin { get; set; }

    [JsonProperty("test-set-id")]
    public string DianTestId { get; set; }

    [JsonProperty("dian-id")]
    public string DianId { get; set; }
  }
  public class Replacement
  {
    [JsonProperty("number")]
    public int Number { get; set; }

    [JsonProperty("prefix")]
    public string Prefix { get; set; }

    [JsonProperty("cune")]
    public string Cune { get; set; }

    [JsonProperty("issue-date")]
    public string IssueDate { get; set; }
  }
  public class ElectronicPayrollInfo
  {
    [JsonProperty("env")]
    public string Env { get; set; }

    [JsonProperty("send_dian")]
    public bool send_dian { get; set; }

    [JsonProperty("prefix")]
    public string Prefix { get; set; }

    [JsonProperty("number")]
    public int Number { get; set; }

    [JsonProperty("salary")]
    public double Salary { get; set; }

    [JsonProperty("periodicity")]
    public string Periodicity { get; set; }

    [JsonProperty("initial-settlement-date")]
    public string InitialSettlementDate { get; set; }

    [JsonProperty("final-settlement-date")]
    public string FinalSettlementDate { get; set; }

    [JsonProperty("issue-date")]
    public string IssueDate { get; set; }

    [JsonProperty("payment-date")]
    public string PaymentDate { get; set; }

    [JsonProperty("entry")]
    public Entry Entry { get; set; }

    [JsonProperty("replacement-for")]
    public Replacement ReplacementFor { get; set; }

    [JsonProperty("notes")]
    public List<Note> Notes { get; set; }

    [JsonProperty("accruals")]
    public List<Accrual> Accruals { get; set; }

    [JsonProperty("deductions")]
    public List<Deduction> Deductions { get; set; }

    [JsonProperty("employee")]
    public Employee Employee { get; set; }

    [JsonProperty("software")]
    public Software Software { get; set; }

 
  }



  public class PayrollResponse
  {
   

    [JsonProperty("pdf")]
    public string Pdf { get; set; }

    [JsonProperty("cune")]
    public string Cune { get; set; }

    [JsonProperty("number")]
    public string Number { get; set; }

    [JsonProperty("qrcode")]
    public string Qrcode { get; set; }

    [JsonProperty("dian_status")]
    public string DianStatus { get; set; }

    [JsonProperty("email_status")]
    public string EmailStatus { get; set; }

    public List<object> dian_messages { get; set; }

    [JsonProperty("errors")]
    public List<Path> Errors { get; set; }

  }
  public class Path
  {

    [JsonProperty("path")]
    public List<PathDescription> errors { get; set; }

    [JsonProperty("Error")]
    public string error { get; set; }

  }
  public class PathDescription
  {

    [JsonProperty("path")]
    public List<object> errors { get; set; }

    [JsonProperty("Error")]
    public string error { get; set; }

  }




  public class Entry
  {
    [JsonProperty("cune")]
    public string Cune { get; set; }

    [JsonProperty("number")]
    public string Number { get; set; }

    [JsonProperty("date")]
    public string Date { get; set; }
  }

}
