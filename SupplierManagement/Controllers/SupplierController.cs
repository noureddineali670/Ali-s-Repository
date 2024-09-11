using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupplierManagement.Data;
using SupplierManagement.Models;


[Route("api/[controller]")]
[ApiController]
public class SupplierController : ControllerBase
{
    private readonly SupplierDbContext _context;

    public SupplierController(SupplierDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Supplier>> CreateSupplier(Supplier supplier)
    {
        if (await _context.Suppliers.AnyAsync(s => s.Email == supplier.Email || s.Phone == supplier.Phone))
        {
            return Conflict("A supplier with the same email or phone number already exists.");
        }

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> GetSupplierById(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return Ok(supplier);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSupplier(int id, Supplier supplier)
    {
        if (id != supplier.Id)
        {
            return BadRequest();
        }

        if (await _context.Suppliers.AnyAsync(s => (s.Email == supplier.Email || s.Phone == supplier.Phone) && s.Id != id))
        {
            return Conflict("Another supplier with the same email or phone number already exists.");
        }

        _context.Entry(supplier).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Suppliers.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
