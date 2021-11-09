using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfferService.Models
{
    public class Offer
    {
        public Offer()
        {
            IsApproved = Quotes.Any(x => x.IsApproved);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        [NotMapped]
        public List<Quote> Quotes { get; set; } = new();
        public bool IsApproved { get; }
    }
}
