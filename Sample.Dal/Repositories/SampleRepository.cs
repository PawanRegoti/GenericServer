using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Polly;
using Sample.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Dal.Repositories
{
  public class SampleRepository : ISampleRepository
  {
    private readonly int _maxRetries;
    private readonly Policy _policy;
    private readonly IMongoCollection<SampleDto> _mongoCollection;
    private readonly ILogger<SampleRepository> _logger;

    public SampleRepository(IMongoDatabase mongoDatabase,
       string deliveryNoteCollectionName,
       int maxRetries,
       ILogger<SampleRepository> logger)
    {
      _mongoCollection = mongoDatabase.GetCollection<SampleDto>(deliveryNoteCollectionName);
      _logger = logger;

      _maxRetries = maxRetries;
      _policy = Policy
        .Handle<Exception>()
        .WaitAndRetry(_maxRetries,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            (exception, timeSpan, retryCount, context) =>
            {
              _logger.LogInformation("Retrying DB connection: ", new Dictionary<string, object> {
                { "retryCount", retryCount },
                { "timeSpan", timeSpan },
                { "exception", exception },
                { "context", context }
              });
            });
    }

    public (IEnumerable<SampleDto>, int, SampleException) GetAll(int userId, int pageSize, int skipPages)
    {
      try
      {
        return _policy.Execute(() =>
        {
          var entries = _mongoCollection.Find(x => x.UserId == userId);
          var totalEntries = (int)entries.CountDocuments();
          var resultEntries = entries.Limit(pageSize).Skip(skipPages).ToList();

          return (resultEntries, totalEntries, (SampleException)null);
        });
      }
      catch (MongoException e)
      {
        _logger.LogError(e, "Error occurred while attempting to get all relevant documents");
        return (new List<SampleDto>(), 0, new SampleException(e.Message));
      }
    }

    public (SampleDto, SampleException) Get(int userId, int documentNr)
    {
      try
      {
        return _policy.Execute(() =>
        {
          return (_mongoCollection
          .Find(x => x.UserId == userId && x.DocumentNr == documentNr)
          .SingleOrDefault()
          , (SampleException)null);
        });
      }
      catch (MongoException e)
      {
        _logger.LogError(e, $"Error occurred while getting document with Id {userId}");
        return (null, new SampleException(e.Message));
      }
    }

    public (SampleDto, SampleException) Create(SampleDto data)
    {
      try
      {
        return _policy.Execute(() =>
        {
          _mongoCollection.InsertOne(data);
          return (data, (SampleException)null);
        });
      }
      catch (MongoException e)
      {
        _logger.LogError(e, "Error occurred while creating document");
        return (null, new SampleException(e.Message));
      }
    }

    public (SampleDto, bool?, SampleException) Update(SampleDto data)
    {
      try
      {
        return _policy.Execute(() =>
        {
          var result = _mongoCollection.ReplaceOne(
            x => x.UserId == data.UserId && x.DocumentNr == data.DocumentNr,
            data,
            new UpdateOptions { IsUpsert = true }
          );

          if (!result.IsAcknowledged)
          {
            return (data, (bool?)null, (SampleException)null);
          }

          return (data, result.ModifiedCount == 0 ? false : true, (SampleException)null);
        });

      }
      catch (MongoException e)
      {
        _logger.LogError(e, $"Error occurred while updating document with Id {data.UserId}");
        return (data, null, new SampleException(e.Message));
      }
    }

    public (bool, SampleException) Delete(int userId, int documentNr)
    {
      try
      {
        var result = _policy.Execute(() =>
        {
          return _mongoCollection.DeleteOne(
            x => x.UserId == userId && x.DocumentNr == documentNr);
        });

        return (result.IsAcknowledged, null);
      }
      catch (MongoException e)
      {
        _logger.LogError(e, $"Error occurred while deleting document with Id {userId}");
        return (false, new SampleException(e.Message));
      }
    }

    public (int, SampleException) GetNextDocumentNr(int userId)
    {
      try
      {
        var lastDocument = _policy.Execute(() =>
        {
          return _mongoCollection.Aggregate()
          .Match(x => x.UserId == userId)
          .Group(key => key.UserId, group =>
            new { DocumentNr = group.Max(x => x.DocumentNr) })
          .FirstOrDefault();
        });

        var nextDocumentId = (lastDocument?.DocumentNr ?? 0) + 1;
        return (nextDocumentId, (SampleException)null);
      }
      catch (MongoException e)
      {
        _logger.LogError(e, "Error occurred while fetching next document number");
        return (0, new SampleException(e.Message));
      }
    }
  }
}
