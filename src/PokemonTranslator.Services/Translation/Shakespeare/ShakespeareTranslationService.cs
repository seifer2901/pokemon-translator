using PokemonTranslator.Abstractions.Services;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTranslator.Services.Translation.Shakespeare
{
	/// <summary>
	/// Shakespeare translation service
	/// Wrapper to https://lingojam.com/EnglishtoShakespearean
	/// The integration is made emulating browser actions using an headless browser https://github.com/hardkoded/puppeteer-sharp
	/// </summary>
	public class ShakespeareTranslationService : ITranslationService
	{
		Task downloadBrowserTask;

		public ShakespeareTranslationService()
		{
			downloadBrowserTask = new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
		}

		public async Task<string> Translate(string text)
		{
			var translatedText = string.Empty;

			if (string.IsNullOrWhiteSpace(text))
				throw new ArgumentException("Invalid text", text);

			await downloadBrowserTask;

			var launchOptions = new LaunchOptions
			{
				Headless = true,
				Args = new string[] { "--no-sandbox" }
			};
			using (var _browser = await Puppeteer.LaunchAsync(launchOptions))
			{
				using (var page = await _browser.NewPageAsync())
				{
					await page.GoToAsync("https://lingojam.com/EnglishtoShakespearean");

					// Clear text
					await page.EvaluateExpressionAsync("$('#english-text').val('')");
					await page.EvaluateExpressionAsync("$('#ghetto-text').val('')");

					// Set text
					//await page.EvaluateExpressionAsync($"$('#english-text').val('{text}')");
					await page.FocusAsync("#english-text");
					await page.Keyboard.TypeAsync(text, new TypeOptions() { Delay = 10 }); ;

					// Read translated text until we have same read twice
					// This is used with the purpose of waiting the correct text to be displayed
					// Could be achievied checking for the gif load animation status but we go with this solution
					// for semplicity
					string[] readAttempt = { string.Empty, string.Empty };
					int index = 0;
					var translatedTextSelector = @"document.querySelector('#ghetto-text').value;";
					while (true)
					{
						// Wait 1 sec
						await Task.Delay(1000);

						readAttempt[index % 2] = await page.EvaluateExpressionAsync<string>(translatedTextSelector);
						index++;

						if (index < 2)
							continue;

						if (string.Compare(readAttempt[0], readAttempt[1]) == 0)
						{
							translatedText = readAttempt[0];
							break;
						}
					}
				}
			}

			return translatedText;
		}
	}
}
