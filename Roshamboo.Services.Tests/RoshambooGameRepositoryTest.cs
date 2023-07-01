using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Roshamboo.Core.Data;
using Roshamboo.Services.Repository;
using Roshamboo.Services.Tests.Input;

namespace Roshamboo.Services.Tests
{
	public class RoshambooGameRepositoryTest
	{
		private readonly RoshambooGameRepository _gameRepository;

		public RoshambooGameRepositoryTest()
		{
			_gameRepository = new RoshambooGameRepository(Mock.Of<ILogger<RoshambooGameRepository>>(), new RoshambooGameContextStub());
		}

		[Theory]
		[InlineData("123")]
		[InlineData("456")]
		public async Task GetGame_RetunsRoshambooGame(string input)
		{
			// Act
			RoshambooGame game = await _gameRepository.GetAsync(input);

			// Assert
			Assert.NotNull(game);
			Assert.Equal(game.Id, input);
		}

		[Fact]
		public async Task GetGame_UnavailableId_RetunsNull()
		{
			// Arrange
			var gameId = Guid.NewGuid().ToString();

			// Act
			RoshambooGame game = await _gameRepository.GetAsync(gameId);

			// Assert
			Assert.Null(game);
		}

		[Fact]
		public async Task AddNewGame_ReturnsGame()
		{
			// Arrange
			var fixture = new Fixture();
			var game = fixture.Create<RoshambooGame>();

			// Act
			RoshambooGame addedGame = await _gameRepository.AddNewAsync(game);

			// Assert
			Assert.NotNull(addedGame);
			Assert.Equal(game.Id, addedGame.Id);
			Assert.Equal(game.Rounds, addedGame.Rounds);
			Assert.Equal(game.GameCounter, addedGame.GameCounter);
			Assert.Equal(game.UserScore, addedGame.UserScore);
			Assert.Equal(game.ComputerScore, addedGame.ComputerScore);
			Assert.Equal(game.UserShapes, addedGame.UserShapes);
			Assert.Equal(game.ComputerShapes, addedGame.ComputerShapes);
		}

		[Fact]
		public async Task AddNewGame_WhenNullGame_ThrowsException()
		{
			// Act & Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await _gameRepository.AddNewAsync(null));
		}

		[Fact]
		public async Task UpdateGame_ReturnsGame()
		{
			// Arrange
			var game = new RoshambooGame
			{
				Id = "123",
				Rounds = 5,
				GameCounter = 1,
				UserScore = 0,
				ComputerScore = 1,
				CurrentUserShape = Shape.Rock.ToString(),
				CurrentComputerShape = Shape.Paper.ToString(),
				UserShapes = new[] { Shape.Rock },
				ComputerShapes = new[] { Shape.Paper }
			};

			// Act
			RoshambooGame addedGame = await _gameRepository.UpdateAsync(game);

			// Assert
			Assert.NotNull(addedGame);
			Assert.Equal(game.Id, addedGame.Id);
			Assert.Equal(game.Rounds, addedGame.Rounds);
			Assert.Equal(game.GameCounter, addedGame.GameCounter);
			Assert.Equal(game.UserScore, addedGame.UserScore);
			Assert.Equal(game.ComputerScore, addedGame.ComputerScore);
		}

		[Fact]
		public async Task UpdateGame_WhenNullGame_ThrowsException()
		{
			// Act & Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await _gameRepository.UpdateAsync(null));
		}

		[Fact]
		public async Task UpdateGame_UnavailableId_RetunsNull()
		{
			// Arrange
			var game = new RoshambooGame
			{
				Id = Guid.NewGuid().ToString()
			};

			// Act
			RoshambooGame addedGame = await _gameRepository.UpdateAsync(game);

			// Assert
			Assert.Null(addedGame);
		}
	}
}
