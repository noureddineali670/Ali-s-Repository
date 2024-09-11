using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SupplierManagement.Models.DTOs
{
    public class CreatePurchaseReceiptDto
    {
        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        public DateTime ReceiptDate { get; set; }

        [Required]
        public List<CreatePurchaseReceiptItemDto> Items { get; set; }
    }

    public class CreatePurchaseReceiptItemDto
    {
        [Required]
        public int PurchaseOrderItemId { get; set; }

        [Required]
        public int ReceivedQuantity { get; set; }
    }
}
