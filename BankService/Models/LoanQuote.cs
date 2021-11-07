
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService.Models
{
    public class LoanQuote
    {
        public double InterestRate { get; init; }
        [Range(0, 100)]
        public double MonthlyPaymentPrecent { get; init; }
    }
}
