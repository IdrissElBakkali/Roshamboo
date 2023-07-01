using Roshamboo.Core;

namespace Roshamboo.Services.Tests
{
	public class ShapeComparisonStrategyServiceTest
	{
		private readonly IShapeComparisonStrategyService _shapeComparisonStrategyService;

		public ShapeComparisonStrategyServiceTest()
		{
			_shapeComparisonStrategyService = new ShapeComparisonStrategyService();
		}

		[Fact]
		public async Task CompareShapes_UserRock_ComputerPaper_ReturnsComputerWins()
		{
			// Arrange
			var userShape = "Rock";
			var computerShape = "Paper";

			// Act
			var scores = await _shapeComparisonStrategyService.CompareShapesAsync(userShape, computerShape);

			// Assert
			Assert.Equal(0, scores.userScore);
			Assert.Equal(1, scores.computerScore);
		}

		[Fact]
		public async Task CompareShapes_UserRock_ComputerScissors_ReturnsUserWins()
		{
			// Arrange
			var userShape = "Rock";
			var computerShape = "Scissors";

			// Act
			var scores = await _shapeComparisonStrategyService.CompareShapesAsync(userShape, computerShape);

			// Assert
			Assert.Equal(1, scores.userScore);
			Assert.Equal(0, scores.computerScore);
		}

		[Fact]
		public async Task CompareShapes_UserPaper_ComputerRock_ReturnsUserWins()
		{
			// Arrange
			var userShape = "Paper";
			var computerShape = "Rock";

			// Act
			var scores = await _shapeComparisonStrategyService.CompareShapesAsync(userShape, computerShape);

			// Assert
			Assert.Equal(1, scores.userScore);
			Assert.Equal(0, scores.computerScore);
		}

		[Fact]
		public async Task CompareShapes_UserPaper_ComputerScissors_ReturnsComputerWins()
		{
			// Arrange
			var userShape = "Paper";
			var computerShape = "Scissors";

			// Act
			var scores = await _shapeComparisonStrategyService.CompareShapesAsync(userShape, computerShape);

			// Assert
			Assert.Equal(0, scores.userScore);
			Assert.Equal(1, scores.computerScore);
		}

		[Fact]
		public async Task CompareShapes_UserScissors_ComputerRock_ReturnsComputerWins()
		{
			// Arrange
			var userShape = "Scissors";
			var computerShape = "Rock";

			// Act
			var scores = await _shapeComparisonStrategyService.CompareShapesAsync(userShape, computerShape);

			// Assert
			Assert.Equal(0, scores.userScore);
			Assert.Equal(1, scores.computerScore);
		}

		[Fact]
		public async Task CompareShapes_UserScissors_ComputerPaper_ReturnsUserWins()
		{
			// Arrange
			var userShape = "Scissors";
			var computerShape = "Paper";

			// Act
			var scores = await _shapeComparisonStrategyService.CompareShapesAsync(userShape, computerShape);

			// Assert
			Assert.Equal(1, scores.userScore);
			Assert.Equal(0, scores.computerScore);
		}

		[Fact]
		public async Task CompareShapes_UserComputerSameShape_ReturnsTie()
		{
			// Arrange
			var userShape = "Scissors";
			var computerShape = "Scissors";

			// Act
			var scores = await _shapeComparisonStrategyService.CompareShapesAsync(userShape, computerShape);

			// Assert
			Assert.Equal(0, scores.userScore);
			Assert.Equal(0, scores.computerScore);
		}
	}
}
