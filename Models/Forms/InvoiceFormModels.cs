using InvoiceApp.Models.DTOs;

namespace InvoiceApp.Models.Forms;

public class InvoiceFormModels
{
    public BillerViewModel? Biller {get; set;}
    public ClientViewModel? Client {get; set;}
    public List<ItemViewModel> Items {get; set;} = new(){new ItemViewModel()};
    public InvoiceViewModel? Invoice {get; set;}
}
