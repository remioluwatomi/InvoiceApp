using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvoiceApp.Data;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models;

namespace InvoiceApp.Pages;

public class DeleteInvoiceModel: InvoiceIdValidatorBaseModelModel
{
    private readonly ILogger<DeleteInvoiceModel> _logger;
    private readonly InvoiceContext _context;


    public DeleteInvoiceModel(ILogger<DeleteInvoiceModel> logger, InvoiceContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult OnGet()
    {
        return StatusCode(StatusCodes.Status405MethodNotAllowed);
    }


    public async Task<IActionResult> OnPost()
    {
        string? id = Request.Form["InvoiceId"];
        try
        {
        bool isValidId = ValidateInvoiceId(id, out Guid validId);
        if(!isValidId) return RedirectToPage("error/400");

        await RemoveInvoice(validId);

        return RedirectToPage("/index");
        }
        catch (Exception ex)
        {
             _logger.LogError(ex, "Error fetching invoice with ID: {InvoiceId}", id);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return RedirectToPage("error/500");
        }
    }

    public async Task RemoveInvoice(Guid id)
    {
        var invoice = await _context.Invoice.SingleOrDefaultAsync(i => i.Uid == id);
        _context.Remove<Invoice>(invoice);
        await _context.SaveChangesAsync();
    }

}
