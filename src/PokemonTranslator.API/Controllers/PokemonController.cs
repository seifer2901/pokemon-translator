using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PokemonTranslator.API.DTOs;

namespace PokemonTranslator.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PokemonController : ControllerBase
	{
		private readonly ILogger<PokemonController> _logger;

		public PokemonController(ILogger<PokemonController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[Route("{pokemonName}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Pokemon>> Get([FromRoute] string pokemonName)
		{
			return await Task.FromResult((Pokemon)null);
		}
	}
}
