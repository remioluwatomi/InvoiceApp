namespace InvoiceApp.Models.DTOs;

public class InvoiceCardDTO
{
    public DateOnly InvoiceDate {get; set;}
    public required InvoiceStatus Status {get; set;}
    public required Guid Uid {get; set;}
    public string? ClientName {get; set;}
    public List<decimal>? ItemsPrices {get; set;} 
    
}
