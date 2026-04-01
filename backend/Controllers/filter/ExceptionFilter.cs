using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Controllers.filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException ex)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = ex.Message
                });
                return;
            }

            context.Result = new ObjectResult(new
            {
                message = "Something went wrong"
            })
            {
                StatusCode = 500
            };
        }
    }
}
