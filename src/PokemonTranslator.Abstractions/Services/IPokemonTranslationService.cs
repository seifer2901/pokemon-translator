using PokemonTranslator.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonTranslator.Abstractions.Services
{
	public interface IPokemonTranslationService
	{
		Task<Pokemon> GetPokemon(string name);
	}
}
