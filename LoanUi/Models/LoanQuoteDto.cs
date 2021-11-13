using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanUi.Models
{
    public class LoanQuoteDto
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public double InterestRate { get; set; }
        public double MonthlyPaymentPrecent { get; set; }
        public bool IsApproved { get; set; }
    }
}
