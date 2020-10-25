using PokemonTranslator.API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonTranslator.API.Services
{
	public interface IPokemonService
	{
		Task<Pokemon> GetPokemon(string name);
	}
}
