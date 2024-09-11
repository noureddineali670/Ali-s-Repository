using System;
using System.Collections.Generic;

namespace SupplierManagement.Models;

public partial class PurchaseReceiptItem
{
    public int Id { get; set; }

    public int PurchaseReceiptId { get; set; }

    public int PurchaseOrderItemId { get; set; }

    public int ReceivedQuantity { get; set; }

    public int? PurchaseOrderId { get; set; }

    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    public virtual PurchaseOrderItem PurchaseOrderItem { get; set; } = null!;

    public virtual PurchaseReceipt PurchaseReceipt { get; set; } = null!;
}
