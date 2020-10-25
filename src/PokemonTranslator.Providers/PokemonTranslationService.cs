using PokemonTranslator.Abstractions;
using PokemonTranslator.DTO;
using System;
using System.Threading.Tasks;

namespace PokemonTranslator.Providers
{
	public class PokemonTranslationService : IPokemonTranslationService
	{
		private IPokemonRepositoryService _repositoryService;
		private ITranslationService _translationService;

		public PokemonTranslationService(IPokemonRepositoryService repositoryService, ITranslationService translationService)
		{
			_repositoryService = repositoryService;
			_translationService = translationService;
		}

		public async Task<Pokemon> GetPokemon(string name)
		{
			var pokemonRepoEntry = await _repositoryService.GetPokemon(name);

			// Return null if pokemon not founf in the repo
			if (pokemonRepoEntry == null)
				return null;

			var translatedDescription = await _translationService.Translate(pokemonRepoEntry.Description);

			var pokemon = new Pokemon
			{
				Name = pokemonRepoEntry.Name,
				Description = translatedDescription
			};

			return pokemon;
		}
	}
}
