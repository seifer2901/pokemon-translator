using FluentAssertions;
using PokemonTranslator.Services.Translation.Shakespeare;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonTranslator.Services.IntegrationTests.Translation.Shakespeare
{
	public class ShakespeareTranslationServiceTests
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public void Translate_InvalidText_ShouldThrownArgumentExcpetion(string text)
		{
			var service = new ShakespeareTranslationService();

			Func<Task<string>> func = async () => await service.Translate(text);

			func.Should().Throw<ArgumentException>();
		}

		[Theory]
		[MemberData(nameof(GetValidTranslationTestData))]
		public async void Translate_ValidText_ShouldReturnTranslatedText(string text, string expectedTranslatedText)
		{
			var service = new ShakespeareTranslationService();

			var translatedText = await service.Translate(text);

			translatedText.Should().Be(expectedTranslatedText);
		}

		public static IEnumerable<object[]> GetValidTranslationTestData()
		{
			yield return new[]
			{
				"Spits fire that is hot enough to melt boulders. Known to cause forest fires unintentionally.",
				"Spits fireth yond is hot enow to melteth bould'rs.  Known to causeth f'rest fires unintentionally"
			};

			yield return new[]
			{
				"It can freely recombine its own cellular structure to transform into other life-forms.",
				"T can freely recombine its owneth cellular structureth to transf'rm into oth'r life-f'rms"
			};
		}
	}
}
