using FluentAssertions;
using PokemonTranslator.Abstractions.Services;
using PokemonTranslator.Services.Translation.Fake;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonTranslator.Services.IntegrationTests.Translation.Fake
{
	public class FakeTranslationServiceTests
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public void Translate_InvalidText_ShouldThrownArgumentExcpetion(string text)
		{
			var service = new FakeTranslationService();

			Func<Task<string>> func = async () => await service.Translate(text);

			func.Should().Throw<ArgumentException>();
		}

		[Theory]
		[InlineData("text to translate")]
		[InlineData(" TexT TO TRanslate")]
		public async void Translate_ValidText_ShouldReturnTranslatedText(string text)
		{
			var expectedTranslatedText = text.ToUpper();
			var service = new FakeTranslationService();

			var translatedText = await service.Translate(text);

			translatedText.Should().Be(expectedTranslatedText);
		}
	}
}
