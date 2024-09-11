using System;
using System.Collections.Generic;

namespace SupplierManagement.Models;

public partial class ItemLedgerEntry
{
    public int Id { get; set; }

    public int PurchaseOrderItemId { get; set; }

    public DateTime EntryDate { get; set; }

    public string EntryType { get; set; } = null!;

    public int Quantity { get; set; }

    public string Remarks { get; set; } = null!;

    public virtual PurchaseOrderItem PurchaseOrderItem { get; set; } = null!;
}
