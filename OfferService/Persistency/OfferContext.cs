using Microsoft.EntityFrameworkCore;
using OfferService.Models;

namespace OfferService.Persistency
{
    public class OfferContext: DbContext
    {
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public OfferContext(DbContextOptions<OfferContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Offer>()
                .HasMany(o => o.Quotes)
                .WithOne(q => q.Offer);
        }
    }
}
