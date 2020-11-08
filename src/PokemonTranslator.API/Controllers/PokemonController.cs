using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonTranslator.DTO;
using PokemonTranslator.Abstractions.Services;
using PokemonTranslator.API.Exstensions;

namespace PokemonTranslator.API.Controllers
{
	/// <summary>
	/// Pokemon controller
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class PokemonController : ControllerBase
	{
		private readonly ILogger<PokemonController> _logger;
		private readonly IPokemonTranslationService _pokemonService;

		public PokemonController(ILogger<PokemonController> logger, IPokemonTranslationService pokemonService)
		{
			_logger = logger;
			_pokemonService = pokemonService;
		}

		/// <summary>
		/// Returns a pokemon translated description given its name.
		/// </summary>
		/// <param name="pokemonName">The pokemon name</param>
		/// <returns><see cref="PokemonDTO"/>.</returns>
		[HttpGet]
		[Route("{pokemonName}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<PokemonDTO>> Get([FromRoute] string pokemonName)
		{
			if (string.IsNullOrWhiteSpace(pokemonName))
			{
				return BadRequest("pokemanName cannot be null or whitespace");
			}

			var pokemon = await _pokemonService.GetPokemon(pokemonName);

			if (pokemon is null)
				return NotFound($"Pokemon {pokemonName} not found");
			else
				return Ok(pokemon.ToDTO());
		}
	}
}
