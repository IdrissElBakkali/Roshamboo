using Microsoft.Extensions.Logging;
using Roshamboo.Store;

namespace Roshamboo.Services.Repository
{
	public class RepositoryBase
	{
		protected readonly ILogger _logger;
		protected RoshambooGameContext _gameContext;

		public RepositoryBase(ILogger logger, RoshambooGameContext gameContext)
		{
			_logger = logger;
			_gameContext = gameContext;
		}

		protected static void CheckNullInstance<TSource>(TSource instance) where TSource : class
		{
			if(instance is null)
			{
				throw new ArgumentNullException(nameof(instance));
			}
		}
	}
}
