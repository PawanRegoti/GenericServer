using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.App.CollectionEnvelope;
using Sample.App.Extensions;
using System;
using System.Collections.Generic;

namespace Sample.App.Controllers
{
  /// <summary>
  /// TDataModel: data model type
  /// TKeyType: key type
  /// </summary>
  /// <typeparam name="TDataModel"></typeparam>
  /// <typeparam name="TKeyType"></typeparam>
  public abstract class BaseController<TDataModel, TKeyType> : ControllerBase
  {
    public Uri CurrentUrl => new Uri(Request.GetDisplayUrl());

    public IQueryInformation Info => Request.Query.GetQueryInformation();


    [NonAction]
    public CollectionPagination GetPagination(int numberOfEntries) => new CollectionPagination(CurrentUrl, Info.SkipPages, Info.PageSize, numberOfEntries);

    [NonAction]
    public CollectionEnvelope<TDataModel> GetCollectionEnvelope(IEnumerable<TDataModel> entries, int totalEntries) => new CollectionEnvelope<TDataModel>(entries, GetPagination(totalEntries), CurrentUrl);

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public abstract ActionResult<CollectionEnvelope<TDataModel>> GetAll();

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public abstract ActionResult<TDataModel> Get(TKeyType id);

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public abstract ActionResult<TDataModel> Create(TDataModel data);

    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(200)]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public abstract ActionResult<TDataModel> Update(TKeyType id, TDataModel data);

    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public abstract ActionResult<TDataModel> Delete(TKeyType id);
  }
}