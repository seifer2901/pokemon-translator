using FluentAssertions;
using PokemonTranslator.Abstractions.Models;
using PokemonTranslator.API.Exstensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonTranslator.API.Tests
{
	public class ModelExstensionsTests
	{
		[Fact]
		public void ToDTO_NullPokemon_ShouldReturnNull()
		{
			Pokemon pokemon = null;

			var pokemonDTO = pokemon.ToDTO();

			pokemonDTO.Should().BeNull();
		}

		[Fact]
		public void ToDTO_ValidPokemon_ShouldReturnValidPokemonDTO()
		{
			var name = "PokemonName";
			var description = "PokemonDescription";
			var translatedDescription = "PokemonTranslatedDescription";
			var pokemon = new Pokemon() { Name = name, Description = description, TranslatedDescription = translatedDescription};

			var pokemonDTO = pokemon.ToDTO();

			pokemonDTO.Should().NotBeNull();
			pokemonDTO.Name.Should().Equals(name);
			pokemonDTO.Description.Should().Equals(translatedDescription);
		}
	}
}
