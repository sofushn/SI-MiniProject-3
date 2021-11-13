using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanUi.Models
{
    public class LoanOfferDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public List<LoanQuoteDto> Quotes { get; set; } = new();
        public bool IsApproved { get; }
    }
}
