using PokemonTranslator.Abstractions.Models;
using PokemonTranslator.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonTranslator.API.Exstensions
{
	public static class ModelsExstensions
	{
		public static PokemonDTO ToDTO(this Pokemon pokemon)
		{
			if (pokemon == null)
				return null;

			return new PokemonDTO()
			{
				Name = pokemon.Name,
				Description = pokemon.TranslatedDescription
			};
		}
	}
}
