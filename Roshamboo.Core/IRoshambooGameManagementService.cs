using Roshamboo.Core.Data;

namespace Roshamboo.Core
{
	public interface IRoshambooGameManagementService
	{
		Task<RoshambooGame> AddNewRoshambooGameAsync(GameInfo gameInfo);
		Task<RoshambooGame> PlayRoshambooGameHandAsync(UserPlayInfo playInfo);
	}
}
