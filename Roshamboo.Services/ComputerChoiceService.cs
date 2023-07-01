using Roshamboo.Core;
using Roshamboo.Core.Data;

namespace Roshamboo.Services
{
	public class ComputerChoiceService : IComputerChoiceService
	{
		public async Task<string> GetComputerShapeChoiceAsync()
		{
			return await Task.Factory.StartNew(() =>
			{
				Array values = Enum.GetValues(typeof(Shape));
				Random random = new Random();
				Shape randomShape = (Shape)values.GetValue(random.Next(values.Length));
				return randomShape.ToString();
			});
		}
	}
}
