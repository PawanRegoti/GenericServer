using System.Collections.Generic;
using Sample.Shared.Exceptions;

namespace Sample.Dal.Repositories.Results
{
	public interface IResultDtosWithPagination<TData>
	{
		(IEnumerable<TData>, int, SampleException) Execute();
	}
}
