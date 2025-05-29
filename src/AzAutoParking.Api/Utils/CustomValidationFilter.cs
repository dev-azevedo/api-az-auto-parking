using AzAutoParking.Application.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AzAutoParking.Api.Utils;

public class CustomValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Any())
            .SelectMany(x => x.Value.Errors)
            .Select(e => {
                var partes = e.ErrorMessage.Split(" | ");
                return new LocalizedMessage(
                    en: partes.ElementAtOrDefault(0) ?? "",
                    ptBr: partes.ElementAtOrDefault(1) ?? ""
                );
            }).ToList();

        var response = new ResultResponse<object>().
            Fail(StatusCodes.Status400BadRequest.GetHashCode(), errors);
            
        context.Result = new BadRequestObjectResult(response);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {}
}