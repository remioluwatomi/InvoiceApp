using InvoiceApp.Models;

namespace InvoiceApp.Models.DTOs;

public class ReceiptDTO
{
    public required Invoice Invoice {get; set;}
    public DateOnly PaymentDate {get; set;}
    public string? Status {get; set;}
    public required Client Client {get; set;}
    public required Biller Biller {get; set;}
    public required List<Item> Items {get; set;}
}
