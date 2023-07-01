using Microsoft.Extensions.Logging;
using Roshamboo.Core.Data;
using Roshamboo.Services.Repository.Mapping;
using Roshamboo.Store;
using Roshamboo.Store.Models;

namespace Roshamboo.Services.Repository
{
	public interface IRoshambooGameRepository
	{
		Task<RoshambooGame> GetAsync(string gameId);
		Task<RoshambooGame> AddNewAsync(RoshambooGame game);
		Task<RoshambooGame> UpdateAsync(RoshambooGame game);
	}

	public class RoshambooGameRepository : RepositoryBase, IRoshambooGameRepository
	{
		public RoshambooGameRepository(ILogger<RoshambooGameRepository> logger, RoshambooGameContext gameContext) : base(logger, gameContext) { }

		public async Task<RoshambooGame> GetAsync(string gameId)
		{
			return await Task.Factory.StartNew(() => _gameContext.RoshambooGames.TryGetValue(gameId, out RoshambooGameDb gameDb) ? gameDb.ToModel() : null);
		}

		public async Task<RoshambooGame> AddNewAsync(RoshambooGame game)
		{
			CheckNullInstance(game);
			RoshambooGameDb gameDb = game.ToDb();
			return await Task.Run(async () =>
			{
				if(_gameContext.RoshambooGames.TryAdd(gameDb.Id, gameDb))
				{
					_logger.LogInformation($"Game Id ={gameDb.Id} created.");
					RoshambooGame ret = await GetAsync(gameDb.Id);
					return ret;
				}

				_logger.LogError($"Error while trying to create Game Id = {gameDb.Id}.");
				return null;
			});
		}

		public async Task<RoshambooGame> UpdateAsync(RoshambooGame game)
		{
			CheckNullInstance(game);
			return await Task.Run(async () =>
			{
				if(_gameContext.RoshambooGames.TryGetValue(game.Id, out RoshambooGameDb oldGameDb))
				{
					RoshambooGameDb newGameDb = game.ToDb();
					newGameDb.Rounds = oldGameDb.Rounds;
					newGameDb.UserShapes = oldGameDb.UserShapes;
					newGameDb.ComputerShapes = oldGameDb.ComputerShapes;
					newGameDb.UserShapes[newGameDb.GameCounter - 1] = game.CurrentUserShape;
					newGameDb.ComputerShapes[newGameDb.GameCounter - 1] = game.CurrentComputerShape;
					if(_gameContext.RoshambooGames.TryUpdate(game.Id, newGameDb, oldGameDb))
					{
						_logger.LogInformation($"Game Id ={game.Id} updated. User shape: {game.CurrentUserShape}, computer shape: {game.CurrentComputerShape}.");
						RoshambooGame ret = await GetAsync(game.Id);
						return ret;
					}

					_logger.LogError($"Error while trying to update Game Id ={game.Id}.");
					return null;
				}

				_logger.LogError($"Error while trying to update Game Id ={game.Id}. Unable to find the game Id the the existing game collection.");
				return null;
			});
		}
	}
}
