using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OfferService.Models
{
    public class Quote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BankName { get; set; }
        public double InterestRate { get; set; }
        public double MonthlyPaymentPrecent { get; set; }
        public bool IsApproved { get; set; }
        [JsonIgnore]
        public Offer Offer { get; set; }
    }
}
