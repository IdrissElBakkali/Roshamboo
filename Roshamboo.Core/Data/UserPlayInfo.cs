using System.ComponentModel.DataAnnotations;

namespace Roshamboo.Core.Data
{
	public class UserPlayInfo : IValidatableObject
	{
		[Required]
		[Display(Name = "Game Id")]
		public string Id { get; init; }

		[Required]
		[Display(Name = "Shape")]
		public string Shape { get; init; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(Enum.IsDefined(typeof(Shape), Shape) == false)
			{
				yield return new ValidationResult(
					$"Unknown shape chosen: {Shape}. Please pick one of the following: {string.Join(", ", Enum.GetNames(typeof(Shape)))}",
					new[] { nameof(Shape) });
			}
		}
	}
}
