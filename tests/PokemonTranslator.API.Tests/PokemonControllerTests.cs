using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PokemonTranslator.API.Controllers;
using PokemonTranslator.DTO;
using PokemonTranslator.Abstractions.Services;
using System;
using System.Threading.Tasks;
using Xunit;
using PokemonTranslator.Abstractions.Models;

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
			var pokemonService = new Mock<IPokemonTranslationService>();
			var controller = new PokemonController(NullLogger<PokemonController>.Instance, pokemonService.Object);

			var actionResult = await controller.Get(pokemonName);
			
			actionResult.Result.Should().BeOfType<BadRequestObjectResult>();
		}

		[Theory]
		[InlineData("charizard")]
		[InlineData("pikachu")]
		public async Task Get_ValidExistingPokemonName_ShouldReturnPokemon(string pokemonName)
		{
			var expectedDescription = pokemonName.ToUpper();
			var pokemonService = new Mock<IPokemonTranslationService>();
			pokemonService.Setup(service => service.GetPokemon(pokemonName))
				.ReturnsAsync(
					new Pokemon() 
					{ 
						Name = pokemonName, Description = expectedDescription
					});
			var controller = new PokemonController(NullLogger<PokemonController>.Instance, pokemonService.Object);

			var actionResult = await controller.Get(pokemonName);

			actionResult.Result.Should().BeOfType<OkObjectResult>();
			var okResult = actionResult.Result as OkObjectResult;
			okResult.Value.Should().BeOfType<PokemonDTO>();
			var pokemon = okResult.Value as PokemonDTO;
			pokemon.Name.Should().Equals(pokemonName);
			pokemon.Description.Should().Equals(expectedDescription);
		}

		[Theory]
		[InlineData("not-exist")]
		public async Task Get_ValidNotExistingPokemonName_ShouldReturnNotFound(string pokemonName)
		{
			var pokemonService = new Mock<IPokemonTranslationService>();
			pokemonService.Setup(service => service.GetPokemon(pokemonName)).ReturnsAsync((Pokemon)null);
			var controller = new PokemonController(NullLogger<PokemonController>.Instance, pokemonService.Object);

			var actionResult = await controller.Get(pokemonName);

			actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
		}
	}
}
