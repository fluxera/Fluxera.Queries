namespace Fluxera.Queries.AspNetCore.UnitTests
{
	using System;
	using System.Net;
	using System.Net.Http;
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Queries.AspNetCore.UnitTests.Model;
	using MadEyeMatt.AspNetCore.Endpoints;
	using Microsoft.AspNetCore.Builder;
	using NUnit.Framework;

	[TestFixture]
	public class FilterQueryOptionTests : TestServerFixtureBase
	{
		/// <inheritdoc />
		protected override Task ConfigureServices(WebApplicationBuilder builder)
		{
			builder.Services.AddDataQueries(options =>
			{
				options.EntitySet<Customer>("Customers");
			});

			return Task.CompletedTask;
		}

		/// <inheritdoc />
		protected override Task Configure(WebApplication app)
		{
			app.MapEndpoints();

			return Task.CompletedTask;
		}

		[Test]
		public async Task Should()
		{
			HttpClient client = this.CreateClient();

			const string path = "api/customers?$filter=FirstName eq 'James'";

			HttpResponseMessage response = await client.GetAsync(path);
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			string json = await response.Content.ReadAsStringAsync();
			Console.WriteLine(json);
		}
	}
}
