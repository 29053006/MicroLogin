using Facture.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Facture.Core.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = "The request is invalid.";
                var errors = context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                var error = new ErrorResponse(code: ErrorResponseCode.ArgumentsCauseError, message: message, details: errors.ToArray());

                // Devolver una respuesta de error personalizada con el código de estado 400 (Bad Request)
                context.Result = new BadRequestObjectResult(error);
            }
        }
    }
}
