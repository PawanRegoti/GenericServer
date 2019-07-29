using Sample.Dal;

namespace Sample.App.Mappers
{
  public static class SampleMapper
  {
    public static SampleModel Map(SampleDto sampleDto)
    {
      if (sampleDto == null)
      {
        return null;
      }

      return new SampleModel
      {
        DocumentNr = sampleDto.DocumentNr,
        Name = sampleDto.Name,
        Address = sampleDto.Address,
        City = sampleDto.City,
        Country = sampleDto.Country
      };
    }

    public static SampleDto Map(int userId, SampleModel sampleModel)
    {
      if (sampleModel == null)
      {
        return null;
      }

      return new SampleDto(userId, sampleModel.DocumentNr)
      {
        Name = sampleModel.Name,
        Address = sampleModel.Address,
        City = sampleModel.City,
        Country = sampleModel.Country
      };
    }
  }
}
