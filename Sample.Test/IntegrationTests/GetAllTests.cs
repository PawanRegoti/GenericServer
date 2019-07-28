using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Sample.App;
using Sample.App.CollectionEnvelope;
using Sample.Test.IntegrationTests.Fixtures;
using Sample.Tests.Helpers;
using Xunit;

namespace Sample.Test.IntegrationTests
{
	[Collection("Database collection")]
	public class GetAllTests : IClassFixture<CustomWebApplicationFactory<Startup>>
	{
		private readonly CustomWebApplicationFactory<Startup> _factory;

		public GetAllTests(CustomWebApplicationFactory<Startup> factory, DatabaseFixture databaseFixture)
		{
			_factory = factory;
		}

		[Theory]
		[InlineData("/api/Sample")]
		public async Task GetAll_Test(string url)
		{
			// Arrange
			var client = _factory.CreateClientWithAuthenticationToken(
				new WebApplicationFactoryClientOptions
				{
					AllowAutoRedirect = false
				});

			// Act
			var response = await client.GetAsync(url);
			var responseBody = ContentHelper.GetContentAsJson<CollectionEnvelope<SampleModel>>(response.Content);

			// Assert
			response.IsSuccessStatusCode.Should().BeTrue();
			responseBody.Should().NotBeNull();
			responseBody.Collection.Should().HaveCount(4);
			responseBody.Self.LocalPath.Should().Be(url);
			responseBody.Pagination.Should().NotBeNull();
			responseBody.Pagination.Results.Should().Be(4);
			responseBody.Pagination.PreviousPage.Should().BeNull();
			responseBody.Pagination.PageSize.Should().Be(1000);
			responseBody.Pagination.SkipPages.Should().Be(0);
		}
	}
}
