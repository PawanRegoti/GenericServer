using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Sample.App;
using Sample.Test.IntegrationTests.Fixtures;

namespace Sample.Test.IntegrationTests
{
	public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
	{
		public const string RevisoSecret = "RevisoSecret";

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			Environment.SetEnvironmentVariable("EmbeddedAppAuthentication:TokenFactorySecrets:0", RevisoSecret);
			Environment.SetEnvironmentVariable("App:ServiceNamespace", "DeliveryNoteAppAutomatedTests");
			Environment.SetEnvironmentVariable("ConnectionStrings:MongoDb", MongoDbEntities.ConnectionString);
			Environment.SetEnvironmentVariable("MongoDb:DatabaseName", MongoDbEntities.DatabaseName);
			Environment.SetEnvironmentVariable("MongoDb:DeliveryNoteCollectionName", MongoDbEntities.CollectionName);
		}

		public HttpClient CreateClientWithAuthenticationToken(WebApplicationFactoryClientOptions options)
		{
			var httpClient = CreateClient(options);
			AddAuthenticationToken(httpClient);
			return httpClient;
		}

		private void AddAuthenticationToken(HttpClient httpClient)
		{
			httpClient.DefaultRequestHeaders.Add("UserId", "12345");
		}
	}
}
