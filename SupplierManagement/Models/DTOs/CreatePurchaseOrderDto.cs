using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SupplierManagement.Models.DTOs
{
    public class CreatePurchaseOrderDto
    {
        [Required]
        public int SupplierId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public List<CreatePurchaseOrderItemDto> Items { get; set; }
    }
}

public class CreatePurchaseOrderItemDto
{
    [Required]
    public string ProductName { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }
}
