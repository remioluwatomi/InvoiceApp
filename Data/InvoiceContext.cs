using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models;
using Microsoft.EntityFrameworkCore.Proxies;
namespace InvoiceApp.Data;

public class InvoiceContext : DbContext
{

    public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options) { }

    public DbSet<Invoice> Invoice {get; set;}
    public DbSet<Item> Item {get; set;}
    public DbSet<Client> Client {get; set;}
    public DbSet<Biller> Biller {get; set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Biller>()
            .HasOne(b => b.Invoice)
            .WithOne(i => i.Biller)
            .HasForeignKey<Biller>(b => b.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Client>()
            .HasOne(c => c.Invoice)
            .WithOne(i => i.Client)
            .HasForeignKey<Client>(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Item>()
                .HasOne(t => t.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(t => t.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Invoice>()
            .Property(i => i.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Invoice>()
            .ToTable(i => i.HasCheckConstraint("INV_Status", "[Status] in ('Pending', 'Approved', 'Draft')"));

        modelBuilder.Entity<Invoice>()
            .Property(i => i.Uid)
            .HasColumnType("UNIQUEIDENTIFIER")
            .HasDefaultValueSql("NEWID()");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
