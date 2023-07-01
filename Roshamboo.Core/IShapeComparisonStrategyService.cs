namespace Roshamboo.Core
{
	public interface IShapeComparisonStrategyService
	{
		Task<(int userScore, int computerScore)> CompareShapesAsync(string userShape, string computerShape);
	}
}
