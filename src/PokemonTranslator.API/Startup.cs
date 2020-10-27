using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PokemonTranslator.Abstractions.Services;
using PokemonTranslator.API.Config;
using PokemonTranslator.Services;
using PokemonTranslator.Services.Repository.Fake;
using PokemonTranslator.Services.Repository.PokeAPI;
using PokemonTranslator.Services.Translation.Fake;
using PokemonTranslator.Services.Translation.Shakespeare;

namespace PokemonTranslator.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var repositoryService = GetRepositoryService();
			ITranslationService translationService = GetTranslationService();

			services.AddSingleton<IPokemonRepositoryService>(repositoryService);
			services.AddSingleton<ITranslationService>(translationService);
			services.AddSingleton<IPokemonTranslationService, PokemonTranslationService>();
			services.AddControllers();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pokemon Translator API", Version = "v1" });
				
				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
		}

		private ITranslationService GetTranslationService()
		{
			var translationServiceType = Configuration.GetValue<TranslationServiceType>("TranslationServiceType");
			ITranslationService translationService = null;
			switch (translationServiceType)
			{
				case TranslationServiceType.Fake:
					translationService = new FakeTranslationService();
					break;
				case TranslationServiceType.Shakespeare:
					translationService = new ShakespeareTranslationService();
					break;
				default:
					break;
			}

			return translationService;
		}

		private IPokemonRepositoryService GetRepositoryService()
		{
			var repoServiceType = Configuration.GetValue<RepositoryServiceType>("RepositoryServiceType");

			IPokemonRepositoryService repositoryService = null;
			switch (repoServiceType)
			{
				case RepositoryServiceType.Fake:
					repositoryService = new FakeRepositoryService();
					break;
				case RepositoryServiceType.PokeAPI:
					repositoryService = new PokeAPIRepositoryService();
					break;
				default:
					repositoryService = new FakeRepositoryService();
					break;
			}

			return repositoryService;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon Translator API v1");
				c.RoutePrefix = string.Empty;
			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
