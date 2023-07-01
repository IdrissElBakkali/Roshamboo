using Microsoft.AspNetCore.Mvc;
using Moq;
using Roshamboo.Controllers;
using Roshamboo.Core;
using Roshamboo.Core.Data;
using System.ComponentModel.DataAnnotations;

namespace Roshamboo.Tests
{
	public class RoshambooGameControllerTest
	{
		private readonly RoshambooGameController _controller;
		private readonly Mock<IRoshambooGameManagementService> _gameManagementServiceMock;

		public RoshambooGameControllerTest()
		{
			_gameManagementServiceMock = new Mock<IRoshambooGameManagementService>();
			_controller = new RoshambooGameController(_gameManagementServiceMock.Object);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(10)]
		public void Post_NewGame_ValidateInput_ReturnsTrue(int input)
		{
			// Arrange
			var gameInfo = new GameInfo(input);

			// Act
			IList<ValidationResult> errorsList = ValidateModel(gameInfo);

			// Assert
			Assert.True(!errorsList.Any());
		}

		[Theory]
		[InlineData(0)]
		[InlineData(11)]
		[InlineData(500)]
		public void Post_NewGame_OutOfRangeInput_ReturnsError(int input)
		{
			// Arrange
			var gameInfo = new GameInfo(input);

			// Act
			IList<ValidationResult> errorsList = ValidateModel(gameInfo);

			// Assert
			Assert.True(errorsList.Count() == 1);
			Assert.Equal("The field Rounds must be between 5 and 10.", errorsList.First().ErrorMessage);
		}

		[Fact]
		public void Post_UpdateGame_ValidateInput_ReturnsTrue()
		{
			// Arrange
			var userPlay = new UserPlayInfo
			{
				Id = Guid.NewGuid().ToString(),
				Shape = It.IsAny<Shape>().ToString()
			};

			// Act
			IList<ValidationResult> errorsList = ValidateModel(userPlay);

			// Assert
			Assert.True(!errorsList.Any());
		}

		[Fact]
		public void Post_UpdateGame_MissingShapeInput_ReturnsError()
		{
			// Arrange
			var userPlay = new UserPlayInfo
			{
				Id = Guid.NewGuid().ToString()
			};

			// Act
			IList<ValidationResult> errorsList = ValidateModel(userPlay);

			// Assert
			Assert.True(errorsList.Count() == 1);
			Assert.Equal("The Shape field is required.", errorsList.First().ErrorMessage);
		}

		[Fact]
		public void Post_UpdateGame_MissingIdInput_ReturnsError()
		{
			// Arrange
			var userPlay = new UserPlayInfo
			{
				Shape = It.IsAny<Shape>().ToString()
			};

			// Act
			IList<ValidationResult> errorsList = ValidateModel(userPlay);

			// Assert
			Assert.True(errorsList.Count() == 1);
			Assert.Equal("The Game Id field is required.", errorsList.First().ErrorMessage);
		}

		[Fact]
		public void Post_UpdateGame_MissingIdAndShapeInput_ReturnsError()
		{
			// Arrange
			var userPlay = new UserPlayInfo();

			// Act
			IList<ValidationResult> errorsList = ValidateModel(userPlay);

			// Assert
			Assert.True(errorsList.Count() == 2);
			Assert.Equal("The Game Id field is required.", errorsList.First().ErrorMessage);
			Assert.Equal("The Shape field is required.", errorsList.Skip(1).First().ErrorMessage);
		}

		[Fact]
		public async Task Post_NewGame_ReturnsContentResponse()
		{
			// Arrange
			var gameInfo = new GameInfo(5);
			_gameManagementServiceMock.Setup(s => s.AddNewRoshambooGameAsync(gameInfo))
				.ReturnsAsync(new RoshambooGame(gameInfo.Rounds));

			// Act
			ActionResult<string> contentResponse = await _controller.NewGame(gameInfo);

			// Assert
			Assert.NotNull(contentResponse);
			Assert.IsType<ContentResult>(contentResponse.Result);
		}

