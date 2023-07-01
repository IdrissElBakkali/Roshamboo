using Microsoft.AspNetCore.Mvc;
using Roshamboo.Core;
using Roshamboo.Core.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace Roshamboo.Controllers
{
	[Route("/roshamboo")]
	[ApiController]
	public class RoshambooGameController : ControllerBase
	{
		private readonly IRoshambooGameManagementService _roshambooGameService;

		public RoshambooGameController(IRoshambooGameManagementService roshambooGameService)
		{
			_roshambooGameService = roshambooGameService;
		}

		[HttpPost("CreateGame")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[SwaggerOperation(Summary = "Create new RoShamBoo game")]
		public async Task<ActionResult<string>> NewGame([FromForm] GameInfo info)
		{
			RoshambooGame ret = await _roshambooGameService.AddNewRoshambooGameAsync(info);
			if(ret == null)
			{
				return BadRequest();
			}

			return Content(ret.ToString());
		}

		[HttpPost("PlayGame")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[SwaggerOperation(Summary = "Play a hand for a RoShamBoo game")]
		public async Task<ActionResult<string>> UpdateGame([FromForm] UserPlayInfo info)
		{
			RoshambooGame ret = await _roshambooGameService.PlayRoshambooGameHandAsync(info);
			if (ret == null)
			{
				return BadRequest();
			}

			return Content(ret.ToString());
		}
	}
}
