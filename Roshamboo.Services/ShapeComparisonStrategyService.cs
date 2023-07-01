using Roshamboo.Core;

namespace Roshamboo.Services
{
	public class ShapeComparisonStrategyService : IShapeComparisonStrategyService
	{
		public async Task<(int userScore, int computerScore)> CompareShapesAsync(string userShape, string computerShape)
		{
			return await Task.Factory.StartNew(() =>
			{
				(int userScore, int computerScore) = (userShape, computerShape) switch
				{
					("Rock", "Paper") => (0, 1),
					("Rock", "Scissors") => (1, 0),
					("Paper", "Rock") => (1, 0),
					("Paper", "Scissors") => (0, 1),
					("Scissors", "Rock") => (0, 1),
					("Scissors", "Paper") => (1, 0),
					(_, _) => (0, 0)
				};

				return (userScore, computerScore);
			});
		}
	}
}
