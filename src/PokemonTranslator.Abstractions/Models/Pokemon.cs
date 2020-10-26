using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonTranslator.Abstractions.Models
{
	public class Pokemon
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string TranslatedDescription { get; set; }
	}
}
