using System;
using System.IO;
using Mongo2Go;
using Xunit;
using Xunit.Abstractions;

namespace Sample.Test.DataSeeder
{
	public class MongoDbSeeder : IDisposable
	{
		private MongoDbRunner _runner;
		private string _databaseName = "SampleAppLocalDB";
		private string _collectionName = "local-sample-app";

		private readonly ITestOutputHelper _output;

		public MongoDbSeeder(ITestOutputHelper output)
		{
			_output = output;
		}

		//[Fact]
		public void LocalDataSeeder()
		{
			_runner = MongoDbRunner.StartForDebugging(singleNodeReplSet: false, port: 27017);
			SeedDatabase(_runner);
			_output.WriteLine($"MongoDB Connection String : {_runner.ConnectionString}");
		}

		private void SeedDatabase(MongoDbRunner runner)
		{
			//Seeding
			var testSeedDataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"DataSeeder\LocalDbData\sample-local-data.json");
			if (File.Exists(testSeedDataFilePath))
			{
				runner.Import(_databaseName, _collectionName, testSeedDataFilePath, true);
				_output.WriteLine("Import Completed");
			}
		}

		public void Dispose()
		{
			_runner.Dispose();
		}
	}
}
