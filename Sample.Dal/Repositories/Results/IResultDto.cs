using Sample.Shared.Exceptions;

namespace Sample.Dal.Repositories.Results
{
	public interface IResultDto<TData>
	{
		(TData, SampleException) Execute();
	}
}
