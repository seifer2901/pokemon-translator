using Moq;
using PokemonTranslator.Abstractions;
using PokemonTranslator.Providers;
using System;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using PokemonTranslator.DTO;

namespace PokemonTranslator.Services.Tests
{
	public class PokemonTranslationServiceTests
	{
		[Theory]
		[InlineData("not-existing")]
		public async Task GetPokemon_NotFoundPokemon_ShouldReturnNull(string pokemonName)
		{
			var pokemonRepositoryService = new Mock<IPokemonRepositoryService>();
			var translationService = new Mock<ITranslationService>();
			pokemonRepositoryService.Setup(repo => repo.GetPokemon(pokemonName)).ReturnsAsync((PokemonRepositoryEntry)null);

			var pokemonTranslationService = new PokemonTranslationService(pokemonRepositoryService.Object, translationService.Object);

			var pokemon = await pokemonTranslationService.GetPokemon(pokemonName);

			pokemon.Should().BeNull();
		}

		[Theory]
		[InlineData("charizard")]
		[InlineData("pikachu")]
		public async Task GetPokemon_ValidExistingPokemonName_ShouldReturnPokemonTranslated(string pokemonName)
		{
			var expectedDescription = $"This is the description of {pokemonName}";
			var expectedTranslation = expectedDescription.ToUpper();
			var pokemonRepositoryService = new Mock<IPokemonRepositoryService>();
			var translationService = new Mock<ITranslationService>();
			pokemonRepositoryService.Setup(repo => repo.GetPokemon(pokemonName))
				.ReturnsAsync(
					new PokemonRepositoryEntry()
					{
						Name = pokemonName,
						Description = expectedDescription
					});
			translationService.Setup(service => service.Translate(It.IsAny<string>())).ReturnsAsync((string s) => s.ToUpper());
			var pokemonTranslationService = new PokemonTranslationService(pokemonRepositoryService.Object, translationService.Object);

			var pokemon = await pokemonTranslationService.GetPokemon(pokemonName);

			pokemon.Should().NotBeNull();
			pokemon.Name.Should().Equals(pokemonName);
			pokemon.Description.Should().Equals(expectedTranslation);
		}
	}
}
