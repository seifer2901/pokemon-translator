using PokemonTranslator.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTranslator.Services.Translation.Shakespeare
{
	/// <summary>
	/// Shakespeare translation service
	/// Wrapper to API https://funtranslations.com/shakespeare
	/// TO DO: not implemented
	/// </summary>
	public class ShakespeareTranslationService : ITranslationService
	{
		public Task<string> Translate(string text)
		{
			throw new NotImplementedException();
		}
	}
}
