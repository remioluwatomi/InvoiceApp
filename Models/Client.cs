using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.Models;

public class Client
{

    public int Id { get; set; }
    public required string ClientName { get; set; }
    [EmailAddress]
    public required string ClientEmail { get; set; }
    public required string StreetAddress { get; set; }
    public required string City { get; set; }
    public required string PostCode { get; set; }
    public required string Country { get; set; }

    public virtual Invoice? Invoice { get; set; }

}
