using System.ComponentModel.DataAnnotations;

namespace Roshamboo.Core.Data
{
	public class GameInfo
	{
		public GameInfo() { }

		public GameInfo(int rounds)
		{
			Rounds = rounds;
		}

		[Required]
		[Display(Name = "Rounds")]
		[Range(5, 10)]
		public int Rounds { get; init; }
	}

	public class RoshambooGame : GameInfo
	{
		public RoshambooGame() { }

		public RoshambooGame(int rounds, string gameId = null) : base(rounds)
		{
			Id = gameId ?? Guid.NewGuid().ToString();
			UserShapes = new Shape[rounds];
			ComputerShapes = new Shape[rounds];
		}

		public int GameCounter { get; set; }
		public string Id { get; init; }
		public string CurrentUserShape { get; init; }
		public string CurrentComputerShape { get; init; }
		public int UserScore { get; set; }
		public int ComputerScore { get; set; }
		public Shape[] UserShapes { get; set; }
		public Shape[] ComputerShapes { get; set; }

		public override string ToString()
		{
			// Game starts
			if(GameCounter == 0)
			{
				return $"Game \"{Id}\" started, user score: {UserScore}, computer score: {ComputerScore}";
			}

			//Game ends
			if(GameCounter.CompareTo(Rounds) == 0)
			{
				string winner = UserScore.CompareTo(ComputerScore) switch
				{
					< 0 => "Computer wins",
					> 0 => "User wins",
					_ => "It's a tie"
				};

				string rounds = string.Join(", ", UserShapes.Zip(ComputerShapes, (userShape, computerShape) => $"(User: \"{userShape}\", Computer: \"{computerShape}\")"));
				return $"Game \"{Id}\" ended. {winner}. User score: {UserScore}, computer score: {ComputerScore}, {Environment.NewLine}rounds: {rounds}";
			}

			// Game ongoing
			return $"Round {GameCounter}/{Rounds}, user played \"{UserShapes[GameCounter - 1]}\", computer played \"{ComputerShapes[GameCounter - 1]}\", " +
				   $"user score: {UserScore}, computer score: {ComputerScore}";
		}
	}
}
