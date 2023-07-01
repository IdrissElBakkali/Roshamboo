using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Roshamboo.Core.Data;
using System.Net;

namespace Roshamboo.Controllers
{
	[Route("error")]
	[ApiExplorerSettings(IgnoreApi = true)]
	[ApiController]
	public class ErrorsController : ControllerBase
	{
		public ErrorResponse Error()
		{
			IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();
			Exception exception = context?.Error;
			Response.StatusCode = exception switch
			{
				ArgumentNullException => (int)HttpStatusCode.NotFound,
				_ => (int)HttpStatusCode.InternalServerError
			};

			return new ErrorResponse(exception ?? new Exception());
		}
	}
}
