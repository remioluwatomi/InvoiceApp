namespace InvoiceApp.Models;

public class Invoice
{

    public int Id {get; set;}
    public DateOnly InvoiceDate{get; set;}
    public int PaymentTerms {get; set;}
    public required string ProjectDescription {get; set;}
    public required InvoiceStatus Status {get; set;}
    public required Guid Uid {get; set;}

    public virtual required ICollection<Item> Items { get; set; }
    public virtual required Client Client {get; set;}
    public virtual required Biller Biller {get; set;} 
}

public enum InvoiceStatus
{
    Pending,
    Approved,
    Draft
}
