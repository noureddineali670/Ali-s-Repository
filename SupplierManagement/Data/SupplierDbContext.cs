using Microsoft.EntityFrameworkCore;
using SupplierManagement.Models;

namespace SupplierManagement.Data
{
    public class SupplierDbContext : DbContext
    {
        public SupplierDbContext(DbContextOptions<SupplierDbContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<PurchaseReceipt> PurchaseReceipts { get; set; }
        public DbSet<PurchaseReceiptItem> PurchaseReceiptItems { get; set; }
        public DbSet<ItemLedgerEntry> ItemLedgerEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.TotalAmount)
                .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(poi => poi.UnitPrice)
                .HasColumnType("decimal(18,2)"); 

            modelBuilder.Entity<PurchaseReceipt>()
                .HasOne(pr => pr.PurchaseOrder)
                .WithMany(po => po.PurchaseReceipts)
                .HasForeignKey(pr => pr.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<PurchaseReceiptItem>()
                .HasOne(pri => pri.PurchaseReceipt)
                .WithMany(pr => pr.Items)
                .HasForeignKey(pri => pri.PurchaseReceiptId)
                .OnDelete(DeleteBehavior.NoAction);  

            modelBuilder.Entity<PurchaseReceiptItem>()
                .HasOne(pri => pri.PurchaseOrderItem)
                .WithMany(poi => poi.PurchaseReceiptItems)
                .HasForeignKey(pri => pri.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.NoAction);  

          modelBuilder.Entity<ItemLedgerEntry>()
                .HasOne(ile => ile.PurchaseOrderItem)
                .WithMany(poi => poi.ItemLedgerEntries)
                .HasForeignKey(ile => ile.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }



    }
}
