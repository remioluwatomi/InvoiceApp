using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvoiceApp.Models;
using InvoiceApp.Data;
using InvoiceApp.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using InvoiceApp.Models.Forms;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceApp.Pages;


public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly InvoiceContext _context;

    // public required BillerViewModel Biller {get; set;}
    // public required ClientViewModel Client {get; set;}
    // public List<ItemViewModel> Items {get; set;} = new(){new ItemViewModel()};
    // public required InvoiceViewModel Invoice {get; set;}
    public List<InvoiceCardDTO>? InvoiceCards {get; set;}
    public readonly string FormSubmitPage = "/";

    [BindProperty]
    public InvoiceFormModels InvoiceForm {get; set;} = new();


    public IndexModel(ILogger<IndexModel> logger, InvoiceContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializePage()
    {
        InvoiceCards = await GetInvoiceCardsAsync();
    }

    public async Task<List<InvoiceCardDTO>> GetInvoiceCardsAsync()
    {
        //invoice db table not expected to expand into thousands of rows, hence performance not extensively considered for the data fetching..
        var invoices = await _context.Invoice.ToListAsync();
        var dtos = new List<InvoiceCardDTO>{};

        foreach (var invoice in invoices)
        {
            string clName = invoice.Client.ClientName;

            dtos.Add(new InvoiceCardDTO{
                InvoiceDate = invoice.InvoiceDate,
                Status = invoice.Status,
                Uid = invoice.Uid,
                ClientName = clName,
                TotalPrice = invoice.Items.Sum(item => item.Price * item.Quantity)
            });
        }

        return dtos;
    }

    public async Task<IActionResult> OnGetAsync()
    {
       try
       {
            await InitializePage();
            Response.StatusCode = StatusCodes.Status200OK;
            
       }
       catch (System.Exception ex)
       {
            _logger.LogError($"Unable to fetch Invoices at the moment: {ex}");
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            //other error handling code to be included 
       }

       return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        
        try
        {
            if(!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                TempData["showInvoiceForm"] = true;
                await InitializePage();
                return Page();
            }
            
            TempData["showInvoiceForm"] = false;

            Biller biller = new()
            {
                StreetAddress = InvoiceForm.Biller.StreetAddress,
                City = InvoiceForm.Biller.City,
                PostCode = InvoiceForm.Biller.PostCode,
                Country = InvoiceForm.Biller.Country,
            };

            Client client = new()
            {
                ClientName = InvoiceForm.Client.ClientName,
                ClientEmail = InvoiceForm.Client.ClientEmail,
                StreetAddress = InvoiceForm.Client.StreetAddress,
                City = InvoiceForm.Client.City,
                PostCode = InvoiceForm.Client.PostCode,
                Country = InvoiceForm.Client.Country,
            };

            List<Item> items = new List<Item>();

            Invoice invoice = new()
            {
                InvoiceDate = InvoiceForm.Invoice.InvoiceDate,
                PaymentTerms = InvoiceForm.Invoice.PaymentTerms,
                ProjectDescription = InvoiceForm.Invoice.ProjectDescription,
                Status = InvoiceStatus.Pending,
                Uid = Guid.NewGuid(),
                Items = items,
                Client = client,
                Biller = biller,
            };


            foreach (ItemViewModel item in InvoiceForm.Items)
            {
                Item itemModel = new()
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Invoice = invoice,
                };

                items.Add(itemModel);
            };

            biller.Invoice = invoice;
            client.Invoice = invoice;
            invoice.Items = items;

            await _context.Invoice.AddAsync(invoice);
            await _context.SaveChangesAsync();
        
            
        }
        catch (System.Exception)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            throw;
        }    
        return RedirectToPage("/index");

    }
    
}
