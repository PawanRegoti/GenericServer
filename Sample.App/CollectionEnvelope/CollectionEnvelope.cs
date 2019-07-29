using System;
using System.Collections.Generic;
using System.Web;

namespace Sample.App.CollectionEnvelope
{
  public class CollectionEnvelope<T>
  {
    public CollectionEnvelope()
    { }

    public CollectionEnvelope(IEnumerable<T> collection, CollectionPagination collectionPagination, Uri self)
    {
      Collection = collection ?? new List<T>();
      Pagination = collectionPagination;
      Self = self;
    }

    public IEnumerable<T> Collection { get; set; }

    public CollectionPagination Pagination { get; set; }

    public Uri Self { get; set; }
  }

  public class CollectionPagination
  {
    public const int DEFAULT_PAGE_SIZE = 20;
    private readonly Uri _link;

    public CollectionPagination()
    { }

    public CollectionPagination(
      Uri link,
      int skipPages,
      int pageSize,
      int results)
    {
      _link = link;
      SkipPages = skipPages;
      PageSize = pageSize;

      Results = results;

      SetPaginationLinks();
    }

    public int PageSize { get; set; }
    public int SkipPages { get; set; }
    public int Results { get; set; }

    public Uri FirstPage { get; set; }
    public Uri NextPage { get; set; }
    public Uri PreviousPage { get; set; }

    private void SetPaginationLinks()
    {
      var uriBuilder = new UriBuilder(_link.ToString());
      var queryString = _link.Query;

      uriBuilder.Query = GetPagedQuery(0);
      FirstPage = uriBuilder.Uri;

      if (Results - (PageSize * SkipPages) > 0)
      {
        uriBuilder.Query = GetPagedQuery(SkipPages + 1);
        NextPage = uriBuilder.Uri;
      }

      if (SkipPages > 0)
      {
        uriBuilder.Query = GetPagedQuery(SkipPages - 1);
        PreviousPage = uriBuilder.Uri;
      }
    }

    private string GetPagedQuery(int skipPages)
    {
      var qs = HttpUtility.ParseQueryString(_link.Query);
      qs.Set("skippages", skipPages.ToString());
      qs.Set("pagesize", PageSize.ToString());
      return qs.ToString();
    }
  }
}