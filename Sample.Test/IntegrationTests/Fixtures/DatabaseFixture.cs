using Mongo2Go;
using MongoDB.Driver;
using Sample.Dal;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Sample.Test.IntegrationTests.Fixtures
{
  public static class MongoDbEntities
  {
    public static string DatabaseName = "SampleIntegrationTest";
    public static string CollectionName = "sample-test-collection";
    public static MongoDbRunner MongoDbRunner { get; set; }
    public static string ConnectionString { get; set; }
    public static IMongoCollection<SampleModel> Collection { get; set; }
  }

  public class DatabaseFixture : IDisposable
  {
    public DatabaseFixture()
    {
      (MongoDbEntities.MongoDbRunner, MongoDbEntities.ConnectionString, MongoDbEntities.Collection) =
        CreateDb(MongoDbEntities.DatabaseName, MongoDbEntities.CollectionName);

      SeedDatabase(MongoDbEntities.MongoDbRunner);
    }

    private (MongoDbRunner, string, IMongoCollection<SampleModel>) CreateDb(string databaseName, string collectionName)
    {
      //Use 'MongoDbRunner.StartForDebugging' instead of 'MongoDbRunner.Start', if you want to see the data in MongoDb.
      //'MongoDbRunner.Start' will delete the db after use whereas 'MongoDbRunner.StartForDebugging' doesn't
      var runner = MongoDbRunner.Start(singleNodeReplSet: false);
      var connectionString = runner.ConnectionString;

      var client = new MongoClient(connectionString);
      var database = client.GetDatabase(MongoDbEntities.DatabaseName);

      var collection = database.GetCollection<SampleModel>(MongoDbEntities.CollectionName);

      collection.InsertOne(new SampleModel(12345, 1)
      {
        Name = "Goku",
        Address = "Kame House",
        City = "Some Island",
        Country = "Japan",
        PhoneNumbers = new List<int> { 1234567890, 2143658709 }
      });

      return (runner, connectionString, collection);
    }

    private void SeedDatabase(MongoDbRunner runner)
    {
      //Seeding
      var testSeedDataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"IntegrationTests\Fixtures\TestData\sample-test-data.json");
      if (File.Exists(testSeedDataFilePath))
      {
        runner.Import(MongoDbEntities.DatabaseName, MongoDbEntities.CollectionName, testSeedDataFilePath, true);
      }
    }

    public void Dispose()
    {
      // Place tear down code here
      MongoDbEntities.MongoDbRunner.Dispose();
    }
  }

  [CollectionDefinition("Database collection")]
  public class DatabaseCollection : ICollectionFixture<DatabaseFixture> { }
}
