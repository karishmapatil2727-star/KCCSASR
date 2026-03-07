using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FeeDetail
/// </summary>
public class FeeDetail
{
    public int EnrollmentId { get; set; }
    public int FeeHeaderId { get; set; }
    public string FeeHeaderTitle { get; set; }
    public int NoOfInstallment { get; set; }
    public decimal HeaderFee { get; set; }
    public bool IsPayable { get; set; }
    public bool IsOneTimePayment { get; set; }
    public int ApplicableWithInstallment { get; set; }
    public string HeaderRemarks { get; set; }
    public string HeaderAmountRemarks { get; set; }
    public string StudentPaymentRemarks { get; set; }
    public decimal TotalPayable { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal Balance { get; set; }
    public int Installment { get; set; }
    public int LastPaidInstallment { get; set; }
    public bool Editable { get; set; }
    public bool IsPaid { get; set; }
}