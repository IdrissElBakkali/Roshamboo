using AutoFixture;
using Roshamboo.Store;
using System.Collections.Concurrent;
using Roshamboo.Store.Models;

namespace Roshamboo.Services.Tests.Input
{
	public class RoshambooGameContextStub : RoshambooGameContext
	{
		public RoshambooGameContextStub()
		{
			var fixture = new Fixture();
			RoshambooGames = new ConcurrentDictionary<string, Store.Models.RoshambooGameDb>()
			{
				["123"] = new()
				{
					Id = "123",
					Rounds = 5,
					GameCounter = 0,
					UserScore = 0,
					ComputerScore = 0,
					UserShapes = new string[5],
					ComputerShapes = new string[5]
				},
				[Guid.NewGuid().ToString()] = fixture.Create<RoshambooGameDb>(),
				[Guid.NewGuid().ToString()] = fixture.Create<RoshambooGameDb>(),
				[Guid.NewGuid().ToString()] = fixture.Create<RoshambooGameDb>(),
				[Guid.NewGuid().ToString()] = fixture.Create<RoshambooGameDb>(),
				[Guid.NewGuid().ToString()] = fixture.Create<RoshambooGameDb>(),
				[Guid.NewGuid().ToString()] = fixture.Create<RoshambooGameDb>(),
				[Guid.NewGuid().ToString()] = fixture.Create<RoshambooGameDb>(),
				["456"] = new() { Id = "456", Rounds = 6 }
			};
		}
	}
}