		[Fact]
		public async Task Post_NewGame_ReturnsCorrectContent()
		{
			// Arrange
			var gameInfo = new GameInfo(5);
			_gameManagementServiceMock.Setup(s => s.AddNewRoshambooGameAsync(gameInfo))
				.ReturnsAsync(new RoshambooGame(gameInfo.Rounds, "123"));

			// Act
			ActionResult<string> contentResponse = await _controller.NewGame(gameInfo);
			var contentResult = contentResponse.Result as ContentResult;

			// Assert
			Assert.NotNull(contentResult);
			Assert.Equal("Game \"123\" started, user score: 0, computer score: 0", contentResult.Content);
		}

		[Fact]
		public async Task Post_NewGame_ErrorWhileAddingToCollection_ResturnsBadRequest()
		{
			// Arrange
			_gameManagementServiceMock.Setup(s => s.AddNewRoshambooGameAsync(It.IsAny<GameInfo>()))
				.ReturnsAsync((RoshambooGame)null);

			// Act
			ActionResult<string> badResponse = await _controller.NewGame(new GameInfo());
			ActionResult? badResponseResult = badResponse?.Result;

			// Assert
			Assert.IsType<BadRequestResult>(badResponseResult);
		}

		[Fact]
		public async Task Post_UpdateGame_ReturnsContentResponse()
		{
			// Arrange
			var userPlay = new UserPlayInfo
			{
				Id = "123",
				Shape = "Paper"
			};
			_gameManagementServiceMock.Setup(s => s.PlayRoshambooGameHandAsync(userPlay))
				.ReturnsAsync(new RoshambooGame(5)
				{
					Id = "123",
					GameCounter = 1,
					UserShapes = new[] { Shape.Paper },
					ComputerShapes = new[] { Shape.Rock },
					UserScore = 1,
					ComputerScore = 0
				});

			// Act
			ActionResult<string> contentResponse = await _controller.UpdateGame(userPlay);

			// Assert
			Assert.NotNull(contentResponse);
			Assert.IsType<ContentResult>(contentResponse.Result);
		}

		[Fact]
		public async Task Post_UpdateGame_ReturnsCorrectContent()
		{
			// Arrange
			var userPlay = new UserPlayInfo
			{
				Id = "123",
				Shape = "Paper"
			};
			_gameManagementServiceMock.Setup(s => s.PlayRoshambooGameHandAsync(userPlay))
				.ReturnsAsync(new RoshambooGame(5)
				{
					Id = "123",
					GameCounter = 1,
					UserShapes = new[] { Shape.Paper },
					ComputerShapes = new[] { Shape.Rock },
					UserScore = 1,
					ComputerScore = 0
				});

			// Act
			ActionResult<string> contentResponse = await _controller.UpdateGame(userPlay);
			var contentResult = contentResponse.Result as ContentResult;

			// Assert
			Assert.NotNull(contentResult);
			Assert.Equal("Round 1/5, user played \"Paper\", computer played \"Rock\", user score: 1, computer score: 0", contentResult.Content);
		}

		[Fact]
		public async Task Post_UpdateGame_ErrorWhileUpdatingCollection_ReturnsBadRequest()
		{
			// Arrange
			_gameManagementServiceMock.Setup(s => s.PlayRoshambooGameHandAsync(It.IsAny<UserPlayInfo>()))
				.ReturnsAsync((RoshambooGame)null);

			// Act
			ActionResult<string> badResponse = await _controller.UpdateGame(new UserPlayInfo());
			var badResponseResult = badResponse.Result;

			// Assert
			Assert.IsType<BadRequestResult>(badResponseResult);
		}

		private IList<ValidationResult> ValidateModel(object model)
		{
			var validationResults = new List<ValidationResult>();
			var ctx = new ValidationContext(model, null, null);
			Validator.TryValidateObject(model, ctx, validationResults, true);
			return validationResults;
		}
	}
}
