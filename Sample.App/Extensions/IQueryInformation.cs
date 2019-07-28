using System.Linq;

namespace Sample.App.Extensions
{
  public interface IQueryInformation
  {
    string Filter { get; set; }
    int PageSize { get; set; }
    int SkipPages { get; set; }
    string Sort { get; set; }

    IQueryable<T> Apply<T>(IQueryable<T> query);

    IQueryable<T> ApplyFilter<T>(IQueryable<T> query);

    IQueryable<T> ApplyPaging<T>(IQueryable<T> query);

    IQueryable<T> ApplySorting<T>(IQueryable<T> query);
  }
}