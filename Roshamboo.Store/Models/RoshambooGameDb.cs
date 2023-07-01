namespace Roshamboo.Store.Models
{
	public class RoshambooGameDb
	{
		public RoshambooGameDb()
		{
			UserShapes = new string[Rounds];
			ComputerShapes = new string[Rounds];
		}

		public string Id { get; set; }
		public int Rounds { get; set; }
		public int GameCounter { get; set; }
		public int UserScore { get; set; }
		public int ComputerScore { get; set; }
		public string[] UserShapes { get; set; }
		public string[] ComputerShapes { get; set; }
	}
}
