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

namespace PokemonTranslator.Services.Repository.PokeAPI
{
	/// <summary>
	/// PokeAPIRepositoryService
	/// Wrapper for getting pokemon data from https://pokeapi.co/
	/// Used PokeAPI clint helper nuget package - https://github.com/mtrdp642/PokeApiNet
	/// </summary>
	public class PokeAPIRepositoryService : IPokemonRepositoryService
	{
		private readonly PokeApiClient _pokeApiClient = new PokeApiClient();

		public PokeAPIRepositoryService()
		{
		}

		public async Task<PokemonData> GetPokemon(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Invalid name", name);

			PokemonSpecies species = null;
			try
			{
				species = await _pokeApiClient.GetResourceAsync<PokemonSpecies>(name);
			}
			catch (HttpRequestException)
			{
			}

			if (species == null)
				return null;

			var description = species.FlavorTextEntries
				.Where(entry => entry.Language.Name == "en")
				.Select(s => Regex.Replace(s.FlavorText, @"\t|\n|\r|\f", " "))
				.FirstOrDefault();

			var pokemonData = new PokemonData()
			{
				Name = species.Name,
				Description = description
			};

			return pokemonData;
		}
	}
}
