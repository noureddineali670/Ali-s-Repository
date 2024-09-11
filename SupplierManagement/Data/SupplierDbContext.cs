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
            // Supplier -> PurchaseOrders
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // PurchaseOrder -> TotalAmount Precision
            modelBuilder.Entity<PurchaseOrder>()
                .Property(po => po.TotalAmount)
                .HasColumnType("decimal(18,2)");  // Set precision and scale for TotalAmount

            // PurchaseOrderItem -> PurchaseOrder
            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // PurchaseOrderItem -> UnitPrice Precision
            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(poi => poi.UnitPrice)
                .HasColumnType("decimal(18,2)");  // Set precision and scale for UnitPrice

            // PurchaseOrder -> PurchaseReceipts
            modelBuilder.Entity<PurchaseReceipt>()
                .HasOne(pr => pr.PurchaseOrder)
                .WithMany(po => po.PurchaseReceipts)
                .HasForeignKey(pr => pr.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading delete

            // PurchaseReceipt -> PurchaseReceiptItems
            modelBuilder.Entity<PurchaseReceiptItem>()
                .HasOne(pri => pri.PurchaseReceipt)
                .WithMany(pr => pr.Items)
                .HasForeignKey(pri => pri.PurchaseReceiptId)
                .OnDelete(DeleteBehavior.NoAction);  // Explicitly prevent cascading delete

            // PurchaseReceiptItem -> PurchaseOrderItem
            modelBuilder.Entity<PurchaseReceiptItem>()
                .HasOne(pri => pri.PurchaseOrderItem)
                .WithMany(poi => poi.PurchaseReceiptItems)
                .HasForeignKey(pri => pri.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.NoAction);  // Explicitly prevent cascading delete

          modelBuilder.Entity<ItemLedgerEntry>()
                .HasOne(ile => ile.PurchaseOrderItem)
                .WithMany(poi => poi.ItemLedgerEntries)
                .HasForeignKey(ile => ile.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }



    }
}
