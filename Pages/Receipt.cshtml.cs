using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvoiceApp.Data;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models.DTOs;
using InvoiceApp.Models;
using InvoiceApp.Models.Forms;
using AutoMapper;
using System.Text.Json;


namespace InvoiceApp.Pages;

public class ReceiptModel : InvoiceIdValidatorBaseModelModel
{
    private readonly ILogger<ReceiptModel> _logger;
    private readonly InvoiceContext _context;
    private readonly  IMapper _mapper;
    private Guid _invoiceId {get; set;}

    public ReceiptDTO? Receipt {get; set;}

    [BindProperty]
    public InvoiceFormModels InvoiceForm {get; set;} = new();
    

    public ReceiptModel(ILogger<ReceiptModel> logger, InvoiceContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    public async Task InitializeReceipt()
    {
        Receipt = await GetInvoiceWithIdAsync(_invoiceId);
    }

    public async Task<IActionResult> OnGetAsync(string? id)
    {   
        try
        {
           bool isValidId = ValidateInvoiceId(id, out Guid validId);
           if(!isValidId) return Page();
           _invoiceId = validId;
            
           await InitializeReceipt();

            if(Receipt is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                TempData["ErrorMessage"] = "Invoice NOT FOUND";
                return Page();
            }

            Response.StatusCode = StatusCodes.Status200OK;
            var billerViewModel = _mapper.Map<BillerViewModel>(Receipt.Biller);

            var clientViewModel = _mapper.Map<ClientViewModel>(Receipt.Client);

            var invoiceViewModel = _mapper.Map<InvoiceViewModel>(Receipt.Invoice);

            var itemViewModels = _mapper.Map<List<ItemViewModel>>(Receipt.Items);

            InvoiceForm = new InvoiceFormModels(){
                Biller = billerViewModel,
                Client = clientViewModel,
                Invoice = invoiceViewModel,
                Items = itemViewModels
            };
            
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching invoice with ID: {InvoiceId}", id);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            TempData["ErrorMessage"] = "An error occurred while fetching the invoice.";
            return Page();
        }
    }

    public async Task<ReceiptDTO?> GetInvoiceWithIdAsync(Guid id)
    {
        var invoice = await _context.Invoice.SingleOrDefaultAsync(i => i.Uid == id);
      
        if(invoice is not null)
        {
            ReceiptDTO receiptDto = new() {
                Invoice = invoice,
                Client = invoice.Client!,
                Status = invoice.Status.ToString().ToLower(),
                Biller = invoice.Biller!,
                Items = invoice.Items.ToList(),
                PaymentDate = invoice.InvoiceDate.AddDays(invoice.PaymentTerms)
            }; 
            return receiptDto;
        }
        return null;
        
    }

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        bool isValidId = ValidateInvoiceId(id, out Guid validId);
        if(!isValidId) return Page();
        _invoiceId = validId;
        
        if (!ModelState.IsValid)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await InitializeReceipt();
            TempData["showInvoiceForm"] = true;
            return Page();
        }

        try
        {
            var existingInvoice = await _context.Invoice
                .Include(i => i.Biller)
                .Include(i => i.Client)
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Uid == validId);

            if (existingInvoice is null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                TempData["ErrorMessage"] = "Invoice NOT FOUND";
                return Page();
            }

            InvoiceStatus status = existingInvoice.Status;

            _mapper.Map(InvoiceForm.Invoice, existingInvoice);

            existingInvoice.Uid = validId;
            existingInvoice.Status = status;

            if (InvoiceForm.Items != null)
            {
                // Clear existing items
                existingInvoice.Items.Clear();
                
                // Add new items
                foreach (var item in InvoiceForm.Items)
                {
                    existingInvoice.Items.Add(_mapper.Map<Item>(item));
                }
            }

            _context.Invoice.Update(existingInvoice);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Invoice updated successfully";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating invoice");
            TempData["ErrorMessage"] = "An error occurred while updating the invoice";
            return Page();
        }
    }
}

                                                                                                                                                                                                    