using System.Collections.Generic;
using Sample.Shared.Exceptions;

namespace Sample.App.Factories
{
  public interface ISampleFactory
  {
    (SampleDto, SampleException) Create(int userId, SampleDto data);
    (bool, SampleException) Delete(int userId, int documentNr);
    (SampleDto, SampleException) Get(int userId, int documentNr);
    (IEnumerable<SampleDto>, int, SampleException) GetAll(int userId, int pageSize, int skipPages);
    (SampleDto, bool?, SampleException) Update(int userId, SampleDto data);
  }
}