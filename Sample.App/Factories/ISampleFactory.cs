using System.Collections.Generic;
using Sample.Shared.Exceptions;

namespace Sample.App.Factories
{
  public interface ISampleFactory
  {
    (SampleModel, SampleException) Create(int userId, SampleModel data);
    (bool, SampleException) Delete(int userId, int documentNr);
    (SampleModel, SampleException) Get(int userId, int documentNr);
    (IEnumerable<SampleModel>, int, SampleException) GetAll(int userId, int pageSize, int skipPages);
    (SampleModel, bool?, SampleException) Update(int userId, SampleModel data);
  }
}