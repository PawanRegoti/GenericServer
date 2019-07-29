using System.Collections.Generic;
using Sample.Shared.Exceptions;

namespace Sample.Dal.Repositories
{
  public interface ISampleRepository
  {
    (SampleDto, SampleException) Create(SampleDto data);
    (bool, SampleException) Delete(int userId, int documentNr);
    (SampleDto, SampleException) Get(int userId, int documentNr);
    (IEnumerable<SampleDto>, int, SampleException) GetAll(int userId, int pageSize, int skipPages);
    (SampleDto, bool?, SampleException) Update(SampleDto data);
    (int, SampleException) GetNextDocumentNr(int userId);
  }
}