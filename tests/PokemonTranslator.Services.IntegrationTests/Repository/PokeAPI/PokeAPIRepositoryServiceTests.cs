using FluentAssertions;
using PokemonTranslator.Abstractions.Services;
using PokemonTranslator.Services.Repository.PokeAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PokemonTranslator.Services.IntegrationTests.Repository.PokeAPI
{
	public class PokeAPIRepositoryServiceTests
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public void GetPokemon_InvalidName_ShouldThrownArgumentExcpetionAsync(string name)
		{
			var service = new PokeAPIRepositoryService();

			Func<Task<PokemonData>> func = async () => await service.GetPokemon(name);

			func.Should().Throw<ArgumentException>();
		}

		[Theory]
		[InlineData("not-existing")]
		public async void GetPokemon_NotExistingPokemon_ShouldReturnNull(string name)
		{
			var service = new PokeAPIRepositoryService();

			var pokemon = await service.GetPokemon(name);

			pokemon.Should().BeNull();
		}

		[Theory]
		[MemberData(nameof(GetValidPokemonTestData))]
		public async void GetPokemon_ExistingPokemon_ShouldReturnExpectedData(string name, string expectedDescription)
		{
			var service = new PokeAPIRepositoryService();

			var pokemon = await service.GetPokemon(name);

			pokemon.Should().NotBeNull();
			pokemon.Name.Should().Be(name);
			pokemon.Description.Should().Be(expectedDescription);
		}

		public static IEnumerable<object[]> GetValidPokemonTestData()
		{
			yield return new[]
			{
				"charizard",
				"Spits fire that is hot enough to melt boulders. Known to cause forest fires unintentionally."
			};

			yield return new[]
			{
				"ditto",
				"It can freely recombine its own cellular structure to transform into other life-forms."
			};
		}
	}
}
