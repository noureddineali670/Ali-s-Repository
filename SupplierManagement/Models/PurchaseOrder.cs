using System;
using System.Collections.Generic;

namespace SupplierManagement.Models;

public partial class PurchaseOrder
{
    public int Id { get; set; }

    public int SupplierId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual ICollection<PurchaseReceiptItem> PurchaseReceiptItems { get; set; } = new List<PurchaseReceiptItem>();

    public virtual ICollection<PurchaseReceipt> PurchaseReceipts { get; set; } = new List<PurchaseReceipt>();

    public virtual Supplier Supplier { get; set; } = null!;
}
