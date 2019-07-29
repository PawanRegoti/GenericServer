using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sample.App.Mappers;
using Sample.Dal.Repositories;
using Sample.Shared.Exceptions;

namespace Sample.App.Factories
{
	public class SampleFactory : ISampleFactory
  {
		private ISampleRepository _sampleRepository;

		public SampleFactory(
			IMongoDatabase mongoDatabase,
			IConfiguration configuration,
			ILogger<SampleRepository> logger)
		{
			var deliveryNoteCollectionName = configuration.GetValue<string>(StartupConfigurationKey.DeliveryNoteCollectionName);
			var maxRetries = Convert.ToInt32(configuration.GetValue<string>(StartupConfigurationKey.MongoDbMaxRetries));

			_sampleRepository = new SampleRepository(
				mongoDatabase,
				deliveryNoteCollectionName,
				maxRetries,
				logger);
		}

    public (IEnumerable<SampleModel>, int, SampleException) GetAll(int userId, int pageSize, int skipPages)
    {
      (var results, var count, var error) = _sampleRepository.GetAll(userId, pageSize, skipPages);
      return (results.Select(result => SampleMapper.Map(result)), count, error);
    }
    public (SampleModel, SampleException) Get(int userId, int documentNr)
    {
      (var result, var error) = _sampleRepository.Get(userId, documentNr);
      return (SampleMapper.Map(result), error);
    }
    public (SampleModel, SampleException) Create(int userId, SampleModel data)
    {
      (var nextDocumentNr, var fetchError) = _sampleRepository.GetNextDocumentNr(userId);
      if(fetchError != null)
      {
        return (data, fetchError);
      }
      data.DocumentNr = nextDocumentNr;

      (var result, var error) = _sampleRepository.Create(SampleMapper.Map(userId, data));
      return (SampleMapper.Map(result), error);
    }
    public (SampleModel, bool?, SampleException) Update(int userId, SampleModel data)
    {
      (var result, var status, var error) = _sampleRepository.Update(SampleMapper.Map(userId, data));
      return (SampleMapper.Map(result), status, error);
    }
    public (bool, SampleException) Delete(int userId, int documentNr) => _sampleRepository.Delete(userId, documentNr);
  }

}
