using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Models;

public class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }

    [Precision(18, 2)]
    public decimal Price { get; set; }

    public int InvoiceId { get; set; }

    public virtual Invoice? Invoice { get; set; }
}
