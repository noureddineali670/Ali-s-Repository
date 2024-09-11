using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data;
using SupplierManagement.Models.DTOs;
using SupplierManagement.Models;


[Route("api/[controller]")]
[ApiController]
public class PurchaseOrderController : ControllerBase
{
    private readonly SupplierDbContext _context;

    public PurchaseOrderController(SupplierDbContext context)
    {
        _context = context;
    }

    // 1. Create Purchase Order
    [HttpPost]
    public async Task<ActionResult<PurchaseOrder>> CreatePurchaseOrder(CreatePurchaseOrderDto dto)
    {
        // Check if the supplier exists
        var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
        if (supplier == null)
        {
            return NotFound("Supplier not found.");
        }

        // Calculate the total amount of the order
        decimal totalAmount = dto.Items.Sum(item => item.Quantity * item.UnitPrice);

        var purchaseOrder = new PurchaseOrder
        {
            SupplierId = dto.SupplierId,
            OrderDate = dto.OrderDate,
            TotalAmount = totalAmount,
            Status = "Pending",
            Items = dto.Items.Select(item => new PurchaseOrderItem
            {
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        _context.PurchaseOrders.Add(purchaseOrder);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPurchaseOrderById), new { id = purchaseOrder.Id }, purchaseOrder);
    }

    // 2. Update Purchase Order Status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdatePurchaseOrderStatus(int id, UpdatePurchaseOrderStatusDto dto)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder == null)
        {
            return NotFound("Purchase order not found.");
        }

        // Update the status
        purchaseOrder.Status = dto.Status;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Get Purchase Order by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrderById(int id)
    {
        var purchaseOrder = await _context.PurchaseOrders
            .Include(po => po.Items)
            .Include(po => po.Supplier)
            .FirstOrDefaultAsync(po => po.Id == id);

        if (purchaseOrder == null)
        {
            return NotFound("Purchase order not found.");
        }

        return Ok(purchaseOrder);
    }
}
