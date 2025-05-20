using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Models;

public class ItemViewModel
{
    [Required(ErrorMessage = "Item Name is required.")]
    public string? Name {get; set;}

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, double.MaxValue, ErrorMessage = "Quantity must be greater than 1.")]
    public int Quantity {get; set;}

    [Precision(18, 2)]
    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price {get; set;}
}
