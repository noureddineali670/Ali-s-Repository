using System;
using System.Collections.Generic;

namespace SupplierManagement.Models;

public partial class PurchaseReceipt
{
    public int Id { get; set; }

    public int PurchaseOrderId { get; set; }

    public DateTime ReceiptDate { get; set; }

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    public virtual ICollection<PurchaseReceiptItem> PurchaseReceiptItems { get; set; } = new List<PurchaseReceiptItem>();
}
