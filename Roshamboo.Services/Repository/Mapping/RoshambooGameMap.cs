using Roshamboo.Core.Data;
using Roshamboo.Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roshamboo.Services.Repository.Mapping
{
	public static class RoshambooGameMap
	{
		public static RoshambooGame ToModel(this RoshambooGameDb dto)
		{
			if(dto == null)
			{
				return null;
			}

			var ret = new RoshambooGame(dto.Rounds, dto.Id)
			{
				GameCounter = dto.GameCounter,
				UserScore = dto.UserScore,
				ComputerScore = dto.ComputerScore,
				UserShapes = dto.UserShapes?.ToShapesArray(),
				ComputerShapes = dto.ComputerShapes?.ToShapesArray()
			};

			return ret;
		}

		public static RoshambooGameDb ToDb(this RoshambooGame game)
		{
			if (game == null)
			{
				return null;
			}

			var ret = new RoshambooGameDb
			{
				Id = game.Id,
				Rounds = game.Rounds,
				GameCounter = game.GameCounter,
				UserScore = game.UserScore,
				ComputerScore = game.ComputerScore,
				UserShapes = game.UserShapes?.Select(s => s.ToString()).ToArray(),
				ComputerShapes = game.ComputerShapes?.Select(s => s.ToString()).ToArray()
			};

			return ret;
		}

		private static Shape[] ToShapesArray(this string[] stringShapes)
		{
			return stringShapes.Where(s => s != null && Enum.IsDefined(typeof(Shape), s))
								.Select(Enum.Parse<Shape>)
								.ToArray();
		}
	}
}
