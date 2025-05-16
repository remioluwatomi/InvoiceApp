namespace InvoiceApp.Models;

public class Biller
{

    public int Id { get; set; }
    public required string StreetAddress { get; set; }
    public required string City { get; set; }
    public required string PostCode { get; set; }
    public required string Country { get; set; }

    public virtual Invoice? Invoice { get; set; }

}
