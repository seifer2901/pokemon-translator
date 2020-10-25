using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTranslator.Abstractions
{
	public class PokemonRepositoryEntry
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}

	public interface IPokemonRepositoryService
	{
		Task<PokemonRepositoryEntry> GetPokemon(string name);
	}
}
