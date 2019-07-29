using System.Linq;

namespace Sample.App.CollectionEnvelope
{
  public class SelfLinkFactory : ISelfLinkFactory
  {
    private readonly string _baseUrl;

    public SelfLinkFactory(string baseUrl)
    {
      _baseUrl = baseUrl;
    }

    public string Build(params object[] parts)
    {
      var concatedParts = "";
      if (parts?.Any() == true)
      {
        concatedParts = string.Join("/", parts);
      }

      var baseUrl = _baseUrl.Trim().TrimEnd('/');
      return $"{baseUrl}/api/{concatedParts}";
    }
  }
}