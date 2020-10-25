using PokemonTranslator.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonTranslator.Abstractions
{
	public interface IPokemonTranslationService
	{
		Task<Pokemon> GetPokemon(string name);
	}
}
