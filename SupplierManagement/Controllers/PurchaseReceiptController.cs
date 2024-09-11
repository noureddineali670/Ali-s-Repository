using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data;
using SupplierManagement.Models.DTOs;
using SupplierManagement.Models;
using SupplierManagementModels.DTOs;

[Route("api/[controller]")]
[ApiController]
public class PurchaseReceiptController : ControllerBase
{
    private readonly SupplierDbContext _context;

    public PurchaseReceiptController(SupplierDbContext context)
    {
        _context = context;
    }

    // 1. Create Purchase Receipt
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseReceipt(CreatePurchaseReceiptDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check Purchase Order and other validation logic...

        // Create the Purchase Receipt
        var receipt = new PurchaseReceipt
        {
            PurchaseOrderId = dto.PurchaseOrderId,
            ReceiptDate = dto.ReceiptDate,
            Items = dto.Items.Select(item => new PurchaseReceiptItem
            {
                PurchaseOrderItemId = item.PurchaseOrderItemId,
                ReceivedQuantity = item.ReceivedQuantity
            }).ToList()
        };

        _context.PurchaseReceipts.Add(receipt);
        await _context.SaveChangesAsync();

        // Create corresponding Item Ledger Entries
        foreach (var item in receipt.Items)
        {
            var itemLedgerEntry = new ItemLedgerEntry
            {
                PurchaseOrderItemId = item.PurchaseOrderItemId,
                EntryDate = DateTime.Now,
                EntryType = "Receipt",  // We are recording a receipt here
                Quantity = item.ReceivedQuantity,
                Remarks = "Item received in purchase receipt"
            };

            _context.ItemLedgerEntries.Add(itemLedgerEntry);
        }

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPurchaseReceiptById), new { id = receipt.Id }, receipt);
    }


    // 2. View Purchase Receipt by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseReceiptDetailsDto>> GetPurchaseReceiptById(int id)
    {
        var receipt = await _context.PurchaseReceipts
            .Include(pr => pr.Items)
            .ThenInclude(pri => pri.PurchaseOrderItem)
            .FirstOrDefaultAsync(pr => pr.Id == id);

        if (receipt == null)
        {
            return NotFound("Purchase receipt not found.");
        }

        var receiptDetails = new PurchaseReceiptDetailsDto
        {
            Id = receipt.Id,
            PurchaseOrderId = receipt.PurchaseOrderId,
            ReceiptDate = receipt.ReceiptDate,
            Items = receipt.Items.Select(item => new PurchaseReceiptItemDetailsDto
            {
                PurchaseOrderItemId = item.PurchaseOrderItemId,
                ProductName = item.PurchaseOrderItem.ProductName,
                OrderedQuantity = item.PurchaseOrderItem.Quantity,
                ReceivedQuantity = item.ReceivedQuantity
            }).ToList()
        };

        return Ok(receiptDetails);
    }

    [HttpGet]
    public async Task<IActionResult> GetItemLedgerEntries()
    {
        var entries = await _context.ItemLedgerEntries
            .Include(ile => ile.PurchaseOrderItem)
            .ToListAsync();

        return Ok(entries);
    }


}
