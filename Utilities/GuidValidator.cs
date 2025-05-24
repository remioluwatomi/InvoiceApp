using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace InvoiceApp.Utility;

public class GuidValidator
{
    public static (bool isValid, string? ErrorMessage) TryValidate(string? id, out Guid parsedId)
    {
         parsedId = Guid.Empty;
        if(string.IsNullOrEmpty(id))
        {
            return (false, "Invoice id is required");
        }

        if(!Guid.TryParse(id, out var validId))
        {
            return (false, "Invoice id has an invalid format");
        }

        parsedId = validId;
        return (true, "");

    }
}
