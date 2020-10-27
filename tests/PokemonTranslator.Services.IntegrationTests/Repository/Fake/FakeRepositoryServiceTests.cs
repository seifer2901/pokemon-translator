using FluentAssertions;
using PokemonTranslator.Abstractions.Services;
using PokemonTranslator.Services.Repository.Fake;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PokemonTranslator.Services.IntegrationTests.Repository.Fake
{
	public class FakeRepositoryServiceTests
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public void GetPokemon_InvalidName_ShouldThrownArgumentExcpetionAsync(string name)
		{
			var service = new FakeRepositoryService();

			Func<Task<PokemonData>> func = async () => await service.GetPokemon(name);

			func.Should().Throw<ArgumentException>();
		}

		[Fact]
		public async void GetPokemon_ExistingPokemon_ShouldReturnExpectedData()
		{
			var name = "FakePokemon";
			var expectedDescription = string.Format("This is the description of {0}", name);
			var service = new FakeRepositoryService();

			var pokemon = await service.GetPokemon(name);

			pokemon.Should().NotBeNull();
			pokemon.Name.Should().Be(name);
			pokemon.Description.Should().Be(expectedDescription);
		}
	}
}
