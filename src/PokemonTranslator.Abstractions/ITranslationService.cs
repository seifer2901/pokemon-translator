using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTranslator.Abstractions
{
	public interface ITranslationService
	{
		public Task<string> Translate(string text);
	}
}
