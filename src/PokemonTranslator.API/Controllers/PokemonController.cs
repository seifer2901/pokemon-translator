using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonTranslator.DTO;
using PokemonTranslator.API.Services;

namespace PokemonTranslator.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PokemonController : ControllerBase
	{
		private readonly ILogger<PokemonController> _logger;
		private readonly IPokemonService _pokemonService;

		public PokemonController(ILogger<PokemonController> logger, IPokemonService pokemonService)
		{
			_logger = logger;
			_pokemonService = pokemonService;
		}

		[HttpGet]
		[Route("{pokemonName}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Pokemon>> Get([FromRoute] string pokemonName)
		{
			if (string.IsNullOrWhiteSpace(pokemonName))
			{
				return BadRequest("pokemanName cannot be null or whitespace");
			}

			var pokemon = await _pokemonService.GetPokemon(pokemonName);

			if (pokemon is null)
				return NotFound(pokemonName);
			else
				return Ok(pokemon);
		}
	}
}
