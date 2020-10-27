using PokeApiNet;
using PokemonTranslator.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PokemonTranslator.Services.Repository.Fake
{
	public class FakeRepositoryService : IPokemonRepositoryService
	{
		public FakeRepositoryService()
		{
		}

		public async Task<PokemonData> GetPokemon(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Invalid name", name);

			var pokemonData = new PokemonData()
			{
				Name = name,
				Description = string.Format("This is the description of {0}", name)
			};

			return await Task.FromResult(pokemonData);
		}
	}
}
