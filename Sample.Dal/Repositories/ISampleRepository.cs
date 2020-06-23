using System.Collections.Generic;
using Sample.Shared.Exceptions;

namespace Sample.Dal.Repositories
{
  public interface ISampleRepository
  {
    (SampleModel, SampleException) Create(SampleModel data);
    (bool, SampleException) Delete(int userId, int documentNr);
    (SampleModel, SampleException) Get(int userId, int documentNr);
    (IEnumerable<SampleModel>, int, SampleException) GetAll(int userId, int pageSize, int skipPages);
    (SampleModel, bool?, SampleException) Update(SampleModel data);
    (int, SampleException) GetNextDocumentNr(int userId);
  }
}