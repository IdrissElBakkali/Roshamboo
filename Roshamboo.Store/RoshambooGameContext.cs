using Roshamboo.Store.Models;
using System.Collections.Concurrent;

namespace Roshamboo.Store
{
	public class RoshambooGameContext
	{
		public RoshambooGameContext()
		{
			RoshambooGames = new ConcurrentDictionary<string, RoshambooGameDb>();
		}

		public ConcurrentDictionary<string, RoshambooGameDb> RoshambooGames { get; set; }
	}
}
