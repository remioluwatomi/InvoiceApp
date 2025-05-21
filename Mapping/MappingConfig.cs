using AutoMapper;
using InvoiceApp.Models;

namespace InvoiceApp.Mapping;

public class MappingConfigProfile: Profile
{
    public MappingConfigProfile()
    {
        CreateMap<Biller, BillerViewModel>().ReverseMap();
        CreateMap<Client, ClientViewModel>().ReverseMap();
        CreateMap<Invoice, InvoiceViewModel>().ReverseMap();;
        CreateMap<Item, ItemViewModel>().ReverseMap();
    }
}
