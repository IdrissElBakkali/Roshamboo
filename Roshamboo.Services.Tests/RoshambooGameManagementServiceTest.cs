using Moq;
using Roshamboo.Core;
using Roshamboo.Core.Data;
using Roshamboo.Services.Repository;

namespace Roshamboo.Services.Tests
{
	public class RoshambooGameManagementServiceTest
	{
		private readonly RoshambooGameManagementService _gameManagementService;
		private readonly Mock<IComputerChoiceService> _computerChoiceServiceMock;
		private readonly Mock<IShapeComparisonStrategyService> _shapeComparisonStrategyServiceMock;
		private readonly Mock<IRoshambooGameRepository> _gameRepositoryMock;

		public RoshambooGameManagementServiceTest()
		{
			_computerChoiceServiceMock= new Mock<IComputerChoiceService>();
			_shapeComparisonStrategyServiceMock = new Mock<IShapeComparisonStrategyService>();
			_gameRepositoryMock = new Mock<IRoshambooGameRepository>();
			_gameManagementService = new RoshambooGameManagementService(_gameRepositoryMock.Object,
																		_computerChoiceServiceMock.Object,
																		_shapeComparisonStrategyServiceMock.Object);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(10)]
		public async Task AddNewRoshambooGame_ReturnsCreatedGame(int input)
		{
			// Arrange
			var gameInfo = new GameInfo(input);
			var game = new RoshambooGame(gameInfo.Rounds);
			_gameRepositoryMock.Setup(s => s.AddNewAsync(It.IsAny<RoshambooGame>()))
				.ReturnsAsync(game);

			// Act
			RoshambooGame createdGame = await _gameManagementService.AddNewRoshambooGameAsync(gameInfo);

			// Assert
			Assert.NotNull(createdGame);
			Assert.Equal(game.Id, createdGame.Id);
			Assert.Equal(input, createdGame.Rounds);
		}

		[Fact]
		public async Task AddNewRoshambooGame_WhenAddingInRepositoryFails_ReturnsNullGame()
		{
			// Arrange
			var gameInfo = new GameInfo(5);
			_gameRepositoryMock.Setup(s => s.AddNewAsync(It.IsAny<RoshambooGame>()))
				.ReturnsAsync((RoshambooGame)null);

			// Act
			RoshambooGame createdGame = await _gameManagementService.AddNewRoshambooGameAsync(gameInfo);

			// Assert
			Assert.Null(createdGame);
		}

		[Fact]
		public async Task PlayRoshambooGame_ReturnsUpdatedGame()
		{
			// Arrange
			var computerShape = "Rock";
			var userPlay = new UserPlayInfo
			{
				Id = "123",
				Shape = "Paper"
			};
			var oldGame = new RoshambooGame(5)
			{
				Id = "123",
				GameCounter = 1,
				UserScore = 0,
				ComputerScore = 0,
				UserShapes = new[] { Shape.Scissors },
				ComputerShapes = new[] { Shape.Scissors } 
			};
			var newGame = new RoshambooGame(5)
			{
				Id = "123",
				GameCounter = 2,
				UserScore = 1,
				ComputerScore = 0,
				CurrentUserShape = userPlay.Shape,
				CurrentComputerShape = computerShape,
				UserShapes = new[] { Shape.Scissors, Shape.Paper },
				ComputerShapes = new[] { Shape.Scissors, Shape.Rock }
			};
			_computerChoiceServiceMock.Setup(c => c.GetComputerShapeChoiceAsync())
				.ReturnsAsync(computerShape);
			_shapeComparisonStrategyServiceMock.Setup(c => c.CompareShapesAsync(userPlay.Shape, computerShape))
				.ReturnsAsync((1, 0));
			_gameRepositoryMock.Setup(s => s.GetAsync(userPlay.Id))
				.ReturnsAsync(oldGame);
			_gameRepositoryMock.Setup(s => s.UpdateAsync(It.IsAny<RoshambooGame>()))
				.ReturnsAsync(newGame);

			// Act
			RoshambooGame updatedGame = await _gameManagementService.PlayRoshambooGameHandAsync(userPlay);

			// Assert
			Assert.NotNull(updatedGame);
		}

		[Fact]
		public async Task PlayRoshambooGame_WhenFailsToGetGameFromRepository_ReturnsNull()
		{
			// Arrange
			var computerShape = "Rock";
			var userPlay = new UserPlayInfo
			{
				Id = "123",
				Shape = "Paper"
			};
			_computerChoiceServiceMock.Setup(c => c.GetComputerShapeChoiceAsync())
				.ReturnsAsync(computerShape);
			_shapeComparisonStrategyServiceMock.Setup(c => c.CompareShapesAsync(userPlay.Shape, computerShape))
				.ReturnsAsync((1, 0));
			_gameRepositoryMock.Setup(s => s.GetAsync(userPlay.Id))
				.ReturnsAsync((RoshambooGame)null);
			_gameRepositoryMock.Setup(s => s.UpdateAsync(It.IsAny<RoshambooGame>()))
				.ReturnsAsync(new RoshambooGame());

			// Act
			RoshambooGame updatedGame = await _gameManagementService.PlayRoshambooGameHandAsync(userPlay);

			// Assert
			Assert.Null(updatedGame);
		}

		[Fact]
		public async Task PlayRoshambooGame_WhenFailsToUpdateRepository_ReturnsNull()
		{
			// Arrange
			var computerShape = "Rock";
			var userPlay = new UserPlayInfo
			{
				Id = "123",
				Shape = "Paper"
			};
			var oldGame = new RoshambooGame(5)
			{
				Id = "123",
				GameCounter = 1,
				UserScore = 0,
				ComputerScore = 0,
				UserShapes = new[] { Shape.Scissors },
				ComputerShapes = new[] { Shape.Scissors }
			};
			_computerChoiceServiceMock.Setup(c => c.GetComputerShapeChoiceAsync())
				.ReturnsAsync(computerShape);
			_shapeComparisonStrategyServiceMock.Setup(c => c.CompareShapesAsync(userPlay.Shape, computerShape))
				.ReturnsAsync((1, 0));
			_gameRepositoryMock.Setup(s => s.GetAsync(userPlay.Id))
				.ReturnsAsync(oldGame);
			_gameRepositoryMock.Setup(s => s.UpdateAsync(It.IsAny<RoshambooGame>()))
				.ReturnsAsync((RoshambooGame)null);

			// Act
			RoshambooGame updatedGame = await _gameManagementService.PlayRoshambooGameHandAsync(userPlay);

			// Assert
			Assert.Null(updatedGame);
		}
	}
}
