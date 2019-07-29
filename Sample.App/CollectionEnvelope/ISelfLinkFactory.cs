namespace Sample.App.CollectionEnvelope
{
  public interface ISelfLinkFactory
  {
    string Build(params object[] parts);
  }
}