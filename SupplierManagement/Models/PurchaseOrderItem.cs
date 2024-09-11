using System;
using System.Collections.Generic;

namespace SupplierManagement.Models;

public partial class PurchaseOrderItem
{
    public int Id { get; set; }

    public int PurchaseOrderId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual ICollection<ItemLedgerEntry> ItemLedgerEntries { get; set; } = new List<ItemLedgerEntry>();

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    public virtual ICollection<PurchaseReceiptItem> PurchaseReceiptItems { get; set; } = new List<PurchaseReceiptItem>();
}
