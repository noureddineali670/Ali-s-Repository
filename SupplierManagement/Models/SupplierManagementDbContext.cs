using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SupplierManagement.Models;

public partial class SupplierManagementDbContext : DbContext
{
    public SupplierManagementDbContext()
    {
    }

    public SupplierManagementDbContext(DbContextOptions<SupplierManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ItemLedgerEntry> ItemLedgerEntries { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public virtual DbSet<PurchaseReceipt> PurchaseReceipts { get; set; }

    public virtual DbSet<PurchaseReceiptItem> PurchaseReceiptItems { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemLedgerEntry>(entity =>
        {
            entity.HasIndex(e => e.PurchaseOrderItemId, "IX_ItemLedgerEntries_PurchaseOrderItemId");

            entity.HasOne(d => d.PurchaseOrderItem).WithMany(p => p.ItemLedgerEntries)
                .HasForeignKey(d => d.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasIndex(e => e.SupplierId, "IX_PurchaseOrders_SupplierId");

            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseOrderItems_PurchaseOrderId");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PurchaseReceipt>(entity =>
        {
            entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseReceipts_PurchaseOrderId");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseReceipts)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PurchaseReceiptItem>(entity =>
        {
            entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseReceiptItems_PurchaseOrderId");

            entity.HasIndex(e => e.PurchaseOrderItemId, "IX_PurchaseReceiptItems_PurchaseOrderItemId");

            entity.HasIndex(e => e.PurchaseReceiptId, "IX_PurchaseReceiptItems_PurchaseReceiptId");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseReceiptItems).HasForeignKey(d => d.PurchaseOrderId);

            entity.HasOne(d => d.PurchaseOrderItem).WithMany(p => p.PurchaseReceiptItems)
                .HasForeignKey(d => d.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PurchaseReceipt).WithMany(p => p.PurchaseReceiptItems)
                .HasForeignKey(d => d.PurchaseReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
