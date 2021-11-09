using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OfferService.Persistency
{
    public class OfferContext: DbContext
    {
        public OfferContext(DbContextOptions options): base(options) { }
    }
}
