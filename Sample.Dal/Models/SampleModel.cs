using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Dal
{
  public class SampleModel
  {
    [BsonId]
    public string Id => SampleKey.GetKey(UserId, DocumentNr);

    public int UserId { get; private set; }

    public int DocumentNr { get; private set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public IEnumerable<int> PhoneNumbers { get; set; }

    public SampleModel(int userId, int documentNr)
    {
      UserId = userId;
      DocumentNr = documentNr;
    }
  }

  public static class SampleKey
  {
    public static string GetKey(int userId, int documentNr) => $"{userId}_{documentNr}";

    public static int GetUserId(string key) => Int32.Parse(key.Split('_').First());

    public static int GetDocumentNr(string key) => Int32.Parse(key.Split('_').Last());
  }
}
