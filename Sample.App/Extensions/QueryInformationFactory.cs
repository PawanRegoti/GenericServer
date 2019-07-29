using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sample.App.Extensions
{
  public static class QueryInformationFactory
  {
    public static IQueryInformation GetQueryInformation(this IEnumerable<KeyValuePair<string, StringValues>> query)
    {
      var info = new QueryInformation();
      foreach (var pair in query)
      {
        var key = pair.Key.ToLower();
        switch (key)
        {
          case "filter":
            info.Filter = pair.Value;
            break;

          case "sort":
            info.Sort = pair.Value;
            break;

          case "pagesize":
            if (int.TryParse(pair.Value, out var pageSize))
            {
              info.PageSize = pageSize;
            }
            break;

          case "skippages":
            if (int.TryParse(pair.Value, out var skipPages))
            {
              info.SkipPages = skipPages;
            }
            break;
        }
      }

      return info;
    }

    public class QueryInformation : IQueryInformation
    {
      public QueryInformation()
      {
        PageSize = 1000;
        SkipPages = 0;
      }

      public string Filter { get; set; }
      public string Sort { get; set; }
      public int PageSize { get; set; }
      public int SkipPages { get; set; }

      public IQueryable<T> ApplyFilter<T>(IQueryable<T> query)
      {
        var filter = Filter;
        if (filter == null)
        {
          return query;
        }

        //Do filter processing

        return null;
      }

      public IQueryable<T> ApplySorting<T>(IQueryable<T> query)
      {
        var sortLambdas = Sort;
        if (sortLambdas?.Any() != true)
        {
          return query;
        }

        //Do sort processing

        return null;
      }

      public IQueryable<T> ApplyPaging<T>(IQueryable<T> query)
      {
        if (SkipPages != 0)
        {
          query = query.Skip(SkipPages * PageSize);
        }

        return query.Take(PageSize);
      }

      public IQueryable<T> Apply<T>(IQueryable<T> query)
      {
        var filtered = ApplyFilter(query);
        var sorted = ApplySorting(filtered);
        return ApplyPaging(sorted);
      }
    }
  }
}