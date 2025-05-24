using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InvoiceApp.Utility;

namespace InvoiceApp.Pages;

public class InvoiceIdValidatorBaseModelModel : PageModel
{
    public bool ValidateInvoiceId(string? id, out Guid validId)
    {
        (bool isValid, string? ErrorMessage) = GuidValidator.TryValidate(id, out validId);
        HandleValidationResposne(isValid, id ?? "Empty Id", ErrorMessage);
        return isValid;
    }
    
    public void HandleValidationResposne(bool isValid, string id, string? ErrorMessage)
    {
        if(!isValid)
        {
             Response.StatusCode = StatusCodes.Status400BadRequest;

             if(ErrorMessage is not null) TempData["ErrorMessage"] = ErrorMessage;
             else TempData["ErrorMessage"] = "We couldn't valid the invoice id";
        }
    }
}

