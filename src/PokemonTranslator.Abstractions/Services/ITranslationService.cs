using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTranslator.Abstractions.Services
{
	public interface ITranslationService
	{
		public Task<string> Translate(string text);
	}
}
