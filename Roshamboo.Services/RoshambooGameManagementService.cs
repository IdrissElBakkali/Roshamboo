using Roshamboo.Core;
using Roshamboo.Core.Data;
using Roshamboo.Services.Repository;

namespace Roshamboo.Services
{
	public class RoshambooGameManagementService : IRoshambooGameManagementService
	{
		private readonly IComputerChoiceService _computerChoiceService;
		private readonly IShapeComparisonStrategyService _shapeComparisonStrategyService;
		private readonly IRoshambooGameRepository _gameRepository;

		public RoshambooGameManagementService(
			IRoshambooGameRepository gameRepository,
			IComputerChoiceService computerChoiceService, 
			IShapeComparisonStrategyService shapeComparisonStrategyService)
		{
			_gameRepository = gameRepository;
			_computerChoiceService = computerChoiceService;
			_shapeComparisonStrategyService = shapeComparisonStrategyService;
		
		}

		public async Task<RoshambooGame> AddNewRoshambooGameAsync(GameInfo gameInfo)
		{
			var game = new RoshambooGame(gameInfo.Rounds);
			return await _gameRepository.AddNewAsync(game);
		}

		public async Task<RoshambooGame> PlayRoshambooGameHandAsync(UserPlayInfo playInfo)
		{
			RoshambooGame roshambooGame = await _gameRepository.GetAsync(playInfo.Id);
			if(roshambooGame == null)
			{
				return null;
			}

			bool isGameCompleted = roshambooGame.Rounds.CompareTo(roshambooGame.GameCounter) == 0;
			if (isGameCompleted)
			{
				return roshambooGame;
			}

			string computerShape = await _computerChoiceService.GetComputerShapeChoiceAsync();
			(int userScore, int computerScore) scores = await _shapeComparisonStrategyService.CompareShapesAsync(playInfo.Shape, computerShape);

			var game = new RoshambooGame
			{
				Id = playInfo.Id,
				GameCounter = roshambooGame.GameCounter + 1,
				UserScore = roshambooGame.UserScore + scores.userScore,
				ComputerScore = roshambooGame.ComputerScore + scores.computerScore,
				CurrentUserShape = playInfo.Shape,
				CurrentComputerShape = computerShape
			};

			return await _gameRepository.UpdateAsync(game);
		}
	}
}
