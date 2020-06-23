using Sample.Dal;
using Sample.Dal.Repositories;
using Sample.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample.Infrastructure.SampleRepository
{
  public class SampleNhibernateRepository : ISampleRepository
  {
    public (SampleModel, SampleException) Create(SampleModel data)
    {
      throw new NotImplementedException();
    }

    public (bool, SampleException) Delete(int userId, int documentNr)
    {
      throw new NotImplementedException();
    }

    public (SampleModel, SampleException) Get(int userId, int documentNr)
    {
      throw new NotImplementedException();
    }

    public (IEnumerable<SampleModel>, int, SampleException) GetAll(int userId, int pageSize, int skipPages)
    {
      throw new NotImplementedException();
    }

    public (int, SampleException) GetNextDocumentNr(int userId)
    {
      throw new NotImplementedException();
    }

    public (SampleModel, bool?, SampleException) Update(SampleModel data)
    {
      throw new NotImplementedException();
    }
  }
}
