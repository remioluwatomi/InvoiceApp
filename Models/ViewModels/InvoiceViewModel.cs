using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.Models;

public class InvoiceViewModel
{
    [Required(ErrorMessage = "Quantity is required.")]
    public required DateOnly InvoiceDate {get; set;}

    [Required(ErrorMessage = "Quantity is required.")]
    public int PaymentTerms {get; set;}

    [Required(ErrorMessage = "Quantity is required.")]
    public required string ProjectDescription {get; set;}
}
