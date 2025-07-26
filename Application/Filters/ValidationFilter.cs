using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Application.Responses;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var messages = context.ModelState
                .SelectMany(ms => ms.Value!.Errors.Select(e => e.ErrorMessage))
                .ToList();

            var errorMessage = string.Join("; ", messages);

            var result = Result<string>.Fail(errorMessage, 400);

            context.Result = new JsonResult(result)
            {
                StatusCode = 400
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
