using Sample.Shared.Exceptions;

namespace Sample.Dal.Repositories.Results
{
	public interface IResultDtoWithStatus<TData, TStatus>
	{
		(TData, TStatus, SampleException) Execute();
	}
}
