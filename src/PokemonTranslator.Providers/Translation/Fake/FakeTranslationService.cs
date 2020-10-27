using PokemonTranslator.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTranslator.Services.Translation.Fake
{
	/// <summary>
	/// Fake tramslation service.
	/// Convert every text to upper case
	/// </summary>
	public class FakeTranslationService : ITranslationService
	{
		public Task<string> Translate(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Invalid text", text);

			return Task.FromResult(text.ToUpper());
		}
	}
}
