using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.Models;

public class ClientViewModel
{
    public required string ClientName {get; set;}
    [EmailAddress]
    public required string ClientEmail {get; set;}
    public required string StreetAddress {get; set;}
    public required string City {get; set;}
    public required string PostCode {get; set;}
    public required string Country {get; set;}
}
