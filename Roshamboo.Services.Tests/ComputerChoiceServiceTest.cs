using Roshamboo.Core;

namespace Roshamboo.Services.Tests
{
	public class ComputerChoiceServiceTest
	{
		private readonly IComputerChoiceService _computerChoiceService;

		public ComputerChoiceServiceTest()
		{
			_computerChoiceService = new ComputerChoiceService();
		}

		[Fact]
		public async Task ComputerShape_IsValidString()
		{
			// Arrange
			var validShapes = new[] { "Rock", "Paper", "Scissors" };

			// Act
			string computerShape = await _computerChoiceService.GetComputerShapeChoiceAsync();

			// Assert
			Assert.NotNull(computerShape);
			Assert.Contains(computerShape, validShapes);
		}
	}
}
