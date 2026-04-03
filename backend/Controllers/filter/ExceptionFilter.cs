using backend.Entity.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace backend.Controllers.filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            switch (exception)
            {
                case EntityNotFoundException:
                    context.Result = new NotFoundObjectResult(new
                    {
                        message = exception.Message
                    });
                    return;

                case ForbiddenAccessException:
                    context.Result = new ObjectResult(new
                    {
                        message = exception.Message
                    })
                    {
                        StatusCode = 403
                    };
                    return;

                case ArgumentException:
                    context.Result = new BadRequestObjectResult(new
                    {
                        message = exception.Message
                    });
                    return;

                case ResourceConflictException:
                    context.Result = new ConflictObjectResult(new
                    {
                        message = exception.Message
                    });
                    return;

                default:
                    context.Result = new ObjectResult(new
                    {
                        message = "Something went wrong"
                    })
                    {
                        StatusCode = 500
                    };
                    return;

            }

            
        }
    }
}
