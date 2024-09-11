using System;
using System.Collections.Generic;

namespace SupplierManagementModels.DTOs
{
    public class PurchaseReceiptDetailsDto
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public List<PurchaseReceiptItemDetailsDto> Items { get; set; }
    }

    public class PurchaseReceiptItemDetailsDto
    {
        public int PurchaseOrderItemId { get; set; }
        public string ProductName { get; set; }
        public int OrderedQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
    }
}
