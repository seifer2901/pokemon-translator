using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PokemonTranslator.API.Controllers;
using PokemonTranslator.API.DTOs;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PokemonTranslator.API.Tests
{
	public class PokemonControllerTests
	{
		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData(null)]
		public async Task Get_InvalidPokemonName_ShouldReturnBadRequest(string pokemonName)
		{
			var controller = new PokemonController(NullLogger<PokemonController>.Instance);

			var actionResult = await controller.Get(pokemonName);
			
			actionResult.Result.Should().BeOfType<BadRequestResult>();
		}

		[Theory]
		[InlineData("charizard")]
		[InlineData("pikachu")]
		public async Task Get_ValidExistingPokemonName_ShouldReturnPokemon(string pokemonName)
		{
			var controller = new PokemonController(NullLogger<PokemonController>.Instance);

			var actionResult = await controller.Get(pokemonName);

			actionResult.Result.Should().BeOfType<OkResult>();
			actionResult.Value.Should().BeOfType<Pokemon>();

			var pokemon = actionResult.Value as Pokemon;
			pokemon.Name.Should().Equals(pokemonName);
		}

		[Theory]
		[InlineData("not-exist")]
		public async Task Get_ValidNotExistingPokemonName_ShouldReturnNotFound(string pokemonName)
		{
			var controller = new PokemonController(NullLogger<PokemonController>.Instance);

			var actionResult = await controller.Get(pokemonName);

			actionResult.Result.Should().BeOfType<NotFoundResult>();
		}
	}
}
